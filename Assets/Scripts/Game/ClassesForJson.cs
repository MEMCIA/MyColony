using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Game 
{
    [Serializable]
    class SaveBoard
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
    class SaveField
    {
        public SaveField(int pawnOwner)
        {
            PawnOwner = pawnOwner;
        }

        public int PawnOwner;
    }

}
