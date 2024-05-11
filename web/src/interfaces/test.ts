import { IUser } from "./auth";
import { Language } from "./lang";

export interface Test {
  title: string;
  description: string;
  imageUrl: string;
  cardsAmount: number;
  language: Language;
  createdBy: IUser;
  cards: Card[];
}

export interface Card {
  question: string;
  questionType: string;
}