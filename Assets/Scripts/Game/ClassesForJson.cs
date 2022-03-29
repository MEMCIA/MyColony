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

        public static SaveBoard CreateFromBoard(IBoard board)
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

            return new SaveBoard(board.Dimensions.x, board.Dimensions.y, savefields);
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
