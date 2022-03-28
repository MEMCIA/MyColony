using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Game
{
    class PlayerPawns
    {
        List<Vector2Int> PawnsCoordinatesPlayer = new List<Vector2Int>();

        public PlayerPawns(List<Vector2Int> pawnsCoordinatesPlayer)
        {
            PawnsCoordinatesPlayer = pawnsCoordinatesPlayer;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="board"></param>
        /// <param name="owner"></param> 1 albo 2 albo 0
        public void SetPawnOwner(Board board, int owner)
        {
            List<IField> fields = board.GetAllFields();
            for (int i = 0; i < fields.Count; i++)
            {
                for (int j = 0; j < PawnsCoordinatesPlayer.Count; j++)
                {
                    if (fields[i].Position != PawnsCoordinatesPlayer[j]) continue;
                    fields[i].Pawn.Owner = owner;
                }
            }

        }
    }
}
