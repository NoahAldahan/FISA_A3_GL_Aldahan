// SaveTaskSocketService.js
class SaveTaskSocketService {
    constructor(url) {
      this.url = url;
      this.socket = null;

      this.onOpen = () => this.sendMessage("hello world");
      this.onMessage = (msg) => console.log("📨 Message reçu :", msg);
      this.onClose = () => console.log("❌ WebSocket fermé !");
      this.onError = (err) => console.error("🚨 Erreur WebSocket :", err);
    }

    connect() {
      this.socket = new WebSocket(this.url);
  
      this.socket.onopen = (event) => {
        this.onOpen(event);
      };
  
      this.socket.onmessage = (event) => {
        const message = event.data;
        this.onMessage(message);
      };
  
      this.socket.onclose = (event) => {
        this.onClose(event);
      };
  
      this.socket.onerror = (error) => {
        this.onError(error);
      };
    }
  
    sendMessage(message) {
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