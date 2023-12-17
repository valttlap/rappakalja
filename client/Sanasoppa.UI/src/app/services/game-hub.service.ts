import { Injectable } from '@angular/core';
import {
  HttpTransportType,
  HubConnection,
  HubConnectionBuilder,
} from '@microsoft/signalr';

@Injectable({
  providedIn: 'root',
})
export class GameHubService {
  private _hubConnection?: HubConnection;
  private hubUrl = 'https://localhost:5001/gamehub';

  constructor() {}

  public async startConnection() {
    try {
      this._hubConnection = new HubConnectionBuilder()
        .withUrl(this.hubUrl, { transport: HttpTransportType.WebSockets })
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
