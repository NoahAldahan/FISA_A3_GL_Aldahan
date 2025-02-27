import React, { useEffect, useState } from "react";
import { Dialog, DialogTitle, DialogContent, DialogActions, Button, List, ListItem, LinearProgress, Typography } from "@mui/material";
import { ThemeProvider } from "@mui/material/styles";
import { darkThemePopup } from "../../../MuiTheme/DarkTheme";
import { Box } from "@mui/material";

const PopupSavingTasks = ({ open, onClose, webSocket }) => {

    const [savetasks, setSaveTasks] = useState([]);

    
    const handlePauseTasks = (saveTaskName) => {
        // Envoi du message WebSocket
        webSocket.sendMessage(
            JSON.stringify({
                "Action": "Pause",
                "Names": [saveTaskName]  
            })
        );
    };
    
    const handlePlayTasks = (saveTaskName) => {
        // Envoi du message WebSocket
        webSocket.sendMessage(
            JSON.stringify({
                "Action": "Play",
                "Names": [saveTaskName]  
            })
        );
    };

    const handleStopTasks = (saveTaskName) => {
        // Envoi du message WebSocket
        webSocket.sendMessage(
            JSON.stringify({
                "Action": "Stop",
                "Names": [saveTaskName]  
            })
        );
    };


    useEffect(() => {
        if (webSocket) {
          const handleAction = (data) => {
            switch(data.Action)
            {
                case "StartSaveTasks":{
                    console.log("start");
                    const updatedSaveTasks = JSON.parse(data.SaveTasks).map(task => ({
                        ...task,  // Copie toutes les propriétés existantes
                        Progress: task.Progress || 0  // Ajoute une propriété Progress (valeur par défaut : 0)
                    }));
                    setSaveTasks(updatedSaveTasks);
                    break;
                }
                case "ProgressUpdate": {
                    console.log("update", data);
                    setSaveTasks(prevTasks => {
                        const updatedTasks = prevTasks.map(task => 
                            task.name === data.TaskName 
                                ? { ...task, Progress: data.Progress }
                                : task
                        );
                    
                        return [...updatedTasks];  // 🔥 On crée un nouvel array pour forcer React à détecter le changement
                    });  
                    break;
                }
                case "StateUpdate": {
                    console.log("before : ", savetasks);
                    setSaveTasks(prevTasks => {
                        console.log("prev", prevTasks)
                        const updatedTasks = prevTasks.map(task => 
                            task.name === data.TaskName 
                                ? { ...task, BindState: data.State }
                                : task
                        );
                        return [...updatedTasks];  // 🔥 On crée un nouvel array pour forcer React à détecter le changement
                    });  
                    console.log("after : ", savetasks);
                    break;
                }
                default:{
                    console.log("default");
                    break;
                }
            }
          };
          // S'abonner aux mises à jour WebSocket
          webSocket.subscribeToAction(handleAction);
      
          return () => {
            // Se désabonner proprement pour éviter les fuites de mémoire
            webSocket.unsubscribeFromAction(handleAction);
          };
        }
      }, [webSocket]);
  return (
    <ThemeProvider theme={darkThemePopup}>
    <Dialog open={open} onClose={onClose} maxWidth="sm" fullWidth>
      <DialogTitle>Saving Tasks</DialogTitle>
      <DialogContent>
        <List>
          {savetasks.map((task, index) => (
            <ListItem key={index} sx={{ width: "100%", padding: 0 }}>
              {/* 🔹 Encapsule chaque item dans une Box stylisée */}
              <Box 
                sx={{
                  width: "100%",
                  padding: 2,
                  backgroundColor: "rgba(255, 255, 255, 0.05)",  // Légèrement différent du fond
                  borderRadius: 2,
                  boxShadow: "0px 2px 8px rgba(0, 0, 0, 0.2)", // Ajoute une ombre légère
                  display: "flex",
                  flexDirection: "column",
                  alignItems: "flex-start",
                  gap: 1, // Espacement uniforme entre les éléments
                  marginBottom: 2 // Espacement entre les Box
                }}
              >
                <Typography variant="h6" sx={{ fontWeight: "bold", color: "#fff" }}>
                {task.BindName}
                </Typography>
                <Typography variant="body2" sx={{ color: "#ccc" }}>
                Progress: {parseInt(task.Progress)}%
                </Typography>
                {/* color: task.BindState === "Success" ? "#4CAF50" : task.BindState === "Error" ? "#FF5252" : "#FFC107" }}> */}
                <Typography variant="body2" sx={{ fontStyle: "italic", color: "#ccc"}}> 
                State: {task.BindState}
                </Typography>
                <LinearProgress
                variant="determinate"
                value={parseFloat(task.Progress)}
                sx={{
                    width: "100%",
                    height: 8,
                    borderRadius: 4,
                    backgroundColor: "rgba(255, 255, 255, 0.2)",
                    "& .MuiLinearProgress-bar": { backgroundColor: "#4CAF50" } // Personnalisation de la barre de progression
                }}
                />
                <Box sx={{display: "flex"}}>
                <Button
                  sx={{
                    backgroundColor: "rgb(89, 89, 183)",
                    color: "#FFFF",
                    marginTop: 1,
                    alignSelf: "center", // Centre le bouton
                    "&:hover": { backgroundColor: "rgb(69, 69, 153)" } // Effet au survol
                  }}
                  onClick={() => {handlePauseTasks(task.BindName)}}
                >
                  Pause
                </Button>
                <Button
                  sx={{
                    backgroundColor: "rgb(89, 89, 183)",
                    color: "#FFFF",
                    marginTop: 1,
                    marginLeft: 2,
                    alignSelf: "center", // Centre le bouton
                    "&:hover": { backgroundColor: "rgb(69, 69, 153)" } // Effet au survol
                  }}
                  onClick={() => {handlePlayTasks(task.BindName)}}
                >
                  Play
                </Button>
                <Button
                  sx={{
                    backgroundColor: "rgb(89, 89, 183)",
                    color: "#FFFF",
                    marginTop: 1,
                    marginLeft: 2,
                    alignSelf: "center", // Centre le bouton
                    "&:hover": { backgroundColor: "rgb(69, 69, 153)" } // Effet au survol
                  }}
                  onClick={() => {handleStopTasks(task.BindName)}}
                >
                  Stop
                </Button>
                </Box>
              </Box>
            </ListItem>
          ))}
        </List>
      </DialogContent>
      <DialogActions>
        <Button onClick={onClose} color="primary" variant="contained">Close</Button>
      </DialogActions>
    </Dialog>
  </ThemeProvider>
  );
};

export default PopupSavingTasks;