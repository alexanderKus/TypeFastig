import { Component, OnDestroy } from '@angular/core';
import { Observable } from 'rxjs';
import { RoomService } from '../services/room.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { PlayerStatus } from '../models/playerStatus.model';

@Component({
  selector: 'app-room',
  templateUrl: './room.component.html',
  styleUrls: ['./room.component.scss'],
})
export class RoomComponent implements OnDestroy {
  isInRoom$: Observable<boolean> = this.roomService.isStarted$;
  stats$: Observable<PlayerStatus[]> = this.roomService.stats$;
  private interval: any;

  constructor(
    private roomService: RoomService,
    private spinner: NgxSpinnerService
  ) {}

  async ngOnDestroy(): Promise<void> {
    if (this.interval) {
      clearInterval(this.interval);
    }
    if (this.isInRoom$) {
      await this.leaveRoom();
    }
  }

  async endGame() {
    await this.leaveRoom();
  }

  async joinRoom() {
    this.spinner.show();
    await this.roomService.joinRoom();
    this.interval = setInterval(async () => {
      const stats: PlayerStatus = {
        name: '',
        progress: 10,
      };
      await this.roomService.sendStats(stats);
    }, 1000);
    this.spinner.hide();
  }

  async leaveRoom() {
    if (this.interval) {
      clearInterval(this.interval);
    }
    await this.roomService.leaveRoom();
  }
}
