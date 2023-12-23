import { API_BASE_URL } from './services/sanasoppa-api.service';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { HttpClientModule } from '@angular/common/http';
import { HomeComponent } from './views/home/home.component';
import { JoinGameComponent } from './views/join-game/join-game.component';
import { LobbyComponent } from './views/lobby/lobby.component';
import { GameComponent } from './views/game/game.component';
import { ConfirmDeleteModalComponent } from './modals/confirm-delete-modal/confirm-delete-modal.component';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    JoinGameComponent,
    LobbyComponent,
    GameComponent,
    ConfirmDeleteModalComponent,
  ],
  imports: [BrowserModule, AppRoutingModule, NgbModule, HttpClientModule],
  providers: [{ provide: API_BASE_URL, useValue: `${window.location.origin}` }],
  bootstrap: [AppComponent],
})
export class AppModule {}
