const express = require('express');
const app = express();
const { DefaultAzureCredential } = require('@azure/identity');
const { SecretClient } = require('@azure/keyvault-secrets');

const port = process.env.PORT || 3000;

const keyVaultName = "naserienkv1";
const keyVaultUrl = `https://${keyVaultName}.vault.azure.net/`;
const secretName = "Secret1";

const credential = new DefaultAzureCredential();
const client = new SecretClient(keyVaultUrl, credential);

app.use('/', express.static('frontend/build'));

app.get('/api', async (req, res) => {
    try {
        // Retrieve the secret from Azure Key Vault
        const secret = await client.getSecret(secretName);

        // Set the secret value as an environment variable
        process.env.KVSecret1 = secret.value;

        // Send the secret value as the response
        res.send(`Secret Value: ${process.env.KVSecret1}`);
    } catch (error) {
        console.error("Error retrieving secret:", error);
        res.status(500).send("Internal Server Error");
    }
});

app.listen(port, () => {
    console.log('Server listening on port ' + port);
});


// const express = require('express');
// const app = express();
// const port = process.env.PORT || 3000

// app.use('/', express.static('frontend/build'));

// app.get('/api', (req, res) => {
//   res.send('Hello, world!');
// });

// app.listen(port, () => {
//   console.log('Server listening on port '+ port);
// });
