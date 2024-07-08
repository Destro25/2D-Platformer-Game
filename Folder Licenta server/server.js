const express = require('express');
const bodyParser = require('body-parser');
const authRoutes = require('./routes/authRoutes');
const gameDataRoutes = require('./routes/gameDataRoutes');

const app = express();
const port = 3000;

// Middleware
app.use(bodyParser.json()); // To parse JSON payloads

// Routes
app.use('/api/auth', authRoutes);
app.use('/api/gameData', gameDataRoutes);

// Start the server
app.listen(port, () => {
    console.log(`Server running on port ${port}`);
});
