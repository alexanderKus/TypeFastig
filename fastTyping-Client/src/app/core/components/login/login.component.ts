import { Component, ElementRef, OnDestroy, ViewChild } from '@angular/core';
import { UserService } from '../../services/user.service';
import { LoginModel } from '../../models/login.model';
import { RegisterModel } from '../../models/register.model';
import { Subscription, catchError } from 'rxjs';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent implements OnDestroy {
  isLogging: boolean = true;
  loginModel: LoginModel = {
    username: '',
    password: '',
  };
  registerModel: RegisterModel = {
    email: '',
    username: '',
    password: '',
  };
  @ViewChild('dialog') dialog!: ElementRef<HTMLDialogElement>;
  dialogMessage: string = '';
  subscriptions: Subscription[] = [];

  constructor(private authService: AuthService, private router: Router) {}

  ngOnDestroy(): void {
    this.subscriptions.forEach((sub) => sub.unsubscribe());
  }

  swapMode() {
    this.isLogging = !this.isLogging;
  }

  login() {
    if (this.isLoginModelValid()) {
      this.subscriptions.push(
        this.authService
          .login(this.loginModel)
          .pipe(
            catchError((err) => {
              this.dialogMessage = 'Something went wrong';
              this.dialog.nativeElement.showModal();
              this.clearModels();
              return err;
            })
          )
          .subscribe((res) => {
            if (!!res) {
              this.router.navigate(['/']);
            }
          })
      );
    }
  }

  register() {
    if (this.isRegisterModelValid()) {
      // TODO: validate password
      this.subscriptions.push(
        this.authService
          .register(this.registerModel)
          .pipe(
            catchError((err) => {
              this.dialogMessage = 'Something went wrong';
              this.dialog.nativeElement.showModal();
              this.clearModels();
              return err;
            })
          )
          .subscribe((res) => {
            if (!!res) {
              this.router.navigate(['/']);
            }
          })
      );
      return;
    }
    this.dialogMessage = 'Minimum length of username and password is 8';
    this.dialog.nativeElement.showModal();
  }

  isLoginModelValid(): boolean {
    return (
      this.loginModel.username.length > 0 && this.loginModel.password.length > 0
    );
  }

  isRegisterModelValid(): boolean {
    return (
      this.registerModel.email.length > 0 &&
      this.registerModel.username.length > 8 &&
      this.registerModel.password.length > 8
    );
  }

  private clearModels() {
    this.loginModel.username = '';
    this.loginModel.password = '';
    this.registerModel.email = '';
    this.registerModel.username = '';
    this.registerModel.password = '';
  }
}
