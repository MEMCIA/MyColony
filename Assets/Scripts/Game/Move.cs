using System.Collections.Generic;

namespace Assets.Scripts.Game
{
    class Move
    {
        public Move(int owner, bool jump, IField start, IField target, List<IField> capturedFields)
        {
            Owner = owner;
            Jump = jump;
            Start = start;
            Target = target;
            CapturedFields = capturedFields;
        }

        public int Owner { get; private set; }
        public bool Jump { get; private set; }
        public IField Start { get; private set; }
        public IField Target { get; private set; }
        public List<IField> CapturedFields { get; private set; }
    }
}
