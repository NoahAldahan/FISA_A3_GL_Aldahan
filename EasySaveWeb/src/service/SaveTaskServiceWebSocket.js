// SaveTaskSocketService.js
class SaveTaskSocketService {
    constructor(url, observer) {
        this.url = url;
        this.socket = null;
        this.actionObservers = [];
    

      this.onOpen = () => console.log("WebSocket ouvert !");
      this.onClose = () => console.log("❌ WebSocket fermé !");
      this.onError = (err) => console.error("🚨 Erreur WebSocket :", err);
    }

    onMessage(event) {
      try {
        const data = JSON.parse(event.data);
          this.notifyActionObservers(data);
      } catch (error) {
        console.error("Erreur lors du parsing du JSON :", error);
      }
    }
  
    // Ajout d'un observateur pour les mises à jour de progression
    subscribeToAction(callback) {
      this.actionObservers.push(callback);
    }
  
    // Suppression d'un observateur
    unsubscribeFromAction(callback) {
      this.actionObservers = this.progressObservers.filter(obs => obs !== callback);
    }
  
    // Notifier tous les observateurs des mises à jour de progression
    notifyActionObservers(data) {
      console.log("notify", data);
      this.actionObservers.forEach(callback => callback(data));
    }
    
  
    connect() {
      this.socket = new WebSocket(this.url);
  
      this.socket.onopen = (event) => {
        this.onOpen(event);
      };
  
      this.socket.onmessage = (event) => this.onMessage(event);
  
      this.socket.onclose = (event) => {
        this.onClose(event);
      };
  
      this.socket.onerror = (error) => {
        this.onError(error);
      };
    }
    sendMessage(message) {
      console.log("message", message);
      if (this.socket?.readyState === WebSocket.OPEN) {
        this.socket.send(message);
      } else {
        console.error("⚠️ WebSocket non connecté ou en cours de connexion");
      }
    }

    
  
    close() {
      if (this.socket) {
        console.log("🛑 Fermeture du WebSocket...");
        this.socket.close();
      }
    }
  }
  
  export default SaveTaskSocketService;