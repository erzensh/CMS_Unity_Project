const express = require('express');
const mongoose = require('mongoose');
const path = require('path');

const app = express();

/* Configure Mongoose to Connect to MongoDB */
mongoose.connect('mongodb://127.0.0.1:27017/cms', {useNewUrlParser: true})
    .then(response => {
        console.log("MongoDB connected successfully.");
    }) .catch(err => {
        console.log("Database connection failed.");
    })

    
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