namespace Assets.Scripts.Game
{
    class Game
    {
        IBoard _board;

        public Game(IBoard board)
        {
            _board = board;
        }

        public bool MakeMove(IField start, IField target)
        {
            if (start.Pawn == null)
                return false;
            if (!target.IsEmpty())
                return false;
            _board.PlacePawnAt(target.Position, start.Pawn.Owner);
            return true;
        }

    }
}
