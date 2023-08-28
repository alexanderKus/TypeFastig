import { Model } from '../../features/models/model';

export class ModelFactory {
  private static prevIndex: number = 0;
  private static texts = new Map<number, string>([
    // TODO: Create more templates
    [0, 'siema'],
    [1, 'aleksander'],
    [2, 'olek'],
    [3, 'kuznia'],
    /*
    [
      1,
      `static void *proc_keys_start(struct seq_file *p, loff_t *_pos)
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
}`,
    ], */
  ]);

  public static createModel(): Model {
    const randomIndex = this.getRandomIndex();
    const text = this.texts.get(randomIndex);
    return new Model(text);
  }

  public static createEmptyModel(): Model {
    return new Model('');
  }

  private static getRandomIndex(): number {
    const randomIndex = Math.floor(Math.random() * this.texts.size);
    if (this.prevIndex !== randomIndex) {
      this.prevIndex = randomIndex;
      return randomIndex;
    }
    return this.getRandomIndex();
  }
}
