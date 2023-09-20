import {
  AfterViewInit,
  Component,
  ElementRef,
  EventEmitter,
  HostListener,
  OnDestroy,
  Output,
  ViewChild,
} from '@angular/core';
import { Score } from 'src/app/core/models/score.model';
import { TokenService } from 'src/app/core/services/token.service';
import { UserService } from 'src/app/core/services/user.service';
import { TextEditor } from '../textEditor';
import { PlayerStatus } from 'src/app/features/models/playerStatus.model';

@Component({
  selector: 'app-typing-board',
  templateUrl: './typing-board.component.html',
  styleUrls: ['./typing-board.component.scss'],
})
export class TypingBoardComponent implements AfterViewInit, OnDestroy {
  baseTextEditor: TextEditor = TextEditor.createNewRandomText();
  writtenTextEditor = new TextEditor('');
  writtenText = '';

  isGameStarted: boolean = false;
  isEndDialogShown: boolean = false;

  currentWordIndex: number = 0;
  interval: any;
  startedAt: number = 0;

  errorCount: number = 0;
  speed: number = 0;
  bestSpeed: number = 0;
  accuracy: number = 0;

  @Output('stats') stats = new EventEmitter<PlayerStatus>();

  @ViewChild('startDialog') startDialog!: ElementRef<HTMLDialogElement>;
  @ViewChild('endDialog') endDialog!: ElementRef<HTMLDialogElement>;

  @HostListener('document:keypress', ['$event'])
  handleKeyboardEvent(event: KeyboardEvent) {
    if (!this.isGameStarted) {
      this.init();
    }
    if (this.currentWordIndex > this.baseTextEditor.text.length) {
      return;
    } else if (event.key === 'Enter') {
      return;
    }
    this.writtenText += event.key;
    this.currentWordIndex++;
    if (this.baseTextEditor.getCharAtIndex(this.currentWordIndex) === '\n') {
      this.writtenText += '\n';
      this.currentWordIndex++;
    }
    this.writtenTextEditor = new TextEditor(this.writtenText);
    this.accuracy = Math.abs(
      ((this.writtenText.length - this.errorCount) / this.writtenText.length) *
        100
    );
    if (this.currentWordIndex == this.baseTextEditor.text.length) {
      this.isGameStarted = false;
      this.isEndDialogShown = true;
      this.endDialog.nativeElement.showModal();
      if (this.tokenService.isTokenValid) {
        const score: Score = {
          UserId: this.tokenService.userId!,
          Speed: this.bestSpeed,
          Accuracy: this.accuracy,
        };
        this.userService.addScore(score).subscribe();
      }
    }
  }

  @HostListener('document:keydown.backspace', ['$event'])
  handleBackSpaceEvent(event: KeyboardEvent) {
    if (this.writtenText.length > 0) {
      if (
        this.writtenTextEditor.getCharAtIndex(this.currentWordIndex - 1) ===
        '\n'
      ) {
        this.currentWordIndex -= 2;
        this.writtenText = this.writtenText.substring(
          0,
          this.writtenText.length - 2
        );
      } else {
        this.writtenText = this.writtenText.substring(
          0,
          this.writtenText.length - 1
        );
        this.currentWordIndex--;
      }
      this.writtenTextEditor = new TextEditor(this.writtenText);
    }
  }

  constructor(
    private userService: UserService,
    private tokenService: TokenService
  ) {}

  ngAfterViewInit(): void {
    this.openStartDialog();
  }

  ngOnDestroy(): void {
    if (this.interval) {
      clearInterval(this.interval);
    }
  }

  closeStartDialog(): void {
    this.startDialog.nativeElement.close();
  }

  closeEndDialog(): void {
    this.endDialog.nativeElement.close();
    this.reset();
    this.isEndDialogShown = false;
  }

  getStyleClassForCurrentLetter(lineIndex: number, wordIndex: number): string {
    let index = this.calculateCurrentIndex(lineIndex, wordIndex);
    if (index === this.currentWordIndex) {
      return 'current-letter';
    }
    return '';
  }

  getStyleClass(letter: string, lineIndex: number, wordIndex: number): string {
    let index = this.calculateCurrentIndex(lineIndex, wordIndex);
    const xEChar = this.baseTextEditor.getCharAtIndex(index);
    const yEChar = this.writtenTextEditor.getCharAtIndex(index);
    if (xEChar === yEChar) {
      return 'correct-letter';
    } else {
      if (letter === ' ' || xEChar === ' ') {
        return 'space';
      }
      return 'wrong-letter';
    }
  }

  private init(): void {
    this.isGameStarted = true;
    this.startedAt = new Date().getTime();
    this.interval = setInterval(() => {
      this.calculateSpeed();
      this.emitStats();
    }, 1000);
  }

  private openStartDialog(): void {
    this.startDialog.nativeElement.showModal();
  }

  private reset(): void {
    if (this.interval) {
      clearInterval(this.interval);
    }
    this.writtenText = '';
    this.writtenTextEditor = new TextEditor('');
    this.baseTextEditor = TextEditor.createNewRandomText();
    this.errorCount = 0;
    this.currentWordIndex = 0;
    this.bestSpeed = 0;
    this.speed = 0;
  }

  private calculateSpeed(): void {
    if (this.isGameStarted) {
      const now = new Date().getTime();
      const numerator = Math.abs(this.currentWordIndex / 5 - this.errorCount);
      const denominator = (now - this.startedAt) / 1000 / 60;
      let currentSpeed = Math.ceil(numerator / denominator);
      this.bestSpeed = Math.max(this.bestSpeed, currentSpeed);
      this.speed = currentSpeed;
    }
  }

  private emitStats(): void {
    const progress = Math.round(
      (this.writtenTextEditor.text.length / this.baseTextEditor.text.length) *
        100
    );
    const stats: PlayerStatus = {
      name: this.userService.getUsername(),
      progress: progress,
    };
    this.stats.emit(stats);
  }

  private calculateCurrentIndex(lineIndex: number, wordIndex: number): number {
    let index = 0;
    for (let i = 0; i < lineIndex; i++) {
      index += this.baseTextEditor.lines[i].length + 1; // +1 because of '\n'
    }
    index += wordIndex;
    return index;
  }
}
