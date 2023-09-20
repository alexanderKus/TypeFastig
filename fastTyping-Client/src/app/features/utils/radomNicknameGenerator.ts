export class RandomNicknameGenerator {
  private static nicknameLength = 12;
  private static characters =
    'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789';

  public static generate(): string {
    const nicknameArray = [];
    for (let i = 0; i < this.nicknameLength; i++) {
      const randomIndex = Math.floor(Math.random() * this.characters.length);
      nicknameArray.push(this.characters.charAt(randomIndex));
    }
    return nicknameArray.join('');
  }
}
