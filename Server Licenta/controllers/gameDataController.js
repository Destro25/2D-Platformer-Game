const GameData = require('../models/GameData');

exports.saveGameData = async (req, res) => {
    const { gameData } = req.body;
    try {
        const db = req.app.locals.db;
        const user = req.user.username;
        await GameData.saveGameData(db, user, gameData);
        res.json({ msg: 'Game data saved successfully' });
    } catch (err) {
        console.error(err.message);
        res.status(500).send('Server error');
    }
};

exports.loadGameData = async (req, res) => {
    try {
        const db = req.app.locals.db;
        const user = req.user.username;
        const data = await GameData.loadGameData(db, user);
        if (!data) return res.status(404).json({ msg: 'No game data found' });
        res.json(data);
    } catch (err) {
        console.error(err.message);
        res.status(500).send('Server error');
    }
};
