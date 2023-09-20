import { Component, OnDestroy } from '@angular/core';
import { Observable } from 'rxjs';
import { RoomService } from '../services/room.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { PlayerStatus } from '../models/playerStatus.model';
import { TokenService } from 'src/app/core/services/token.service';
import { RandomNicknameGenerator } from '../utils/radomNicknameGenerator';
import { UserService } from 'src/app/core/services/user.service';

@Component({
  selector: 'app-room',
  templateUrl: './room.component.html',
  styleUrls: ['./room.component.scss'],
})
export class RoomComponent implements OnDestroy {
  isInRoom$: Observable<boolean> = this.roomService.isStarted$;
  stats$: Observable<PlayerStatus[]> = this.roomService.stats$;

  constructor(
    private roomService: RoomService,
    private userService: UserService,
    private spinner: NgxSpinnerService
  ) {}

  async ngOnDestroy(): Promise<void> {
    if (this.isInRoom$) {
      await this.leaveRoom();
    }
  }

  async endGame(): Promise<void> {
    await this.leaveRoom();
  }

  async joinRoom(): Promise<void> {
    this.spinner.show();
    await this.roomService.joinRoom();
    const stats: PlayerStatus = {
      name: this.userService.getUsername(),
      progress: 0,
    };
    await this.roomService.sendStats(stats);
    this.spinner.hide();
  }

  async leaveRoom(): Promise<void> {
    await this.roomService.leaveRoom();
  }

  async updateStats(stats: PlayerStatus): Promise<void> {
    await this.roomService.sendStats(stats);
  }
}
