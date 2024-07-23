const express = require('express');
const { saveGameData, loadGameData } = require('../controllers/gameDataController');

const router = express.Router();

router.post('/save', saveGameData);
router.get('/load', loadGameData);

module.exports = router;
