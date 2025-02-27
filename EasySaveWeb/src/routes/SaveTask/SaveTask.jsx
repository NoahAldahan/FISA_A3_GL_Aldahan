
import { useState, useEffect } from "react";
import SaveTaskDatagrid from "../../components/SaveTask/Datagrid/SaveTaskDatagrid";
import "./SaveTask.css"
import PopupCreateSaveTask from "../../components/SaveTask/Popup/Create/PopupCreateSaveTask";
import SaveTaskSocketService from "../../service/SaveTaskServiceWebSocket";
import PopupSavingTasks from "../../components/SaveTask/Popup/SaveTask/PopupSavingTasks";

function SaveTask() {

  useEffect(() => {
    const webSocket = new SaveTaskSocketService("ws://localhost:5000/api/ws");
    webSocket.connect();
    setWebSocket(webSocket);
    return () => {
      webSocket.close();
    }
  }, []);
  const [statePopupCreate, setStatePopupCreate]  = useState(false);
  const [statePopupSavingTasks, setStatePopupSavingTasks] = useState(false);
  const [updateDatagrid, setUpdateDatagrid] = useState(false);
  const [webSocket, setWebSocket] = useState();

    function onCloseCreatePopup(){setStatePopupCreate(false);}
    function onOpenCreatePopup(){setStatePopupCreate(true);}
    function onCloseSavingTasksPopup(){setStatePopupSavingTasks(false);}
    function onOpenSavingTasksCreatePopup(){setStatePopupSavingTasks(true);}
    return (<div className="save-task">
        <SaveTaskDatagrid webSocket={webSocket} onOpenCreatePopup={onOpenCreatePopup} onOpenSavingTasksCreatePopup={onOpenSavingTasksCreatePopup}
        updateDatagrid={updateDatagrid} setUpdateDatagrid={setUpdateDatagrid} />
        <PopupCreateSaveTask statePopupCreate={statePopupCreate} onClose={onCloseCreatePopup} setUpdateDatagrid={setUpdateDatagrid} />
        <PopupSavingTasks webSocket={webSocket} open={statePopupSavingTasks} onClose={onCloseSavingTasksPopup} />
        </div>);
  }

export default SaveTask;
