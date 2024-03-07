import { Component, OnInit, computed, signal } from '@angular/core';
import { GameHubService } from '../../services/game-hub.service';
import { Client } from '../../services/sanasoppa-api.service';
import { GameService } from '../../services/game.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-lobby',
  standalone: true,
  imports: [],
  templateUrl: './lobby.component.html',
  styleUrl: './lobby.component.scss',
})
export class LobbyComponent implements OnInit {
  protected players = signal<string[]>([]);
  protected isOwner = computed(() => this.gameService.isOwner);
  constructor(
    private gameHub: GameHubService,
    private sanasoppaApi: Client,
    protected gameService: GameService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.getPlayers();
    this.gameHub.HubConnection.on('PlayerJoined', () => this.getPlayers());
    this.gameHub.HubConnection.on('GameStarted', () => {
      this.router.navigate(['game']);
    });
  }

  private getPlayers() {
    this.sanasoppaApi
      .getPlayersByGameSessionId(this.gameService.gameId)
      .subscribe(players => {
        this.players.set(players.map(p => p.name));
      });
  }

  protected startGame() {
    this.gameHub.HubConnection.invoke('StartGame', this.gameService.gameId);
  }
}
