using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Game
{
    class Field:IField
    {
        public Field(Vector2Int position)
        {
            Position = position;
        }
        // position of this field
        public Vector2Int Position { get; }

        // Pawn at this place, if any
        public IPawn Pawn { get; }

        // is field emtpy, and can be moved into?
        public bool IsEmpty()
        {
            if ( Pawn == null) return true;
            return false;
        }
    }
}
