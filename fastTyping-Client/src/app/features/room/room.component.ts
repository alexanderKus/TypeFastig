import { Component, ElementRef, OnDestroy, ViewChild } from '@angular/core';
import { Observable } from 'rxjs';
import { RoomService } from '../services/room.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { PlayerStatus } from '../models/playerStatus.model';
import { UserService } from 'src/app/core/services/user.service';
import { Language } from '../models/language.enum';

@Component({
  selector: 'app-room',
  templateUrl: './room.component.html',
  styleUrls: ['./room.component.scss'],
})
export class RoomComponent implements OnDestroy {
  isInRoom$: Observable<boolean> = this.roomService.isStarted$;
  stats$: Observable<PlayerStatus[]> = this.roomService.stats$;
  @ViewChild('waitingDialog') waitingDialog!: ElementRef<HTMLDialogElement>;

  constructor(
    private roomService: RoomService,
    private userService: UserService,
    private spinner: NgxSpinnerService
  ) {}

  getLanguage(): Language | undefined {
    return this.roomService.roomLanguage$.getValue();
  }

  async ngOnDestroy(): Promise<void> {
    if (this.roomService.roomId != undefined) {
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
    this.waitingDialog.nativeElement.showModal();
    while (!this.roomService.isStarted$.getValue()) {}
    this.waitingDialog.nativeElement.close();
  }

  async leaveRoom(): Promise<void> {
    await this.roomService.leaveRoom();
  }

  async updateStats(stats: PlayerStatus): Promise<void> {
    await this.roomService.sendStats(stats);
  }
}
