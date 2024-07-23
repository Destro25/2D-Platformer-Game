// /controllers/authController.js
const express = require('express');
const bcrypt = require('bcryptjs');
const jwt = require('jsonwebtoken');
const db = require('../db');

const registerUser = async (req, res) => {
    const { username, password } = req.body;

    try {
        const hashedPassword = await bcrypt.hash(password, 10);
        const sql = 'INSERT INTO users (username, password) VALUES (?, ?)';
        const [result] = await db.query(sql, [username, hashedPassword]);

        const token = jwt.sign({ id: result.insertId, username: username }, 'your_jwt_secret_key', { expiresIn: '1h' });
        res.json({ token });
    } catch (err) {
        console.error('Error registering user: ', err);
        res.status(500).send('Internal server error');
    }
};

const loginUser = async (req, res) => {
    const { username, password } = req.body;

    try {
        const sql = 'SELECT * FROM users WHERE username = ?';
        const [rows] = await db.query(sql, [username]);

        if (rows.length === 0) {
            return res.status(400).send('Invalid username or password');
        }

        const user = rows[0];
        const validPassword = await bcrypt.compare(password, user.password);
        if (!validPassword) {
            return res.status(400).send('Invalid username or password');
        }

        const token = jwt.sign({ id: user.id, username: user.username }, 'your_jwt_secret_key', { expiresIn: '1h' });
        res.json({ token });
    } catch (err) {
        console.error('Error logging in user: ', err);
        res.status(500).send('Internal server error');
    }
};

module.exports = { registerUser, loginUser };
