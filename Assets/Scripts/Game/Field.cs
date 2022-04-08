using UnityEngine;

namespace Assets.Scripts.Game
{
    class Field : IField
    {
        public Field(Vector2Int position)
        {
            Position = position;
        }

        public bool IsObstacle { get; } = false;

        // position of this field
        public Vector2Int Position { get; }

        // Pawn at this place, if any
        public IPawn Pawn { get; set; }

        // is field emtpy, and can be moved into?
        public bool IsEmpty()
        {
            if (Pawn == null&&!IsObstacle) return true;

            return false;
        }

        public void RemovePawn()
        {
            Pawn = null;
        }
    }
}
