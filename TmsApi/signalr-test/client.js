const signalR = require("@microsoft/signalr");

const connection = new signalR.HubConnectionBuilder()
  .withUrl("http://localhost:5063/hubs/notifications")
  .withAutomaticReconnect()
  .build();

connection.on("TranscriptCompleted", payload => {
  console.log("REAL-TIME EVENT RECEIVED");
  console.log(payload);
});

async function start() {
  try {
    await connection.start();
    console.log("Connected to SignalR hub");
  } catch (err) {
    console.error("Connection failed:", err);
    setTimeout(start, 2000);
  }
}

start();