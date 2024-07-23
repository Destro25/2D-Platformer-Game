// /routes/gameDataRoutes.js
const express = require('express');
const { saveGameData, loadGameData } = require('../controllers/gameDataController');
const auth = require('../middleware/auth');

const router = express.Router();

router.post('/save', auth, saveGameData);
router.get('/load', auth, loadGameData);

module.exports = router;
