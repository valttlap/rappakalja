import { Component, OnInit, signal } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { GameHubService } from '../../services/game-hub.service';
import { GameService } from '../../services/game.service';

@Component({
  selector: 'app-join-game',
  templateUrl: './join-game.component.html',
  styleUrl: './join-game.component.scss',
})
export class JoinGameComponent implements OnInit {
  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private gameHub: GameHubService,
    private gameService: GameService
  ) {}

  ngOnInit(): void {
    if (this.route.snapshot.params['joinCode']) {
      const joinCode = this.route.snapshot.params['joinCode'];
      this.joinCodeSgn.set(joinCode);
      this.hasJoinCode.set(true);
    }
  }
  protected joinCodeSgn = signal('');
  protected hasJoinCode = signal(false);

  protected username = signal('');

  protected changeUsername(event: Event) {
    const username = (event.target as HTMLInputElement).value;
    this.username.set(username);
  }

  protected changeJoinCode(event: Event) {
    const joinCode = (event.target as HTMLInputElement).value;
    this.joinCodeSgn.set(joinCode);
  }

  protected startGame() {
    this.gameHub.HubConnection.invoke(
      'JoinGame',
      this.joinCodeSgn(),
      this.username()
    ).then((gameId: string) => {
      this.gameService.gameId = gameId;
      this.gameService.joinCode = this.joinCodeSgn();
      this.router.navigate(['/lobby']);
    });
  }
}
