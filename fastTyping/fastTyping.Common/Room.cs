using fastTyping.Common.Entity;

namespace fastTyping.Common
{
    public class Room
    {
        private readonly int _maxRoomSize = 5;
        private readonly List<Typist> _typists = new();
        public readonly Guid RoomId = Guid.NewGuid();

        public void AddTypist(Typist typist)
        {
            if (!IsFull())
            {
                _typists.Add(typist);
            }
            else
            {
                throw new InvalidOperationException("Room is full");
            }
        }

        public void RemoveTypist(Typist typist)
        {
            if (_typists.Contains(typist))
            {
                _typists.Remove(typist);
            }
            else
            {
                throw new InvalidOperationException(
                    "Typist do not exits in this room");
            }
        }
        public bool IsFull() => _typists.Count == _maxRoomSize;

        public IReadOnlyCollection<Typist> GetTypists()
        {
            return _typists.AsReadOnly();
        }

        public Room()
        {
        }
    }
}

