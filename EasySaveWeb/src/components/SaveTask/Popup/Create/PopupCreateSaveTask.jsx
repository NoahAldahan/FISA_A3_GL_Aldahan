import React, { useState } from "react";
import { Dialog, DialogTitle, DialogContent, DialogActions, TextField, Button } from "@mui/material";
import { ThemeProvider } from "@mui/material/styles";
import { darkThemePopup } from "../../../MuiTheme/DarkTheme";

const SaveTaskPopup = ({statePopupCreate, onClose}) => {
  // 🛠 State pour stocker les valeurs des champs
  const [taskData, setTaskData] = useState({
    type: "",
    name: "",
    sourcePath: "",
    targetPath: "",
  });

  function handleCreateSaveTask(){
    console.log(taskData);
  }

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
    handleCreateSaveTask(); // Envoie les données au parent
    onClose();
  };

  return (
    <ThemeProvider theme={darkThemePopup}>
      <Dialog open={statePopupCreate} onClose={onClose} fullWidth maxWidth="sm">
        <DialogTitle>Créer une tâche</DialogTitle>
        <DialogContent>
          <TextField
            fullWidth
            label="Type de Sauvegarde"
            variant="outlined"
            margin="dense"
            name="type"
            value={taskData.type}
            onChange={handleChange}
          />
          <TextField
            fullWidth
            label="Nom de la tâche"
            variant="outlined"
            margin="dense"
            name="name"
            value={taskData.name}
            onChange={handleChange}
          />
          <TextField
            fullWidth
            label="Chemin Source"
            variant="outlined"
            margin="dense"
            name="sourcePath"
            value={taskData.sourcePath}
            onChange={handleChange}
          />
          <TextField
            fullWidth
            label="Chemin Cible"
            variant="outlined"
            margin="dense"
            name="targetPath"
            value={taskData.targetPath}
            onChange={handleChange}
          />
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
