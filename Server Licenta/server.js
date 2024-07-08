const express = require('express');
const connectDB = require('./config/db');
const dotenv = require('dotenv');
const cors = require('cors');
const bodyParser = require('body-parser');

dotenv.config();

const app = express();

// Middleware
app.use(cors());
app.use(bodyParser.json());

// Define Routes
app.use('/api/auth', require('./routes/auth'));
app.use('/api/gameData', require('./routes/gameData'));

// Connect Database
connectDB().then(connection => {
    app.locals.db = connection;
});

// Start Server
const PORT = process.env.PORT || 3000;
app.listen(PORT, () => console.log(`Server started on port ${PORT}`));
