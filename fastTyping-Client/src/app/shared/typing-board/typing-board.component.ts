import {
  Component,
  ElementRef,
  HostListener,
  OnDestroy,
  ViewChild,
} from '@angular/core';
import { Score } from 'src/app/core/models/score.model';
import { TokenService } from 'src/app/core/services/token.service';
import { Model } from 'src/app/features/models/model';
import { ModelFactory } from '../model-factory/model-factory';
import { UserService } from 'src/app/core/services/user.service';

@Component({
  selector: 'app-typing-board',
  templateUrl: './typing-board.component.html',
  styleUrls: ['./typing-board.component.scss'],
})
export class TypingBoardComponent implements OnDestroy {
  isGameStarted: boolean = false;
  isEndDialogShown: boolean = false;

  textModel: Model = ModelFactory.createModel();

  writtenText: string = '';
  writtenTextModel: Model | undefined;

  currentWordIndex: number = 0;
  interval: any;
  startedAt: number = 0;

  errorCount: number = 0;
  speed: number = 0;
  bestSpeed: number = 0;
  accuracy: number = 0;

  @ViewChild('dialog') dialog!: ElementRef<HTMLDialogElement>;

  @HostListener('document:keypress', ['$event'])
  handleKeyboardEvent(event: KeyboardEvent) {
    if (this.isEndDialogShown) {
      return;
    }
    if (!this.isGameStarted) {
      this.init();
    }
    let letter = this.textModel.getLetterFromIndex(this.currentWordIndex);
    let isLetter = this.textModel.isLetterFromIndex(this.currentWordIndex);
    if (event.key === ' ' && isLetter) {
      this.writtenText += '.';
    } else if (event.key === 'Enter') {
      this.writtenText += '\n';
    } else {
      this.writtenText += event.key;
    }
    if (event.key !== letter) {
      this.errorCount++;
    }
    this.writtenTextModel = new Model(this.writtenText);
    this.currentWordIndex++;
    this.accuracy = Math.abs(
      ((this.writtenText.length - this.errorCount) / this.writtenText.length) *
        100
    );
    if (this.currentWordIndex == this.textModel.length) {
      this.isGameStarted = false;
      this.isEndDialogShown = true;
      this.dialog.nativeElement.showModal();
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
    if (this.isGameStarted && this.writtenText.length > 0) {
      this.writtenText = this.writtenText.slice(0, this.writtenText.length - 1);
      this.writtenTextModel = new Model(this.writtenText);
    }
    if (this.currentWordIndex > 0) {
      this.currentWordIndex--;
    }
  }

  constructor(
    private userService: UserService,
    private tokenService: TokenService
  ) {}

  ngOnDestroy(): void {
    if (this.interval) {
      clearInterval(this.interval);
    }
  }

  closeDialog(): void {
    this.dialog.nativeElement.close();
    this.reset();
    this.isEndDialogShown = false;
  }

  private init(): void {
    this.isGameStarted = true;
    this.startedAt = new Date().getTime();
    this.interval = setInterval(() => {
      this.calculateSpeed();
    }, 100);
  }

  private reset(): void {
    if (this.interval) {
      clearInterval(this.interval);
    }
    this.writtenText = '';
    this.writtenTextModel = ModelFactory.createEmptyModel();
    this.textModel = ModelFactory.createModel();
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

  public getClass(
    letter: string,
    lineIndex: number,
    wordIndex: number,
    letterIndex: number
  ): string {
    if (!this.exists(lineIndex, wordIndex, letterIndex)) {
      return 'neutral-letter';
    }
    if (this.isCorrect(letter, lineIndex, wordIndex, letterIndex)) {
      return 'correct-letter';
    } else {
      return 'wrong-letter';
    }
  }

  public exists(
    lineIndex: number,
    wordIndex: number,
    letterIndex: number
  ): boolean {
    let letter =
      this.writtenTextModel?.lines[lineIndex]?.line[wordIndex]?.letter[
        letterIndex
      ];
    return letter !== undefined;
  }

  public isCorrect(
    letter: string,
    lineIndex: number,
    wordIndex: number,
    letterIndex: number
  ): boolean {
    return (
      this.writtenTextModel?.lines[lineIndex]?.line[wordIndex]?.letter[
        letterIndex
      ] == letter
    );
  }
}
