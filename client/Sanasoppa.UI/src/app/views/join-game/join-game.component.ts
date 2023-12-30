import { Component, computed, signal } from '@angular/core';
import { Router } from '@angular/router';
import { GameHubService } from '../../services/game-hub.service';
import { GameService } from '../../services/game.service';

@Component({
  selector: 'app-join-game',
  templateUrl: './join-game.component.html',
  styleUrl: './join-game.component.scss',
})
export class JoinGameComponent {
  constructor(
    private router: Router,
    private gameHub: GameHubService,
    private gameService: GameService
  ) {}

  protected joinCodeSgn = signal('');

  protected joinCodeValid = computed(() => this.joinCodeSgn().length === 4);

  protected changeJoinCode(event: Event) {
    const joinCode = (event.target as HTMLInputElement).value;
    this.joinCodeSgn.set(joinCode);
  }

  protected startGame() {
    this.gameHub.HubConnection.invoke('JoinGame', this.joinCodeSgn()).then(
      (gameId: string) => {
        this.gameService.gameId = gameId;
        this.gameService.joinCode = this.joinCodeSgn();
        this.router.navigate(['/lobby']);
      }
    );
  }
}
