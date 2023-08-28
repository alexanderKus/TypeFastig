import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { BehaviorSubject, Observable, map } from 'rxjs';
import { TokenService } from './token.service';
import { Score } from '../models/score.model';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  constructor(private http: HttpClient, private tokenService: TokenService) {}

  getUserBestSpeedScore(userId: number): Observable<Score> {
    return this.http.get<Score>(
      `${environment.apiUrl}/score/getBestSpeedScore/${userId}`
    );
  }

  getUserBestAccuracyScore(userId: number): Observable<Score> {
    return this.http.get<Score>(
      `${environment.apiUrl}/score/getBestAccuracyScore/${userId}`
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
