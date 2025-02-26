
import { useState, useEffect } from "react";
import SaveTaskDatagrid from "../../components/SaveTask/Datagrid/SaveTaskDatagrid";
import "./SaveTask.css"
import PopupCreateSaveTask from "../../components/SaveTask/Popup/Create/PopupCreateSaveTask";
import SaveTaskSocketService from "../../service/SaveTaskServiceWebSocket";

function SaveTask() {

  useEffect(() => {
    const webSocket = new SaveTaskSocketService("ws://localhost:5000/api/ws");
    webSocket.connect();
    setWebSocket(webSocket);
    // return () => {
    //   console.log("WebSocket closed");
    //   webSocket.close(); // Assurez-vous que votre service a une méthode de déconnexion
    // };
  }, []);
  const [statePopupCreate, setStatePopupCreate]  = useState(false);
  const [updateDatagrid, setUpdateDatagrid] = useState(false);
  const [webSocket, setWebSocket] = useState();

    function onCloseCreatePopup()
    {
      setStatePopupCreate(false);
    }
    function onOpenCreatePopup()
    {
      setStatePopupCreate(true);
    }
    return (<div className="save-task">
        <SaveTaskDatagrid webSocket={webSocket} onOpenCreatePopup={onOpenCreatePopup} updateDatagrid={updateDatagrid} setUpdateDatagrid={setUpdateDatagrid} />
        <PopupCreateSaveTask statePopupCreate={statePopupCreate} onClose={onCloseCreatePopup} setUpdateDatagrid={setUpdateDatagrid} />
        </div>);
  }

export default SaveTask;
