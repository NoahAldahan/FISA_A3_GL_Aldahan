const API_URL = "http://localhost:5000"; // Remplace par l'URL réelle de ton API

const SaveTaskService = {
  // Récupérer toutes les tâches
  async getAllTasks() {
    try {
      const response = await fetch(API_URL + "/api/getsavetasks");
      if (!response.ok) throw new Error("Erreur lors de la récupération des tâches");
      {
        return await response.json();
      }
    } catch (error) {
      console.error("Erreur GET:", error);
      return null;
    }
  },

  // Ajouter une nouvelle tâche
  async createTask(taskData) 
  {
    try {
      const response = await fetch(API_URL + "/api/addsavetask", {
        method: "POST",
        headers: {
          "Content-Type": "application/json"
        },
        body: JSON.stringify(taskData)
      });

      if (!response.ok) throw new Error("Erreur lors de la création de la tâche");
      return await response.json();
    } catch (error) {
      console.error("Erreur POST:", error);
      return null;
    }
  },

  // Mettre à jour une tâche existante
  async updateTask(updatedData) {
    try {
      const response = await fetch(API_URL + "/api/sendmodification", {
        method: "PUT",
        headers: {
          "Content-Type": "application/json"
        },
        body: JSON.stringify(updatedData)
      });

      if (!response.ok) throw new Error("Erreur lors de la mise à jour de la tâche");
      return await response.json();
    } catch (error) {
      console.error("Erreur PUT:", error);
      return null;
    }
  },

  // Supprimer une tâche
  async deleteTask(taskIds) {
    try {
      const response = await fetch(API_URL + "/api/deletesavetasks", 
      {
        method: "DELETE",
        headers: {
          "Content-Type": "application/json"
        },
        body: JSON.stringify(taskIds)
      });

      if (!response.ok) throw new Error("Erreur lors de la suppression de la tâche");
      return { success: true };
    } catch (error) {
      console.error("Erreur DELETE:", error);
      return null;
    }
  }
};

export default SaveTaskService;
