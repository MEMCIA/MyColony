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
            var firstBestMove = moveDataListinOrder[0];

            if(firstBestMove.Value == 0)
            {
                var movesInDistance1 = game.Utils().FindMovesInDistance(moveDataList, 1);
                var movesInDistance1Count = movesInDistance1.Count;

                if (movesInDistance1Count > 0)
                {
                    int randomIndex1 = Random.Range(0, movesInDistance1Count);
                    return movesInDistance1[randomIndex1];
                }
            }

            List<MoveData> bestMoves = new List<MoveData>();
            bestMoves = moveDataList.Where(move => move.Value == firstBestMove.Value).ToList();
            int randomIndex2 = Random.Range(0, bestMoves.Count);
            return bestMoves[randomIndex2];
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
