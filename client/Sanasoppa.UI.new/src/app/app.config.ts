import {
  APP_INITIALIZER,
  ApplicationConfig,
  importProvidersFrom,
} from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { HTTP_INTERCEPTORS, provideHttpClient } from '@angular/common/http';
import {
  AuthClientConfig,
  AuthConfig,
  AuthHttpInterceptor,
  AuthModule,
} from '@auth0/auth0-angular';
import { Client } from './services/sanasoppa-api.service';
import { firstValueFrom } from 'rxjs';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

function intializeAppFactory(client: Client, authClient: AuthClientConfig) {
  return async () => {
    const settings = await firstValueFrom(client.getAuth0Settings());
    const config: AuthConfig = {
      domain: settings.domain,
      clientId: settings.clientId,
      authorizationParams: {
        redirect_uri: window.location.origin,
        audience: settings.audience,
      },
      cacheLocation: 'localstorage',
      httpInterceptor: {
        allowedList: ['*'],
      },
    };
    authClient.set(config);
  };
}

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes),
    { provide: HTTP_INTERCEPTORS, useClass: AuthHttpInterceptor, multi: true },
    importProvidersFrom(AuthModule.forRoot()),
    provideHttpClient(),
    { provide: Client, useClass: Client },
    {
      provide: APP_INITIALIZER,
      useFactory: intializeAppFactory,
      deps: [Client, AuthClientConfig],
      multi: true,
    },
    importProvidersFrom(NgbModule),
  ],
};
