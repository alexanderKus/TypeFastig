export class Model {
  public lines: Line[] = [];

  constructor(data: string = '') {
    let lines: string[] = data.trim().split('\n');
    for (let i = 0; i < lines.length; i++) {
      this.lines.push(new Line(lines[i]));
    }
  }

  public get length() {
    return this.lines.toString().length;
  }

  public isLetterFromIndex(index: number): boolean {
    let modelAsString = this.lines.toString();
    if (index > modelAsString.length) {
      return false;
    }
    let letter = modelAsString[index];
    return letter !== undefined && letter !== ' ';
  }

  public getLetterFromIndex(index: number): string {
    let modelAsString = this.lines.toString();
    if (index > modelAsString.length) {
      return '';
    }
    return modelAsString[index];
  }

  public toString(): string {
    return this.lines.join('\n');
  }
}

export class Line {
  public line: Word[] = [];

  constructor(line: string = '') {
    let words = line.trim().split(' ');
    for (let i = 0; i < words.length; i++) {
      this.line.push(new Word(words[i]));
    }
  }

  public toString(): string {
    return this.line.join(' ');
  }
}

export class Word {
  public letter: string[] = [];

  constructor(word: string = '') {
    this.letter = word.trim().split('');
  }

  public toString(): string {
    return this.letter.join('');
  }
}
