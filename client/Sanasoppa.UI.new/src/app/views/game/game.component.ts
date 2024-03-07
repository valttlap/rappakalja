import { Component, OnInit, signal } from '@angular/core';
import { Router } from '@angular/router';
import { NgbModal, NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { GameRouterData } from '../../models/game-router-data';
import { SubmissionReturnDto } from '../../models/submission-return-dto';
import { GameHubService } from '../../services/game-hub.service';
import { GameService } from '../../services/game.service';
import { ConfirmDeleteModalComponent } from '../../modals/confirm-delete-modal/confirm-delete-modal.component';

@Component({
  selector: 'app-game',
  standalone: true,
  imports: [NgbModule, ConfirmDeleteModalComponent],
  templateUrl: './game.component.html',
  styleUrl: './game.component.scss',
})
export class GameComponent implements OnInit {
  constructor(
    private gameHub: GameHubService,
    public game: GameService,
    private modal: NgbModal,
    private router: Router
  ) {
    const currentNavigation = this.router.getCurrentNavigation();
    const state = currentNavigation?.extras.state as { data: GameRouterData };
    if (state && state.data) {
      const data: GameRouterData = state.data;
      this.roundWord.set(data.word ? data.word : '');
      this.submissions.set(data.submissions);
    }
  }

  ngOnInit(): void {
    this.gameHub.HubConnection.on('WordSubmitted', (word: string) => {
      this.roundSubmission.set('');
      this.roundWord.set(word);
      this.game.status = 'give submission';
    });
    this.gameHub.HubConnection.on(
      'ReadSubmissions',
      (submissions: SubmissionReturnDto[]) => {
        console.log(submissions);
        this.game.status = 'read submissions';
        this.submissions.set(submissions);
      }
    );
    this.gameHub.HubConnection.on('NewLeader', () => {
      this.game.isDasher = true;
    });
    this.gameHub.HubConnection.on('RoundEnded', () => {
      this.submissions.set([]);
      this.roundWord.set('');
      this.roundSubmission.set('');
    });
  }

  protected roundWord = signal<string>('');
  protected roundSubmission = signal<string>('');
  protected submissions = signal<SubmissionReturnDto[]>([]);

  updateWord(event: Event) {
    this.roundWord.set((event.target as HTMLInputElement).value);
  }

  updateSubmission(event: Event) {
    this.roundSubmission.set((event.target as HTMLInputElement).value);
  }

  submitWord() {
    this.gameHub.HubConnection.invoke(
      'SubmitWord',
      this.game.gameId,
      this.roundWord()
    );
  }

  submitSubmission() {
    this.gameHub.HubConnection.invoke(
      'SubmitSubmission',
      this.game.gameId,
      this.roundSubmission()
    ).then(() => {
      if (this.game.isDasher) {
        this.game.status = 'read submissions';
      } else {
        this.game.status = 'wait';
      }
    });
  }

  endRound() {
    this.gameHub.HubConnection.invoke('EndRound', this.game.gameId).then(() => {
      this.game.status = 'wait';
      this.game.isDasher = false;
    });
  }

  restartRound() {
    this.gameHub.HubConnection.invoke('RestartRound', this.game.gameId);
    this.roundWord.set('');
    this.roundSubmission.set('');
    this.submissions.set([]);
    this.game.status = 'give word';
  }

  deleteSubmission(playerName: string) {
    const confirmDeleteModalRef = this.modal.open(ConfirmDeleteModalComponent, {
      ariaLabelledBy: 'modal-basic-title',
    });

    confirmDeleteModalRef.componentInstance.playerName = playerName;
    confirmDeleteModalRef.result.then(() => {
      this.submissions.update(submissions =>
        submissions.filter(submission => submission.playerName !== playerName)
      );
    });
  }
}
