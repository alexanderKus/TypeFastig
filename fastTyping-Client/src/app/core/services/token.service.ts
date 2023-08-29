import { Injectable } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';
import jwt_decode from 'jwt-decode';

@Injectable({
  providedIn: 'root',
})
export class TokenService {
  private token: string | undefined;

  constructor(private cookieService: CookieService) {
    if (this.isTokenInCookies()) {
      this.token = this.cookieService.get('auth_token');
    }
  }

  public get isTokenValid(): boolean {
    if (!this.isTokenInCookies()) {
      this.token = undefined;
      return false;
    }
    return true;
  }

  public get userId(): number | undefined {
    if (this.isTokenInCookies()) {
      let decoded: any = jwt_decode(this.token!);
      return decoded.sub;
    }
    return undefined;
  }

  public get username(): string {
    if (this.isTokenInCookies()) {
      let decoded: any = jwt_decode(this.token!);
      return decoded.given_name;
    }
    return '';
  }

  public setToken(token: string): void {
    this.token = token;
    let decoded: any = jwt_decode(this.token!);
    this.cookieService.set('auth_token', token, {
      expires: decoded.exp,
      secure: true,
    });
  }

  public clearToken(): void {
    this.token = undefined;
    this.cookieService.delete('auth_token');
  }

  private isTokenInCookies(): boolean {
    return this.cookieService.check('auth_token') ? true : false;
  }
}
