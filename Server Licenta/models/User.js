const bcrypt = require('bcryptjs');

const createUser = async (db, username, password) => {
    const hashedPassword = await bcrypt.hash(password, 10);
    const [result] = await db.execute(
        'INSERT INTO users (username, password) VALUES (?, ?)',
        [username, hashedPassword]
    );
    return result;
};

const findUserByUsername = async (db, username) => {
    const [rows] = await db.execute(
        'SELECT * FROM users WHERE username = ?',
        [username]
    );
    return rows[0];
};

module.exports = {
    createUser,
    findUserByUsername
};
