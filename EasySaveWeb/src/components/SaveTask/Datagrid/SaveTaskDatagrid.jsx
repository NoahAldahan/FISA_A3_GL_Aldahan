import * as React from 'react';
import { useEffect, useState} from 'react';
import Box from '@mui/material/Box';
import { DataGrid } from '@mui/x-data-grid';
import { ThemeProvider, createTheme } from '@mui/material/styles';
import {darkThemeDatgrid} from "../../MuiTheme/DarkTheme"
import "./SaveTaskDataGrid.css"
import Button from '@mui/material/Button';
import SaveTaskService from '../../../service/SaveTaskService'


const columns = [
  { field: "id", headerName: "ID", width: 70, editable: false },
  {
    field: "type",
    headerName: "Type de Sauvegarde",
    width: 200,
    editable: true,
    type: 'singleSelect', // Type ComboBox
    valueOptions: ["Complete", "Differential"], // Options disponibles
  },
  { field: "name", headerName: "Nom", width: 150, editable: true },
  { field: "sourcePath", headerName: "Chemin Source", width: 500, editable: true },
  { field: "targetPath", headerName: "Chemin Cible", width: 500, editable: true },
];

const handleRowUpdate = (updatedRow, originalRow) => {
  console.log("Ancienne valeur :", originalRow);
  console.log("Nouvelle valeur :", updatedRow);
  let changes = {};
  Object.keys(updatedRow).forEach((key) => {
    console.log("key : ", key)
    if(updatedRow[key] !== originalRow[key]){
      changes["name"] = originalRow["name"];
      changes["field"] = key;
      changes["newvalue"] =  updatedRow[key];
    }
  });
  SaveTaskService.updateTask(changes).then((value)=>{
    console.log(value);
  })
  return updatedRow; 
};

const handleSuppressSaveTask = (selectedRows, rows) => {
  console.log(selectedRows);
  let deleteRowName = [];
  let updatedRow = []; 
  if(selectedRows.length == 0){
    return rows;
  }
  selectedRows.forEach((value) => 
  {
    deleteRowName = rows.filter(row => selectedRows.includes(row.id)).map(row => row.name); // 🔥 Transforme en une liste de noms;
    updatedRow = rows.filter(row => !selectedRows.includes(row.id));
  });
  console.log(deleteRowName);
  SaveTaskService.deleteTask(deleteRowName);
  return updatedRow;
}

const handleStartSaveTask = (webSocket, selectedRows, rows) => {
  console.log(selectedRows);
  let saveTaskToStart = [];

  selectedRows.forEach((value) => 
    {
      saveTaskToStart = rows.filter(row => selectedRows.includes(row.id)).map(row => row.name); 
    });
    console.log(saveTaskToStart);
    webSocket.sendMessage(
      JSON.stringify  ({
        "Action": "Start",
        "Names": saveTaskToStart  
      }));
  } 


const GetSaveTaskTypeToStr = (type) => {
  switch(type){
    case 1:
      return "Differential"
    case 2:
      return "Complete"
  }
} 

const GetSaveTaskTypeToInt = (type) =>
{
  switch(type){
    case "Differential":
      return 1
    case "Complete":
      2
  }
}

export default function SaveTaskDataGrid({onOpenCreatePopup,updateDatagrid,setUpdateDatagrid,webSocket}) 
{
  const [rows, setRows] = useState();
  const [selectedRows, setSelectedRows] = useState([]);
  const handleLoadDatagridRows = () => {
    SaveTaskService.getAllTasks().then(value => {
      console.log(value);
      setRows(value.map((item, index) => ({
        id: index + 1, // Ajout d'un identifiant unique obligatoire pour le DataGrid
        type: GetSaveTaskTypeToStr(item.BindSaveTaskType),
        name: item.BindName,
        sourcePath: item.BindSource,
        targetPath: item.BindDestination
      })));
    })
  }

  useEffect(() => { 
    handleLoadDatagridRows();
    if(updateDatagrid)
    {
      setUpdateDatagrid(false);
    }
  }, [updateDatagrid]);

  return (<div className="savetask-datagrid">
    <h2>Grid Save Task</h2>
    <ThemeProvider theme={darkThemeDatgrid}>
    <Box sx={{ height: 'auto', width: '100%'}}>
      <DataGrid
        item xs={100} sm={20} md={20}
        rows={rows}
        columns={columns}
        initialState={{
          pagination: {
            paginationModel: {
              pageSize: 5,
            },
          },
        }}
        pageSizeOptions={[5]}
        processRowUpdate={(updatedRow, originalRow) => {return handleRowUpdate(updatedRow, originalRow)}}
        onRowSelectionModelChange={(newSelection) => setSelectedRows(newSelection)}
        checkboxSelection
        disableRowSelectionOnClick
      />
    </Box>
    </ThemeProvider>
    <div className="savetask-action">
    <Button onClick={() => {onOpenCreatePopup()}} class='mui-btn' variant="contained">Create SaveTask</Button>
    <Button onClick={() => {setRows(handleSuppressSaveTask(selectedRows, rows))}} class='mui-btn' variant="contained">Delete SaveTask</Button>
    <Button onClick={() => {handleStartSaveTask(webSocket, selectedRows, rows)}} class='mui-btn' variant="contained">Start SaveTask</Button>
    </div>
    </div>);
}
