using System.Collections.Generic;
using UnityEngine;

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

        public List<IField> GetAvailableMovesFor(Vector2Int position)
        {
            List<Vector2Int> neighborsCoordinates = FindNeighborOfFieldCoordinates(position);
            List<IField> availableMoves = new List<IField>();
            foreach (var n in neighborsCoordinates)
            {
                var field = _board.GetField(n);
                if (field != null || field.IsEmpty()) availableMoves.Add(field);
            }
            return availableMoves;
        }

        List<Vector2Int> FindNeighborOfFieldCoordinates(Vector2Int position)
        {
            List<Vector2Int> neighbors = new List<Vector2Int>();
            neighbors.Add(new Vector2Int(position.x + 1, position.y));
            neighbors.Add(new Vector2Int(position.x - 1, position.y));
            neighbors.Add(new Vector2Int(position.x, position.y + 1));
            neighbors.Add(new Vector2Int(position.x, position.y - 1));
            neighbors.Add(new Vector2Int(position.x + 1, position.y + 1));
            neighbors.Add(new Vector2Int(position.x + 1, position.y - 1));
            neighbors.Add(new Vector2Int(position.x - 1, position.y + 1));
            neighbors.Add(new Vector2Int(position.x - 1, position.y - 1));
            return neighbors;
        }

    }
}
