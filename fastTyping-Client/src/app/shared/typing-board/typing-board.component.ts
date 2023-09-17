import {
  Component,
  ElementRef,
  HostListener,
  OnDestroy,
  ViewChild,
} from '@angular/core';
import { Score } from 'src/app/core/models/score.model';
import { TokenService } from 'src/app/core/services/token.service';
import { UserService } from 'src/app/core/services/user.service';
import { TextEditor } from '../textEditor';

@Component({
  selector: 'app-typing-board',
  templateUrl: './typing-board.component.html',
  styleUrls: ['./typing-board.component.scss'],
})
export class TypingBoardComponent implements OnDestroy {
  x = `\
static void *proc_keys_start(struct seq_file *p, loff_t *_pos)
  __acquires(key_serial_lock)
{
  key_serial_t pos = *_pos;
	struct key *key;
  spin_lock(&key_serial_lock);
  if (*_pos > INT_MAX)
    return NULL;
  key = find_ge_key(p, pos);
  if (!key)
    return NULL;
  *_pos = key->serial;
  return &key->serial_node;
}`; // TODO: move this to factory
  xE = new TextEditor(this.x);
  y = '';
  yE = new TextEditor(this.y);

  isGameStarted: boolean = false;
  isEndDialogShown: boolean = false;

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
    if (!this.isGameStarted) {
      this.init();
    }
    if (this.currentWordIndex > this.xE.text.length) {
      return;
    } else if (event.key === 'Enter') {
      return;
    }
    this.y += event.key;
    this.currentWordIndex++;
    if (this.xE.getCharAtIndex(this.currentWordIndex) === '\n') {
      this.y += '\n';
      this.currentWordIndex++;
    }
    this.yE = new TextEditor(this.y);
    console.clear();
    console.log(
      `${this.currentWordIndex}\n${this.yE.text}\n${this.yE.getCharAtIndex(
        this.currentWordIndex - 1
      )}\n${this.xE.text.substring(
        0,
        this.currentWordIndex
      )}\n${this.xE.getCharAtIndex(this.currentWordIndex - 1)}`
    );
    this.accuracy = Math.abs(
      ((this.y.length - this.errorCount) / this.y.length) * 100
    );
    if (this.currentWordIndex == this.xE.text.length) {
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
    if (this.y.length > 0) {
      if (this.yE.getCharAtIndex(this.currentWordIndex - 1) === '\n') {
        this.currentWordIndex -= 2;
        this.y = this.y.substring(0, this.y.length - 2);
      } else {
        this.y = this.y.substring(0, this.y.length - 1);
        this.currentWordIndex--;
      }
      this.yE = new TextEditor(this.y);
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

  getStyleClassForCurrentLetter(lineIndex: number, wordIndex: number): string {
    let index = this.calculateCorrentIndex(lineIndex, wordIndex);
    if (index === this.currentWordIndex) {
      return 'current-letter';
    }
    return '';
  }

  getStyleClass(letter: string, lineIndex: number, wordIndex: number): string {
    let index = this.calculateCorrentIndex(lineIndex, wordIndex);
    const xEChar = this.xE.getCharAtIndex(index);
    const yEChar = this.yE.getCharAtIndex(index);
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
    }, 100);
  }

  private reset(): void {
    if (this.interval) {
      clearInterval(this.interval);
    }
    // TODO: gerenate new xE
    this.y = '';
    this.yE = new TextEditor('');
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

  private calculateCorrentIndex(lineIndex: number, wordIndex: number): number {
    let index = 0;
    for (let i = 0; i < lineIndex; i++) {
      index += this.xE.lines[i].length + 1; // +1 because of '\n'
    }
    index += wordIndex;
    return index;
  }
}
