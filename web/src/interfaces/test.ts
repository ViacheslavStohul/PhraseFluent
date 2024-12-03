import { IUser } from "./auth";

export interface Test {
  uuid: string;
  title: string;
  description: string;
  imageUrl: string;
  cardsCount: number;
  createdBy: IUser;
  cards: ICard[];
  completedAttempts?: number;
}

export interface Option {
  optionText: string;
  isCorrect?: boolean;
  uuid?: string;
  isAllowedText?: boolean;
  selectionCount?: number;
  selectionPercentage?: number;
}

export interface ICard {
  question: string;
  testUuid: string;
  questionType: string;
  answerOptions?: Option[];
  uuid?: string;
  textAnswers?: Option[];
}

export interface createTestRequest {
  title: string,
  description?: string,
  imageUrl?: string,
  languageUuid: string
}

export interface BeginTestResponse {
  card?: ICard;
  currentQuestion: number;
  questions: number;
  testAttemptUuid: string;
}

export interface NextTestRequest {
  cardUuid: string;
  testAttemptUuid: string;
  answerString?: string;
  pickedOptions?: string[];
}
