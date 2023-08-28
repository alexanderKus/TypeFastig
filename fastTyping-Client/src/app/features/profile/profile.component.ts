import { Component, OnInit } from '@angular/core';
import { Chart } from 'chart.js/auto';
import { Score } from 'src/app/core/models/score.model';
import { TokenService } from 'src/app/core/services/token.service';
import { UserService } from 'src/app/core/services/user.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss'],
})
export class ProfileComponent implements OnInit {
  username: string = '';
  bestSpeedScore: Score = {
    Id: 0,
    Speed: 0,
    Accuracy: 0,
  };
  bestAccuracyScore: Score = {
    Id: 0,
    Speed: 0,
    Accuracy: 0,
  };
  history: Score[] = [];
  columns: string[] = ['position', 'Accuracy', 'Speed'];
  plot: any;

  constructor(
    private userService: UserService,
    private tokenService: TokenService
  ) {}

  ngOnInit(): void {
    this.username = this.tokenService.username;
    let userId: number | undefined = this.tokenService.userId;
    if (userId) {
      this.userService.getUserBestSpeedScore(userId).subscribe((score) => {
        this.bestSpeedScore = score;
      });
      this.userService.getUserBestAccuracyScore(userId).subscribe((score) => {
        this.bestAccuracyScore = score;
      });
      this.userService.getScoreHistory(userId).subscribe((scores) => {
        this.history = scores;
        this.fixPosition();
        this.createPlot();
      });
    }
  }

  private fixPosition(): void {
    let index = 1;
    this.history.forEach((el) => (el.Id = index++));
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
}
