import React, { useState } from "react";
import { Dialog, DialogTitle, DialogContent, DialogActions, TextField, Button, Select, MenuItem} from "@mui/material";
import { ThemeProvider } from "@mui/material/styles";
import { darkThemePopup } from "../../../MuiTheme/DarkTheme";
import SaveTaskService from "../../../../service/SaveTaskService";

const SaveTaskPopup = ({statePopupCreate, onClose, setUpdateDatagrid}) => {
  // 🛠 State pour stocker les valeurs des champs
  const [taskData, setTaskData] = useState({
    type: "",
    name: "",
    sourcePath: "",
    targetPath: "",
  });

  // 🔄 Gérer le changement des champs
  const handleChange = (event) => {
    setTaskData({ ...taskData, [event.target.name]: event.target.value });
  };

  // ✅ Sauvegarder et envoyer les données
  const handleSave = () => {
    if (!taskData.name || !taskData.type || !taskData.sourcePath || !taskData.targetPath) {
      alert("Tous les champs doivent être remplis !");
      return;
    }
    SaveTaskService.createTask(taskData).then(setUpdateDatagrid(true)).catch();
    onClose();
  };

  return (
    <ThemeProvider theme={darkThemePopup}>
      <Dialog open={statePopupCreate} onClose={onClose} fullWidth maxWidth="sm">
        <DialogTitle>Créer une tâche</DialogTitle>
        <DialogContent sx={{ display: "flex",flexDirection: "column", justifyContent: "center", alignItems: "center", textAlign: "center"}}>
          <TextField    
            sx={{width: "60%"}}
            label="Nom de la tâche"
            variant="outlined"
            margin="dense"
            name="name"
            value={taskData.name}
            onChange={handleChange}
          />
          <TextField
            sx={{width: "60%"}}
            label="Chemin Source"
            variant="outlined"
            margin="dense"
            name="sourcePath"
            value={taskData.sourcePath}
            onChange={handleChange}
          />
          <TextField
            sx={{width: "60%"}}
            label="Chemin Cible"
            variant="outlined"
            margin="dense"
            name="targetPath"
            value={taskData.targetPath}
            onChange={handleChange}
          />
        <Select
            sx={{width: "60%"}}
            variant="outlined"
            name="type"
            value={taskData.type}
            onChange={handleChange}
            >
            <MenuItem value="Complete">Complete</MenuItem>
            <MenuItem value="Differential">Différentiel</MenuItem>
        </Select>
        </DialogContent>
        <DialogActions>
          <Button onClick={onClose} color="secondary">
            Annuler
          </Button>
          <Button onClick={handleSave} color="primary" variant="contained">
            Sauvegarder
          </Button>
        </DialogActions>
      </Dialog>
    </ThemeProvider>
  );
};

export default SaveTaskPopup;
