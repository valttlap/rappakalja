import { Component, Input } from '@angular/core';
import { NgbActiveModal, NgbModule } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-confirm-delete-modal',
  standalone: true,
  imports: [NgbModule],
  templateUrl: './confirm-delete-modal.component.html',
  styleUrl: './confirm-delete-modal.component.scss',
})
export class ConfirmDeleteModalComponent {
  @Input({ required: true }) playerName!: string;
  constructor(private activeModal: NgbActiveModal) {}

  dismiss() {
    this.activeModal.dismiss();
  }

  close() {
    this.activeModal.close();
  }
}
