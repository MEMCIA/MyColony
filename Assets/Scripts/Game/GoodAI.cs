using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Game
{
    class GoodAI : AIPlayer
    {
        override public void OnTurnStart(Game game)
        {
            MoveData bestMove = FindBestMoveForGoodAI(game);
            game.Turn(bestMove.Start, bestMove.Destination);
        }

        MoveData FindBestMoveForGoodAI(Game game)
        {
            List<MoveData> moveDataList = CreateMoveDataList(game);
            var moveDataListinOrder = moveDataList.OrderByDescending(x => x.Value).ToList();
            List<MoveData> bestMoves = new List<MoveData> { moveDataListinOrder[0] };

            for (int i = 2; i < moveDataListinOrder.Count; i++)
            {
                if (moveDataListinOrder[-1 + i].Value == moveDataListinOrder[-2 + i].Value)
                {
                    bestMoves.Add(moveDataListinOrder[-1 + i]);
                }
                else
                {
                    break;
                }
            }

            int randomIndex = Random.Range(0, bestMoves.Count);

            return bestMoves[randomIndex];
        }

        List<MoveData> CreateMoveDataList(Game game)
        {
            List<IField> pawnsOfPlayer = game.Utils().GetAllFieldsWithAvailableMoves(game.GetActivePlayer()).ToList();
            List<MoveData> moves = new List<MoveData>();

            foreach (var start in pawnsOfPlayer)
            {
                List<IField> availableMoves = game.Utils().GetAvailableMovesFor(start.Position);

                foreach (var target in availableMoves)
                {
                    int numberOfFieldsWithEnemyPawns = game.Utils().FindEnemiesPawnsInNeighborhood(target, game.GetActivePlayer()).Count;
                    moves.Add(new MoveData(start, target, numberOfFieldsWithEnemyPawns));
                }
            }

            return moves;
        }
    }
}
