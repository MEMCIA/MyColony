namespace Assets.Scripts.Game
{
    class MoveData
    {
        public MoveData(IField start, IField destination, int value)
        {
            Start = start;
            Destination = destination;
            Value = value;        }

        public IField Start;
        public IField Destination;
        public int Value;
    }
}
