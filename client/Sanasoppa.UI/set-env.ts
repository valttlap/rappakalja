/* eslint-disable @typescript-eslint/no-var-requires */
const fs = require('fs');
const dotenv = require('dotenv');

// Load environment variables from .env file
dotenv.config();

const isProd = process.env['BUILD_ENV'] === 'production';

// Define the content of the Angular environment file
const envConfigFile = `export const environment = {
  production: ${isProd},
  auth0: {
    domain: '${process.env['AUTH0_DOMAIN']}',
    clientId: '${process.env['AUTH0_CLIENT_ID']}',
    authorizationParams: {
      audience: '${process.env['AUTH0_AUDIENCE']}',
    },
  },
};
`;

const envFileName = isProd ? 'environment.ts' : 'environment.development.ts';

// Write the file to the Angular environment directory
fs.writeFileSync(`./src/environments/${envFileName}`, envConfigFile);

console.log(
  `Angular environment.ts file generated correctly at ${new Date().toISOString()}`
);
