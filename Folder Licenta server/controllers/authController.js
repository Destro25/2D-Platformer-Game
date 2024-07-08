const bcrypt = require('bcrypt');
const jwt = require('jsonwebtoken');
const pool = require('../db');
const { jwtSecret } = require('../config/config');

// Register User
const registerUser = async (req, res) => {
    try {
        const { username, password } = req.body;

        if (!username || !password) {
            return res.status(400).json({ msg: "Please provide both username and password" });
        }

        // Check if user already exists
        const userCheckQuery = 'SELECT * FROM users WHERE username = ?';
        const [userCheckResult] = await pool.query(userCheckQuery, [username]);

        if (userCheckResult.length > 0) {
            return res.status(400).json({ msg: "User already exists" });
        }

        // Hash the password
        const salt = await bcrypt.genSalt(10);
        const hashedPassword = await bcrypt.hash(password, salt);

        // Insert new user into database
        const insertUserQuery = 'INSERT INTO users (username, password) VALUES (?, ?)';
        await pool.query(insertUserQuery, [username, hashedPassword]);

        res.status(201).json({ msg: "User registered successfully" });
    } catch (error) {
        console.error("Error during user registration: ", error);
        res.status(500).json({ msg: "Server error" });
    }
};

// Login User
const loginUser = async (req, res) => {
    try {
        const { username, password } = req.body;

        if (!username || !password) {
            return res.status(400).json({ msg: "Please provide both username and password" });
        }

        // Check if user exists
        const userCheckQuery = 'SELECT * FROM users WHERE username = ?';
        const [userCheckResult] = await pool.query(userCheckQuery, [username]);

        if (userCheckResult.length === 0) {
            return res.status(400).json({ msg: "User not found" });
        }

        const user = userCheckResult[0];
        const isMatch = await bcrypt.compare(password, user.password);

        if (!isMatch) {
            return res.status(400).json({ msg: "Invalid credentials" });
        }

        // Generate JWT token
        const token = jwt.sign({ id: user.id, username: user.username }, jwtSecret, { expiresIn: '1h' });

        res.status(200).json({ token });
    } catch (error) {
        console.error("Error during user login: ", error);
        res.status(500).json({ msg: "Server error" });
    }
};

module.exports = {
    registerUser,
    loginUser
};
