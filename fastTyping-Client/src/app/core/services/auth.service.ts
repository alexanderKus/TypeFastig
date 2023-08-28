import { Injectable } from '@angular/core';
import { LoginModel } from '../models/login.model';
import { BehaviorSubject, Observable, map } from 'rxjs';
import { AuthenticatedResponse } from '../models/authenticatedResponse.model';
import { environment } from 'src/environments/environment';
import { RegisterModel } from '../models/register.model';
import { TokenService } from './token.service';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  public isLoggedIn = new BehaviorSubject<boolean>(false);

  constructor(private http: HttpClient, private tokenService: TokenService) {
    if (this.tokenService.isTokenValid) {
      this.isLoggedIn.next(true);
    }
  }

  login(loginModel: LoginModel): Observable<boolean> {
    return this.http
      .post<AuthenticatedResponse>(
        `${environment.apiUrl}/auth/login`,
        loginModel
      )
      .pipe(
        map((res) => {
          if (!!res) {
            this.tokenService.setToken(res.Token);
            this.isLoggedIn.next(true);
            return true;
          }
          return false;
        })
      );
  }

  register(registerModel: RegisterModel): Observable<boolean> {
    return this.http
      .post<AuthenticatedResponse>(
        `${environment.apiUrl}/auth/register`,
        registerModel
      )
      .pipe(
        map((res) => {
          if (!!res) {
            this.tokenService.setToken(res.Token);
            this.isLoggedIn.next(true);
            return true;
          }
          return false;
        })
      );
  }

  logout(): void {
    this.isLoggedIn.next(false);
    this.tokenService.clearToken();
  }
}
