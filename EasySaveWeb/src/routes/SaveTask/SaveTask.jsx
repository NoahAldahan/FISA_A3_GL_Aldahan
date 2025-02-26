
import { useState, useEffect } from "react";
import SaveTaskDatagrid from "../../components/SaveTask/Datagrid/SaveTaskDatagrid";
import "./SaveTask.css"
import PopupCreateSaveTask from "../../components/SaveTask/Popup/Create/PopupCreateSaveTask";
import SaveTaskSocketService from "../../service/SaveTaskServiceWebSocket";

function SaveTask() {


  useEffect(() => {
    console.log("helloworld")
    const webSocket = new SaveTaskSocketService("ws://localhost:5000/api/ws");
    webSocket.connect();
  }, []);
  const [statePopupCreate, setStatePopupCreate]  = useState(false);
  const [updateDatagrid, setUpdateDatagrid] = useState(false);

    function onCloseCreatePopup()
    {
      setStatePopupCreate(false);
    }
    function onOpenCreatePopup()
    {
      setStatePopupCreate(true);
    }
    return (<div className="save-task">
        <SaveTaskDatagrid onOpenCreatePopup={onOpenCreatePopup} updateDatagrid={updateDatagrid} setUpdateDatagrid={setUpdateDatagrid} />
        <PopupCreateSaveTask statePopupCreate={statePopupCreate} onClose={onCloseCreatePopup} setUpdateDatagrid={setUpdateDatagrid} />
        </div>);
  }

export default SaveTask;
