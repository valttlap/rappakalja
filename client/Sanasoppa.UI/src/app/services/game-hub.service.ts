import { Injectable } from '@angular/core';
import { AuthService } from '@auth0/auth0-angular';
import {
  HttpTransportType,
  HubConnection,
  HubConnectionBuilder,
} from '@microsoft/signalr';
import { lastValueFrom } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class GameHubService {
  private _hubConnection?: HubConnection;
  private hubUrl = `${window.location.origin}/hubs/gamehub`;

  constructor(private auth: AuthService) {}

  public async startConnection() {
    try {
      const token = await lastValueFrom(this.auth.getAccessTokenSilently());
      this._hubConnection = new HubConnectionBuilder()
        .withUrl(this.hubUrl, {
          accessTokenFactory: () => token,
          transport: HttpTransportType.WebSockets,
        })
        .withAutomaticReconnect()
        .build();
      await this._hubConnection.start();
    } catch (err) {
      console.log(err);
    }
  }

  get HubConnection(): HubConnection {
    if (!this._hubConnection) {
      throw new Error('HubConnection is not initialized');
    }
    return this._hubConnection;
  }
}
