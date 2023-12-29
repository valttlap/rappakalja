import { API_BASE_URL } from './services/sanasoppa-api.service';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { HomeComponent } from './views/home/home.component';
import { JoinGameComponent } from './views/join-game/join-game.component';
import { LobbyComponent } from './views/lobby/lobby.component';
import { GameComponent } from './views/game/game.component';
import { ConfirmDeleteModalComponent } from './modals/confirm-delete-modal/confirm-delete-modal.component';
import { AuthHttpInterceptor, AuthModule } from '@auth0/auth0-angular';
import { environment } from '../environments/environment';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    JoinGameComponent,
    LobbyComponent,
    GameComponent,
    ConfirmDeleteModalComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    NgbModule,
    HttpClientModule,
    AuthModule.forRoot({
      domain: environment.auth0.domain,
      clientId: environment.auth0.clientId,
      authorizationParams: {
        redirect_uri: window.location.origin,
        audience: 'https://localhost:7020',
      },
      cacheLocation: 'localstorage',
      httpInterceptor: {
        allowedList: ['*'],
      },
    }),
  ],
  providers: [
    { provide: API_BASE_URL, useValue: `${window.location.origin}` },
    { provide: HTTP_INTERCEPTORS, useClass: AuthHttpInterceptor, multi: true },
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
