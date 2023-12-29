import { Component } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { AuthService } from '@auth0/auth0-angular';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
})
export class AppComponent {
  title = 'Sanasoppa.UI';

  constructor(private auth: AuthService) {}

  protected authLoading = toSignal(this.auth.isLoading$);
}
