// /controllers/gameDataController.js
const db = require('../db');

const saveGameData = async (req, res) => {
    const userId = req.user.id;
    const { level, gameData } = req.body;

    if (!gameData || !level) {
        return res.status(400).send('Level and game data are required');
    }

    try {
        const sql = `
            INSERT INTO game_data (user_id, level, game_data)
            VALUES (?, ?, ?)
            ON DUPLICATE KEY UPDATE
            game_data = VALUES(game_data)
        `;
        await db.query(sql, [userId, level, JSON.stringify(gameData)]);
        res.send('Game data saved successfully');
    } catch (err) {
        console.error('Error saving game data: ', err);
        res.status(500).send('Internal server error');
    }
};

const loadGameData = async (req, res) => {
    const userId = req.user.id;

    try {
        const [rows] = await db.query('SELECT level, game_data FROM game_data WHERE user_id = ?', [userId]);
        if (rows.length === 0) {
            return res.status(404).json({ error: 'No game data found' });
        }
        console.log(rows.length);
        console.log(rows);

        res.json(rows);
    } catch (error) {
        console.error('Error loading game data: ', error);
        res.status(500).json({ error: 'Failed to load game data' });
    }
};

module.exports = { saveGameData, loadGameData };
