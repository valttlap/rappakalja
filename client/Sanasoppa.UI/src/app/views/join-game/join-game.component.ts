import { Component, OnInit, signal } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { GameHubService } from '../../services/game-hub.service';

@Component({
  selector: 'app-join-game',
  templateUrl: './join-game.component.html',
  styleUrl: './join-game.component.scss',
})
export class JoinGameComponent implements OnInit {
  constructor(
    private route: ActivatedRoute,
    private gameHub: GameHubService
  ) {}

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      this.joinCode.set(params.get('joinCode') || '');
      this.hasJoinCode = !!this.joinCode();
    });
  }
  joinCode = signal('');
  hasJoinCode = false;

  protected username = signal('');

  protected changeUsername(event: Event) {
    const username = (event.target as HTMLInputElement).value;
    this.username.set(username);
  }

  protected changeJoinCode(event: Event) {
    const joinCode = (event.target as HTMLInputElement).value;
    this.joinCode.set(joinCode);
  }

  protected startGame() {
    this.gameHub.HubConnection.invoke(
      'JoinGame',
      this.joinCode(),
      this.username()
    );
  }
}
