export const texts = new Map<number, string>([
  [
    0,
    `\
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
}`,
  ],
  [
    1,
    `\
class AdagradOptimizer(optimizer.Optimizer):
  def __init__(self, learning_rate, initial_accumulator_value=0.1,
    use_locking=False, name="Adagrad"):
    if initial_accumulator_value <= 0.0:
      raise ValueError("initial_accumulator_value must be positive: %s" %
                        initial_accumulator_value)
  super(AdagradOptimizer, self).__init__(use_locking, name)
  self._learning_rate = learning_rate
  self._initial_accumulator_value = initial_accumulator_value
  self._learning_rate_tensor = None
`,
  ],
]);
