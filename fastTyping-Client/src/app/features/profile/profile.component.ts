import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Chart } from 'chart.js/auto';
import { NgxSpinnerService } from 'ngx-spinner';
import { Subscription } from 'rxjs';
import { Score } from 'src/app/core/models/score.model';
import { TokenService } from 'src/app/core/services/token.service';
import { UserService } from 'src/app/core/services/user.service';
import { Language } from '../models/language.enum';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss'],
})
export class ProfileComponent implements OnInit, OnDestroy {
  langName = Language;
  lang = Language.C;
  langOpt = [
    { value: Language.C, viewValue: Language[Language.C] },
    { value: Language.PYTHON, viewValue: Language[Language.PYTHON] },
  ];
  username: string = '';
  bestSpeedScore: undefined | Score = {
    UserId: 0,
    Speed: 0,
    Accuracy: 0,
    Language: Language.C,
  };
  bestAccuracyScore: undefined | Score = {
    UserId: 0,
    Speed: 0,
    Accuracy: 0,
    Language: Language.C,
  };
  history: Score[] = [];
  columns: string[] = ['position', 'Accuracy', 'Speed', 'Language'];
  plot: any;
  subscriptions: Subscription[] = [];

  constructor(
    private userService: UserService,
    private tokenService: TokenService,
    private spinner: NgxSpinnerService,
    private router: Router
  ) {
    if (!this.tokenService.isTokenValid) {
      this.router.navigate(['/login']);
    }
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach((sub) => sub.unsubscribe());
  }

  ngOnInit(): void {
    this.spinner.show();
    this.getScores();
  }

  changeScore(): void {
    this.getScores();
  }

  private getScores(): void {
    this.username = this.tokenService.username;
    let userId: number | undefined = this.tokenService.userId;
    if (userId) {
      this.subscriptions.forEach((sub) => sub.unsubscribe());
      this.subscriptions.push(
        this.userService
          .getUserBestSpeedScore(userId, this.lang)
          .subscribe((score) => {
            this.bestSpeedScore = score;
          })
      );
      this.subscriptions.push(
        this.userService
          .getUserBestAccuracyScore(userId, this.lang)
          .subscribe((score) => {
            this.bestAccuracyScore = score;
          })
      );
      this.subscriptions.push(
        this.userService
          .getScoreHistory(userId, this.lang)
          .subscribe((scores) => {
            this.history = scores;
            this.destroyPlot();
            this.fixPosition();
            this.createPlot();
            this.spinner.hide();
          })
      );
    }
  }

  private fixPosition(): void {
    let index = 1;
    this.history.forEach((el) => (el.UserId = index++));
  }

  private createPlot(): void {
    const ctx = document.getElementById('plot') as HTMLCanvasElement;
    if (ctx) {
      this.plot = new Chart(ctx, {
        type: 'line',
        data: {
          labels: this.history.map((el) => el.Accuracy),
          datasets: [
            { data: this.history.map((el) => el.Speed), borderWidth: 1 },
          ],
        },
        options: {
          plugins: {
            legend: {
              display: false,
            },
          },
          scales: {
            y: {
              title: {
                display: true,
                text: 'Speed',
              },
            },
            x: {
              title: {
                display: true,
                text: 'Accuracy',
              },
            },
          },
        },
      });
    }
  }
  private destroyPlot(): void {
    if (this.plot) this.plot.destroy();
  }
}
