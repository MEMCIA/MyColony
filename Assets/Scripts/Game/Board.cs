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
        List<IField> allFields = new List<IField>();

        public void CreateFields()
        {
            for (int i = 0; i < Dimensions.x; i++)
            {
                for (int j = 0; j < Dimensions.y; j++)
                {
                    allFields.Add(new Field(new Vector2Int(i, j)));
                }
            }
        }

        public IField GetField(Vector2Int position)
        {
            int index = position.y * (Dimensions.x) + position.x;
            if (index < 0 || index > allFields.Count) return null;
            return allFields[index];
        }

        public List<IField> GetAllFields()
        {
            return allFields;
        }

        public List<IField> GetAvailableMovesFor(Vector2Int position)
        {
            List<Vector2Int> neighborsCoordinates = FindNeighborOfFieldCoordinates(position);
            List<IField> availableMoves = new List<IField>();
            foreach (var n in neighborsCoordinates)
            {
                var field = GetField(n);
                if (field != null || field.Pawn == null) availableMoves.Add(field);
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
            if (field.Pawn != null) return false;
            field.Pawn = new Pawn(owner);
            //tutaj jeszcze mogłabym dopisać dodawanie do listy pionków gracza
            return true;
        }
    }
}
