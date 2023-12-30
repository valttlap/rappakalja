import { SubmissionReturnDto } from './submission-return-dto';

export enum GameState {
  Wait,
  SubmitWord,
  SubmitGuess,
  ReadGuesses,
}

export interface GameStatusDto {
  state: GameState;
  isOwner: boolean;
  isDasher: boolean;
  gameId: string;
  joinCode: string;
  word: string | null;
  submissions: SubmissionReturnDto[];
}
