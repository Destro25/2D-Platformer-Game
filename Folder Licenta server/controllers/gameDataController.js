// /controllers/gameDataController.js
const db = require('../db');

const saveGameData = async (req, res) => {
    const userId = req.user.id;
    const gameData = req.body;

    if (!gameData) {
        return res.status(400).send('Game data is required');
    }

    try {
        const sql = `
            INSERT INTO game_data (user_id, game_data)
            VALUES (?, ?)
            ON DUPLICATE KEY UPDATE
            game_data = VALUES(game_data)
        `;
        await db.query(sql, [userId, JSON.stringify(gameData)]);
        res.send('Game data saved successfully');
    } catch (err) {
        console.error('Error saving game data: ', err);
        res.status(500).send('Internal server error');
    }
};

const loadGameData = async (req, res) => {
    const userId = req.user.id;

    try {
        const sql = 'SELECT game_data FROM game_data WHERE user_id = ?';
        const [rows] = await db.query(sql, [userId]);

        if (rows.length > 0) {
            res.json(JSON.parse(rows[0].game_data));
        } else {
            res.status(404).send('No game data found for this user');
        }
    } catch (err) {
        console.error('Error loading game data: ', err);
        res.status(500).send('Internal server error');
    }
};

module.exports = {
    saveGameData,
    loadGameData,
};
