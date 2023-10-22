const express = require('express');
const mongoose = require('mongoose');
const path = require('path');

const app = express();

/* Configure Mongoose to Connect to MongoDB */
mongoose.connect('mongodb://localhost:27017/cms', { useNewUrlParser: true, useUnifiedTopology: true });

const db = mongoose.connection;

db.on('error', console.error.bind(console, 'MongoDB connection error:'));

db.once('open', () => {
    console.log('MongoDB Connected Successfully.');
});


/* Configure Express */
app.use(express.json());
app.use(express.urlencoded({ extended: true }));
app.use(express.static(path.join(__dirname, 'public')));


/* Routes */
app.use('/', (req, res) =>{
    res.send("Welcome to the CMS App");
});




app.listen(3000, () => {
    console.log(`Server is running on port 3000`);
});