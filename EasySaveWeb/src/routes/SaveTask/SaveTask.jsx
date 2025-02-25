
import { useState } from "react";
import SaveTaskDatagrid from "../../components/SaveTask/Datagrid/SaveTaskDatagrid";
import "./SaveTask.css"
import PopupCreateSaveTask from "../../components/SaveTask/Popup/Create/PopupCreateSaveTask";

function SaveTask() {

  const [statePopupCreate, setStatePopupCreate]  = useState(false);

    function onCloseCreatePopup(){
      setStatePopupCreate(false);
    }
    function onOpenCreatePopup(){
      setStatePopupCreate(true);
    }
    return (<div className="save-task">
        <SaveTaskDatagrid onOpenCreatePopup={onOpenCreatePopup} />
        <PopupCreateSaveTask statePopupCreate={statePopupCreate} onClose={onCloseCreatePopup}  />
        </div>);
  }

export default SaveTask;
