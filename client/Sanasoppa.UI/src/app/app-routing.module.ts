import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { JoinGameComponent } from './views/join-game/join-game.component';
import { HomeComponent } from './views/home/home.component';

const routes: Routes = [
  { path: 'join/:joinCode', component: JoinGameComponent },
  { path: 'join', component: JoinGameComponent },
  { path: '', component: HomeComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
