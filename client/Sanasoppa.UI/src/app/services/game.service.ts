import { Injectable } from '@angular/core';

export type GameStatus =
  | 'wait'
  | 'give word'
  | 'give submission'
  | 'read submissions';

@Injectable({
  providedIn: 'root',
})
export class GameService {
  _gameId?: string;
  _joinCode?: string;
  _isOwner: boolean = false;
  _isDasher: boolean = false;
  _status: GameStatus = 'wait';

  constructor() {}

  get gameId(): string {
    if (!this._gameId) {
      throw new Error('GameId is not initialized');
    }
    return this._gameId;
  }

  set gameId(gameId: string) {
    this._gameId = gameId;
  }

  get joinCode(): string {
    if (!this._joinCode) {
      throw new Error('JoinCode is not initialized');
    }
    return this._joinCode;
  }

  set joinCode(joinCode: string) {
    this._joinCode = joinCode;
  }

  get isOwner(): boolean {
    return this._isOwner;
  }

  set isOwner(isOwner: boolean) {
    this._isOwner = isOwner;
    this.isDasher = isOwner;
  }

  get isDasher(): boolean {
    return this._isDasher;
  }

  set isDasher(isDasher: boolean) {
    this._isDasher = isDasher;
    isDasher ? (this.status = 'give word') : (this.status = 'wait');
  }

  get status(): GameStatus {
    return this._status;
  }

  set status(status: GameStatus) {
    this._status = status;
  }
}
