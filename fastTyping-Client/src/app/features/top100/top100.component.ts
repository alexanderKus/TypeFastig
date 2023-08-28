import { Component, OnDestroy, OnInit } from '@angular/core';
import { ScoresService } from '../services/scores.service';
import { Top100Scores } from '../models/top100Scores.model';
import { NgxSpinnerService } from 'ngx-spinner';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-top100',
  templateUrl: './top100.component.html',
  styleUrls: ['./top100.component.scss'],
})
export class Top100Component implements OnInit, OnDestroy {
  data: Top100Scores = {
    bestAccuracy: [],
    bestSpeed: [],
  };
  columns1: string[] = ['Username', 'Accuracy', 'Speed'];
  columns2: string[] = ['Username', 'Speed', 'Accuracy'];
  subscription: Subscription | undefined;

  constructor(
    private scoreService: ScoresService,
    private spinner: NgxSpinnerService
  ) {}

  ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  ngOnInit(): void {
    this.spinner.show();
    this.subscription = this.scoreService.getTop100().subscribe((res) => {
      this.data.bestAccuracy = res.bestAccuracy;
      this.data.bestSpeed = res.bestSpeed;
      this.spinner.hide();
    });
  }
}
