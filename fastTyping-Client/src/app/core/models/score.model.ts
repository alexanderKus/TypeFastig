import { Language } from 'src/app/features/models/language.enum';

export interface Score {
  UserId: number;
  Speed: number;
  Accuracy: number;
  Language: Language;
}
