namespace Assets.Scripts.Game
{
    class Pawn : IPawn
    {
        public Pawn(int owner)
        {
            Owner = owner;
        }

        public int Owner { get; set; }
    }
}
