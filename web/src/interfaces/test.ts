import { IUser } from "./auth";
import { Language } from "./lang";

export interface Test {
  uuid: string;
  title: string;
  description: string;
  imageUrl: string;
  cardsCount: number;
  language: Language;
  createdBy: IUser;
}

export interface Option {
  optionText: string;
  isCorrect: boolean;
  uuid?: string;
}

export interface ICard {
  question: string;
  testUuid: string;
  questionType: string;
  answerOptions: Option[];
  uuid?: string;
}

export interface createTestRequest {
  title: string,
  description?: string,
  imageUrl?: string,
  languageUuid: string
}

export interface BeginTestResponse {
  card: ICard;
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
