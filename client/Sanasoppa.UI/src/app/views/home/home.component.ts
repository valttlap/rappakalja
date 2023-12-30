import { Component, effect } from '@angular/core';
import { GameHubService } from '../../services/game-hub.service';
import { Client } from '../../services/sanasoppa-api.service';
import { Router } from '@angular/router';
import { GameService } from '../../services/game.service';
import { AuthService } from '@auth0/auth0-angular';
import { toSignal } from '@angular/core/rxjs-interop';
import { GameStatusDto } from '../../models/game-status';
import { GameRouterData } from '../../models/game-router-data';

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
      if (this.user()) {
        try {
          this.gameHub.HubConnection;
        } catch {
          this.gameHub.startConnection();
        }
      }
    });
  }

  protected user = toSignal(this.auth.user$);

  protected createGame() {
    this.sanasoppaApi.createGameSession().subscribe(session => {
      this.gameService.isOwner = true;
      this.gameService.joinCode = session.joinCode!;
      this.gameHub.HubConnection.invoke('JoinGame', session.joinCode).then(
        (gameId: string) => {
          this.gameService.gameId = gameId;
          this.router.navigate(['/lobby']);
        }
      );
    });
  }

  protected joinGame() {
    this.gameService.isOwner = false;
    this.router.navigate(['/join']);
  }

  protected async getStatus() {
    const status: GameStatusDto =
      await this.gameHub.HubConnection.invoke('GetGameStatus');
    this.gameService.joinCode = status.joinCode;
    this.gameService.gameId = status.gameId;
    this.gameService.isDasher = status.isDasher;
    this.gameService.isOwner = status.isOwner;
    this.gameService.status = GameService.gameStateEnumToStatus(status.state);
    const routerData: GameRouterData = {
      word: status.word,
      submissions: status.submissions,
    };
    this.router.navigate(['/game'], { state: { data: routerData } });
  }
}
