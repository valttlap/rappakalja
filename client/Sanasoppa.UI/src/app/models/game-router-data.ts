import { SubmissionReturnDto } from './submission-return-dto';

export interface GameRouterData {
  word: string | null;
  submissions: SubmissionReturnDto[];
}
