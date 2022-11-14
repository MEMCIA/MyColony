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
            List<MoveData> bestMoves = new List<MoveData>();
            bestMoves = moveDataList.Where(move => move.Value == firstBestMove.Value).ToList();
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
                    int distanceFromStartPawn = game.Utils().CheckDistanceBetween(start, target);
                    int pointsForNewPawn = distanceFromStartPawn == game.DistanceInWhichPawnIsNotDeleted ? 1 : 0;
                    int value = numberOfFieldsWithEnemyPawns + pointsForNewPawn;
                    moves.Add(new MoveData(start, target, value));
                }
            }

            return moves;
        }
    }
}
