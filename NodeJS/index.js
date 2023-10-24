const express = require('express');
const spdy = require('spdy');
const mongoose = require('mongoose');
const path = require('path');
const fs = require('fs');
const WebSocket = require('ws');

const app = express();

/* Configure Mongoose to Connect to MongoDB */
mongoose.connect('mongodb://127.0.0.1:27017/cms', { useNewUrlParser: true })
    .then(() => {
        console.log("MongoDB connected successfully.");
    })
    .catch(err => {
        console.log("Database connection failed.", err);
    });

// Enable HTTP/2
const server = spdy.createServer({
    key: fs.readFileSync('E:/T-Unity-9/Documents/CMS_Unity_Project/NodeJS/private-key.pem'),
    cert: fs.readFileSync('E:/T-Unity-9/Documents/CMS_Unity_Project/NodeJS/certificate.pem'),
}, app);

// Create a WebSocket server attached to the HTTP server
const wss = new WebSocket.Server({ noServer: true });

// Handle WebSocket connections
wss.on('connection', (ws) => {
    console.log('Client connected');

    // Handle messages from Unity
    ws.on('message', (message) => {
        console.log(`Received message from Unity: ${message}`);

        // Process the message or perform any other actions

        // Send a response back to Unity
        ws.send(JSON.stringify({ message: 'Text received successfully.' }));
    });
});

// Upgrade an incoming HTTP request to a WebSocket connection
server.on('upgrade', (request, socket, head) => {
    wss.handleUpgrade(request, socket, head, (ws) => {
        wss.emit('connection', ws, request);
    });
});

/* Configure Express */
app.use(express.json());
app.use(express.urlencoded({ extended: true }));
app.use(express.static(path.join(__dirname, 'public')));

/* Routes */
app.get('/', (req, res) => {
    res.send("Welcome to the CMS App");
});

const PORT = process.env.PORT || 3000;

server.listen(PORT, () => {
    console.log(`Server is running on port ${PORT}`);
});
