import { inject } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';

export function tokenGetter() {
  const cookie = inject(CookieService);
  return cookie.get('auth_token');
}
