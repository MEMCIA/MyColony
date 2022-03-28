using UnityEngine;
using System;

namespace Assets.Scripts.Game
{
    [Serializable]
    class Field : IField
    {

        public Field(Vector2Int position)
        {
            Position = position;
        }

        public Field()
        {

        }

        // position of this field
        public Vector2Int Position { get; }

        // Pawn at this place, if any
        public IPawn Pawn { get; set; }

        // is field emtpy, and can be moved into?
        public bool IsEmpty()
        {
            if (Pawn == null) return true;

            return false;
        }
    }
}
