// server.js
const express = require('express');
const bodyParser = require('body-parser');
const authRoutes = require('./routes/authRoutes');
const gameDataRoutes = require('./routes/gameDataRoutes');

const app = express();

app.use(bodyParser.json());
app.use('/api/auth', authRoutes);
app.use('/api/gameData', gameDataRoutes);

const port = process.env.PORT || 3000;
app.listen(port, () => {
    console.log(`Server running on port ${port}`);
});
