import * as React from 'react';
import { useEffect, useState} from 'react';
import Box from '@mui/material/Box';
import { DataGrid } from '@mui/x-data-grid';
import { ThemeProvider, createTheme } from '@mui/material/styles';
import {darkThemeDatgrid } from "../../MuiTheme/DarkTheme"
import "./SaveTaskDataGrid.css"
import Button from '@mui/material/Button';


  // Définition des colonnes du DataGrid
  const columns = [
    { field: "id", headerName: "ID", width: 70, editable: true,},
    { field: "type", headerName: "Type de Sauvegarde", width: 200,editable: true, },
    { field: "name", headerName: "Nom", width: 150,editable: true, },
    { field: "sourcePath", headerName: "Chemin Source", width: 500,editable: true, },
    { field: "targetPath", headerName: "Chemin Cible", width: 500,editable: true, }
  ];

const handleRowUpdate = (updatedRow, originalRow) => {
  console.log("Ancienne valeur :", originalRow);
  console.log("Nouvelle valeur :", updatedRow);
  return updatedRow; 
};

const handleSuppressSaveTask = (selectedRows, rows) => {
  console.log(selectedRows);
  let deleteRow = [];
  let updatedRow = []; 
  selectedRows.forEach((value) => 
  {
    deleteRow = rows.filter(row => selectedRows.includes(row.id));
    updatedRow = rows.filter(row => !selectedRows.includes(row.id));
  });
  console.log(deleteRow);
  return updatedRow;
}

export default function SaveTaskDataGrid({onOpenCreatePopup}) 
{
  const [rows, setRows] = useState();
  const [selectedRows, setSelectedRows] = useState([]);
  useEffect(() => 
    { 
        const jsonData = [
          {"$type":"SaveTaskDifferential","CurrentDirectoryPair":{"SourcePath":"C:\\Users\\matte\\OneDrive\\Bureau\\CESI\\A3\\GenieLogiciel\\ProjetGenieLogiciel","TargetPath":"C:\\Users\\matte\\OneDrive\\Bureau\\CESI\\A3\\GenieLogiciel\\ProjetGenieLogiciel"},"name":"test"},{"$type":"SaveTaskDifferential","CurrentDirectoryPair":{"SourcePath":"C:\\Users\\basti\\Documents\\CESI\\GenieLogiciel\\workshop1\\TEST\\Boucle3","TargetPath":"C:\\Users\\basti\\Documents\\CESI\\GenieLogiciel\\workshop1\\TEST\\boucle3Copie"},"name":"fd"},{"$type":"SaveTaskDifferential","CurrentDirectoryPair":{"SourcePath":"C:\\Users\\jeanv\\Desktop\\Source","TargetPath":"C:\\Users\\jeanv\\Desktop\\Target"},"name":"Regis"},{"$type":"SaveTaskComplete","CurrentDirectoryPair":{"SourcePath":"C:\\Users\\jeanv\\Desktop\\Source","TargetPath":"C:\\Users\\jeanv\\Desktop\\Target"},"name":"Romain"},{"$type":"SaveTaskComplete","CurrentDirectoryPair":{"SourcePath":"C:\\Users\\jeanv\\Desktop\\Source","TargetPath":"C:\\Users\\jeanv\\Desktop\\Source"},"name":"Fred"},{"$type":"SaveTaskComplete","CurrentDirectoryPair":{"SourcePath":"C:\\Users\\jeanv\\Desktop\\Source","TargetPath":"C:\\Users\\jeanv\\Desktop\\Source"},"name":"JJJ"}
        ]
        // Transformer les données en ajoutant un ID unique
    // Transformer les données en ajoutant un ID unique
      setRows(jsonData.map((item, index) => ({
        id: index + 1, // Ajout d'un identifiant unique obligatoire pour le DataGrid
        type: item["$type"],
        name: item.name,
        sourcePath: item.CurrentDirectoryPair.SourcePath,
        targetPath: item.CurrentDirectoryPair.TargetPath
      })));

      console.log("Données transformées :", rows);
    }, []);

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
    </div>
    </div>);
}
