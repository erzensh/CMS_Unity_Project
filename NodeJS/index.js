const express = require('express');
const spdy = require('spdy');
const mongoose = require('mongoose');
const path = require('path');
const fs = require('fs');
const WebSocket = require('ws');
const mysql = require('mysql');
const bodyParser = require('body-parser');


const app = express();
const port = process.env.PORT || 5000

app.use(bodyParser.urlencoded({ extended: false}))
app.use(bodyParser.json())
//MySQL
 const pool = mysql.createPool({
    connectionLimit : 10,
     host : '127.0.0.1',
     user : 'root',
     password : '',
     database : 'cmsapp'
 
    })


//Get all datas
app.get('', (req, res) => {
    pool.getConnection((err, connection) => {
        if (err) throw err;

        connection.query('SELECT name FROM datas', (err, rows) => {
            connection.release();

            if (!err) {
                res.json(rows);
            } else {
                console.log(err);
                res.status(500).send('Internal Server Error');
            }
        });
    });
});

app.get('/1', (req, res) => {
    pool.getConnection((err, connection) => {
        if (err) throw err;

        connection.query('SELECT description FROM datas', (err, rows) => {
            connection.release();

            if (!err) {
                res.json(rows);
            } else {
                console.log(err);
                res.status(500).send('Internal Server Error');
            }
        });
    });
});

app.get('/2', (req, res) => {
    pool.getConnection((err, connection) => {
        if (err) throw err;

        connection.query('SELECT url FROM datas', (err, rows) => {
            connection.release();

            if (!err) {
                res.json(rows);
            } else {
                console.log(err);
                res.status(500).send('Internal Server Error');
            }
        });
    });
});
//Listen on enviroment port or 5000
app.listen(port, () => console.log(`Listen on port ${port}`))


// Enable HTTP/2
const server = spdy.createServer({
    key: fs.readFileSync('E:/T-Unity-9/Documents/CMS_Unity_Project/NodeJS/private-key.pem'),
    cert: fs.readFileSync('E:/T-Unity-9/Documents/CMS_Unity_Project/NodeJS/certificate.pem'),
}, app);



/* Configure Express */
app.use(express.json());
app.use(express.urlencoded({ extended: true }));
app.use(express.static(path.join(__dirname, 'public')));





// /* Routes */
// app.use('/', (req, res) => {
//    res.send("Welcome to my CMS App");
// });

// app.listen(3000, () => {
//     console.log(`Server is running on port 3000`);
// });
