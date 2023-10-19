/* Importing Different Modules */

const express = require('express');
const mongoose = require('mongoose');
const path = require('path');
//const hbs = require('express-handlebars');

const app = express();


//Configure Mongoose to Connect to MongoDB
mongoose.connect('mongodb://localhost:27017/cms_project', {useNewUrlParser: true })
    .then(response => {
        console.log("MongoDB connected successfully");
    }) .catch(err => {
        console.log("Database connection failed");
    })



/* Configure express */
app.use(express.json());
app.use(express.urlencoded({extended: true}));
app.use(express.static(path.join(__dirname, 'public')));


/* Setup View Engine To Use Handlebars */
//app.engine('handlebars', hbs({defaultLayout: 'default'}))



/* Routes */
app.use('/', (req, res) => {
    res.send("Welcome to the CMS App");
});





app.listen(3000, () => {
    console.log('Server is running on port 3000');
})