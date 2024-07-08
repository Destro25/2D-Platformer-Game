const pool = require('../db');

// Save Game Data
const saveGameData = async (req, res) => {
    try {
        const { userId, gameData } = req.body;

        const saveGameQuery = 'INSERT INTO game_data (user_id, game_data) VALUES (?, ?) ON DUPLICATE KEY UPDATE game_data = VALUES(game_data)';
        await pool.query(saveGameQuery, [userId, JSON.stringify(gameData)]);

        res.status(200).json({ msg: "Game data saved successfully" });
    } catch (error) {
        console.error("Error saving game data: ", error);
        res.status(500).json({ msg: "Server error" });
    }
};

// Load Game Data
const loadGameData = async (req, res) => {
    try {
        const { userId } = req.query;

        const loadGameQuery = 'SELECT game_data FROM game_data WHERE user_id = ?';
        const [gameDataResult] = await pool.query(loadGameQuery, [userId]);

        if (gameDataResult.length === 0) {
            return res.status(404).json({ msg: "Game data not found" });
        }

        res.status(200).json(JSON.parse(gameDataResult[0].game_data));
    } catch (error) {
        console.error("Error loading game data: ", error);
        res.status(500).json({ msg: "Server error" });
    }
};

module.exports = {
    saveGameData,
    loadGameData
};
