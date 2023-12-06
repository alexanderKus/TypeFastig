import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { BehaviorSubject, Observable, map } from 'rxjs';
import { TokenService } from './token.service';
import { Score } from '../models/score.model';
import { RandomNicknameGenerator } from 'src/app/features/utils/radomNicknameGenerator';
import { Language } from 'src/app/features/models/language.enum';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  private nickname: string;

  constructor(private http: HttpClient, private tokenService: TokenService) {
    this.nickname = this.tokenService.username;
  }

  public getUsername(): string {
    if (this.nickname.length <= 0) {
      this.nickname = RandomNicknameGenerator.generate();
    }
    return this.nickname;
  }

  getUserBestSpeedScore(userId: number, lang: Language): Observable<Score> {
    return this.http.get<Score>(
      `${environment.apiUrl}/score/getBestSpeedScore/${userId}?lang=${lang}`
    );
  }

  getUserBestAccuracyScore(userId: number, lang: Language): Observable<Score> {
    return this.http.get<Score>(
      `${environment.apiUrl}/score/getBestAccuracyScore/${userId}?lang=${lang}`
    );
  }

  getScoreHistory(userId: number): Observable<Score[]> {
    return this.http.get<Score[]>(
      `${environment.apiUrl}/score/getScoreForUser/${userId}`
    );
  }

  addScore(score: Score): Observable<void> {
    return this.http.post<void>(`${environment.apiUrl}/score/addScore`, score);
  }
}
