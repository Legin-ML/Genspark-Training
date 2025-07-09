const express = require('express');
const path = require('path');

const app = express();
const port = process.env.PORT || 3000;

const distFolder = path.join(__dirname, 'dist', 'true-feedback-fe', 'browser');

app.use(express.static(distFolder));

app.get('/{*any}',(req, res) => {
  res.sendFile(path.join(distFolder, 'index.html'));
});

app.listen(port, () => {
  console.log(`App is running on http://localhost:${port}`);
});
