import { Language } from './language.enum';

export interface ScoreInfoDto {
  username: string;
  accuracy: number;
  speed: number;
  language: Language;
}
