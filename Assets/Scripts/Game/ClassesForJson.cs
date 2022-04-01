using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Game 
{
    [Serializable]
    public class SaveBoard
    {
        public int Width;
        public int Height;
        public List<SaveField> Fields;

        public SaveBoard(int width, int height, List<SaveField> fields)
        {
            Width = width;
            Height = height;
            Fields = fields;
        }

        public static string BoardToJSON(IBoard board)
        {
            var allFieldsBoard = board.GetAllFields();
            List<SaveField> savefields = new List<SaveField>();

            for (int i = 0; i < allFieldsBoard.Count; i++)
            {
                if (allFieldsBoard[i].Pawn == null)
                {
                    savefields.Add(new SaveField(SaveField.NoPawn));
                    continue;
                }
                savefields.Add(new SaveField(allFieldsBoard[i].Pawn.Owner));
            }

            SaveBoard sv = new SaveBoard(board.Dimensions.x, board.Dimensions.y, savefields);
            return SaveBoard.ToJSON(sv);
        }

        public static Board CreateFromJSON(string JSON)
        {
            if (string.IsNullOrEmpty(JSON)) return new Board(new Vector2Int(3,3));

            SaveBoard sv = SaveBoard.FromJSON(JSON);
            Board board = new Board(new Vector2Int(sv.Width, sv.Height));
            board.CreateFields();

            for (int i = 0; i < sv.Fields.Count; i++)
            {
                if (sv.Fields[i].PawnOwner == SaveField.NoPawn)
                {
                    continue;
                }

                IField field = board.GetFieldFromIndex(i);
                field.Pawn = new Pawn(sv.Fields[i].PawnOwner);
            }

            return board;
        }

        public static string ToJSON(SaveBoard b)
        {
            return JsonUtility.ToJson(b);
        }

        public static SaveBoard FromJSON(string json)
        {
            return JsonUtility.FromJson<SaveBoard>(json);
        }
    }

    [Serializable]
    public class SaveField
    {
        public const int NoPawn = -1;
        public SaveField(int pawnOwner)
        {
            PawnOwner = pawnOwner;
        }

        public int PawnOwner;
    }

}
