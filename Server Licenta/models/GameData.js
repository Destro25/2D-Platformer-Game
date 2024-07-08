const saveGameData = async (db, userId, gameData) => {
    const [rows] = await db.execute(
        'REPLACE INTO game_data (user_id, data) VALUES (?, ?)',
        [userId, gameData]
    );
    return rows;
};

const loadGameData = async (db, userId) => {
    const [rows] = await db.execute(
        'SELECT data FROM game_data WHERE user_id = ?',
        [userId]
    );
    return rows[0];
};

module.exports = {
    saveGameData,
    loadGameData
};
