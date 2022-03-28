using System;

namespace Assets.Scripts.Game
{
    [Serializable]
    class Pawn : IPawn
    {
        public Pawn(int owner)
        {
            Owner = owner;
        }

        public int Owner { get; set; }
    }
}
