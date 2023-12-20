import { Component, OnInit } from '@angular/core';
import { GameHubService } from '../../services/game-hub.service';
import { Client } from '../../services/sanasoppa-api.service';
import { Router } from '@angular/router';
import { GameService } from '../../services/game.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss',
})
export class HomeComponent implements OnInit {
  constructor(
    private gameHub: GameHubService,
    private sanasoppaApi: Client,
    private router: Router,
    private gameService: GameService
  ) {}

  ngOnInit(): void {
    this.gameHub.startConnection();
  }

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
