const express = require('express');
const spdy = require('spdy');
const mongoose = require('mongoose');
const path = require('path');
const fs = require('fs');
const http2 = require('http2');


const app = express();

/* Configure Mongoose to Connect to MongoDB */
mongoose.connect('mongodb://127.0.0.1:27017/cms', { useNewUrlParser: true })
    .then(response => {
        console.log("MongoDB connected successfully.");
    })
    .catch(err => {
        console.log("Database connection failed.");
    });

// Enable HTTP/2
const server = spdy.createServer({
    key: fs.readFileSync('C:/Users/erzen/Documents/GitHub/CMS_Unity_Project/NodeJS/private-key.pem'),
    cert: fs.readFileSync('C:/Users/erzen/Documents/GitHub/CMS_Unity_Project/NodeJS/certificate.pem'),
}, app);




/* Configure Express */
app.use(express.json());
app.use(express.urlencoded({ extended: true }));
app.use(express.static(path.join(__dirname, 'public')));

/* Routes */
app.use('/', (req, res) => {
    res.send("Welcome to the CMS App");
});


app.listen(3000, () => {
    console.log(`Server is running on port 3000`);
});