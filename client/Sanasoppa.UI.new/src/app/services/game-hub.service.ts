import { Injectable } from '@angular/core';
import { AuthService } from '@auth0/auth0-angular';
import {
  HttpTransportType,
  HubConnection,
  HubConnectionBuilder,
} from '@microsoft/signalr';
import { lastValueFrom } from 'rxjs';
import { GameStatusDto } from '../models/game-status';
import { GameService } from './game.service';
import { GameRouterData } from '../models/game-router-data';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class GameHubService {
  private _hubConnection?: HubConnection;
  private hubUrl = `${window.location.origin}/hubs/gamehub`;
  private readonly gameRoute = '/game';

  constructor(
    private auth: AuthService,
    private gameService: GameService,
    private router: Router
  ) {}

  public async startConnection(): Promise<void> {
    try {
      await this.initializeConnection();
      this.setupOnCloseHandler();
    } catch (err) {
      console.error('Error starting the SignalR connection:', err);
    }
  }

  private async initializeConnection(): Promise<void> {
    const token = await lastValueFrom(this.auth.getAccessTokenSilently());
    this._hubConnection = new HubConnectionBuilder()
      .withUrl(this.hubUrl, {
        accessTokenFactory: () => token,
        transport: HttpTransportType.WebSockets,
      })
      .withAutomaticReconnect()
      .build();
    await this._hubConnection.start();
  }

  private setupOnCloseHandler(): void {
    this._hubConnection?.onclose(async () => {
      await this.initializeConnection();
      await this.onReconnected();
    });
  }

  private async onReconnected(): Promise<void> {
    const status: GameStatusDto =
      await this.HubConnection.invoke('GetGameStatus');
    this.gameService.updateGameStatus(status);
    const routerData: GameRouterData = {
      word: status.word,
      submissions: status.submissions,
    };
    this.router.navigate([this.gameRoute], { state: { data: routerData } });
  }

  get HubConnection(): HubConnection {
    if (!this._hubConnection) {
      throw new Error('HubConnection is not initialized');
    }
    return this._hubConnection;
  }
}
