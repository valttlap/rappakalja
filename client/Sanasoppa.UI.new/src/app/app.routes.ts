import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    loadComponent: () =>
      import('./views/home/home.component').then(mod => mod.HomeComponent),
  },
  {
    path: 'lobby',
    pathMatch: 'full',
    loadComponent: () =>
      import('./views/lobby/lobby.component').then(mod => mod.LobbyComponent),
  },
  {
    path: 'join',
    pathMatch: 'full',
    loadComponent: () =>
      import('./views/join-game/join-game.component').then(
        mod => mod.JoinGameComponent
      ),
  },
  {
    path: 'game',
    pathMatch: 'full',
    loadComponent: () =>
      import('./views/game/game.component').then(mod => mod.GameComponent),
  },
];
