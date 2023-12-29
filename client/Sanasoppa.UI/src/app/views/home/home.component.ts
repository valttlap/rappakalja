import { Component, effect } from '@angular/core';
import { GameHubService } from '../../services/game-hub.service';
import { Client } from '../../services/sanasoppa-api.service';
import { Router } from '@angular/router';
import { GameService } from '../../services/game.service';
import { AuthService } from '@auth0/auth0-angular';
import { toSignal } from '@angular/core/rxjs-interop';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss',
})
export class HomeComponent {
  constructor(
    private gameHub: GameHubService,
    private sanasoppaApi: Client,
    private router: Router,
    private gameService: GameService,
    protected auth: AuthService
  ) {
    effect(() => {
      console.log(this.isAuthenticated());
      if (this.isAuthenticated()) {
        this.gameHub.startConnection();
      }
      console.log(this.user());
    });
  }

  protected isAuthenticated = toSignal(this.auth.isAuthenticated$);

  protected user = toSignal(this.auth.user$);

  protected createGame() {
    this.sanasoppaApi.createGameSession().subscribe(session => {
      this.gameService.isOwner = true;
      this.gameService.joinCode = session.joinCode!;
      this.router.navigate(['/join', { joinCode: session.joinCode }]);
    });
  }

  protected joinGame() {
    this.gameService.isOwner = false;
    this.router.navigate(['/join']);
  }
}
