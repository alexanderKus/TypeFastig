import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Top100Scores } from '../models/top100Scores.model';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class ScoresService {
  constructor(private http: HttpClient) {}

  getTop100(): Observable<Top100Scores> {
    return this.http.get<Top100Scores>(
      `${environment.apiUrl}/score/getTop100Scores`
    );
  }
}
