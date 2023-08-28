import { Component, OnInit } from '@angular/core';
import { ScoresService } from '../services/scores.service';
import { Top100Scores } from '../models/top100Scores.model';

@Component({
  selector: 'app-top100',
  templateUrl: './top100.component.html',
  styleUrls: ['./top100.component.scss'],
})
export class Top100Component implements OnInit {
  data: Top100Scores = {
    bestAccuracy: [],
    bestSpeed: [],
  };
  columns1: string[] = ['Username', 'Accuracy', 'Speed'];
  columns2: string[] = ['Username', 'Speed', 'Accuracy'];

  constructor(private scoreService: ScoresService) {}

  ngOnInit(): void {
    this.scoreService.getTop100().subscribe((res) => {
      this.data.bestAccuracy = res.bestAccuracy;
      this.data.bestSpeed = res.bestSpeed;
    });
  }
}
