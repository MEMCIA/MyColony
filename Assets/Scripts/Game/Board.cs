using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Game
{
    class Board : IBoard
    {
        public Board(Vector2Int dimensions)
        {
            Dimensions = dimensions;
            CreateBoardCoordinates();
            CreateFields();
        }

        // width & height
        public Vector2Int Dimensions { get; }

        List<Vector2Int> _coordinates = new List<Vector2Int>();
        List<IField> allFields = new List<IField>();

        public void CreateFields()
        {
            for (int i = 0; i < _coordinates.Count; i++)
            {
                allFields.Add(new Field(_coordinates[i]));
            }
        }
        public IField GetField(Vector2Int position)
        {
            int index = position.y * (Dimensions.x+1) + position.x + 1;

                if (allFields[index].Position == position) return allFields[index];
 
            return null;
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
                if (GetField(n) != null) availableMoves.Add(GetField(n));
            }
            return availableMoves;
        }

        List<Vector2Int> FindNeighborOfFieldCoordinates(Vector2Int position)
        {
            List<Vector2Int> neighbors = new List<Vector2Int>();
            neighbors.Add(new Vector2Int(position.x+1, position.y));
            neighbors.Add(new Vector2Int(position.x-1, position.y));
            neighbors.Add(new Vector2Int(position.x, position.y+1));
            neighbors.Add(new Vector2Int(position.x, position.y-1));
            neighbors.Add(new Vector2Int(position.x+1, position.y+1));
            neighbors.Add(new Vector2Int(position.x+1, position.y-1));
            neighbors.Add(new Vector2Int(position.x-1, position.y+1));
            neighbors.Add(new Vector2Int(position.x-1, position.y-1));
            return neighbors;
        }

        // try to place pawn at spe
        public bool PlacePawnAt(Vector2Int position, int owner)
        {
           var field =  GetField(position);
            if (field == null) return false;
            if (field.Pawn != null) return false;
            field.Pawn = new Pawn(owner);
            //tutaj jeszcze mogłabym dopisać dodawanie do listy pionków gracza
            return true;
        }

        public void CreateBoardCoordinates()
        {
            for (int i = 0; i < Dimensions.x; i++)
            {
                for (int j = 0; j < Dimensions.y; j++)
                {
                    _coordinates.Add(new Vector2Int(i, j));
                }
            }
        }
    }
}
