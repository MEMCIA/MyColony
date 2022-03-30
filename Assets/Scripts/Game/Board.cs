﻿using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Game
{
    public class Board : IBoard
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

        public IField GetFieldFromIndex(int index)
        {
            if (index < 0 || index >= _allFields.Count) return null;
            return _allFields[index];
        }

        public List<IField> GetAllFields()
        {
            return _allFields;
        }

        // try to place pawn at spe
        public bool PlacePawnAt(Vector2Int position, int owner)
        {
            var field = GetField(position);
            if (field == null) return false;
            if (!field.IsEmpty()) return false;
            field.Pawn = new Pawn(owner);
            return true;
        }

        public bool RemovePawn(Vector2Int position)
        {
            var field = GetField(position);
            if (field == null) return false;
            if (field.IsEmpty()) return false;
            field.SetPawnToNull();
            return true;
        }
    }
}
