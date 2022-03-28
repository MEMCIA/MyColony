using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Game
{
    class Board : IBoard
    {
        public Board(Vector2Int dimensions)
        {
            Dimensions = dimensions;
            CreateFields();
        }

        // width & height
        public Vector2Int Dimensions { get; }
        List<IField> _allFields = new List<IField>();

        public void CreateFields()
        {
            for (int i = 0; i < Dimensions.y; i++)
            {
                for (int j = 0; j < Dimensions.x; j++)
                {
                    _allFields.Add(new Field(new Vector2Int(j, i)));
                }
            }
        }

        public IField GetField(Vector2Int position)
        {
            int index = position.y * Dimensions.x + position.x;
            if (index < 0 || index >= _allFields.Count) return null;
            return _allFields[index];
        }

        public List<IField> GetAllFields()
        {
            return _allFields;
        }

        public List<IField> GetAvailableMovesFor(Vector2Int position)
        {
            List<Vector2Int> neighborsCoordinates = FindNeighborOfFieldCoordinates(position);
            List<IField> availableMoves = new List<IField>();
            foreach (var n in neighborsCoordinates)
            {
                var field = GetField(n);
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

        // try to place pawn at spe
        public bool PlacePawnAt(Vector2Int position, int owner)
        {
            var field = GetField(position);
            if (field == null) return false;
            if (field.IsEmpty()) return false;
            field.Pawn = new Pawn(owner);
            return true;
        }
    }
}
