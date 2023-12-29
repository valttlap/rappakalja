export const environment = {
  production: false,
  auth0: {
    domain: 'sanasoppa-dev.eu.auth0.com',
    clientId: 'OB61IDgXpiuxMp8rtltfONjoC0CPm8r9',
    authorizationParams: {
      audience: 'YOUR AUDIENCE',
      redirect_uri: 'YOUR CALLBACK URL',
    },
    errorPath: '/callback',
  },
};
