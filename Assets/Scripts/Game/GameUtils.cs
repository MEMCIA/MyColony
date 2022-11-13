using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Game
{
    class GameUtils
    {
        IBoard _board;
        int _availableDistanceToMove = 2;

        public GameUtils(IBoard board)
        {
            _board = board;
        }

        public int GetOwnerWithHighestId()
        {
            return _board.GetAllFields()
                .Where(field => field.Pawn != null)
                .Select(field => field.Pawn.Owner)
                .DefaultIfEmpty()
                .Max();
        }


        IEnumerable<IField> FindPawnsThatBelongsToPlayer(int player)
        {

            var filedsWithPawns = from a in _board.GetAllFields()
                                  where a.Pawn != null
                                  select a;

            var fieldsWithPawnsOfPlayer = from f in filedsWithPawns
                                          where f.Pawn.Owner == player
                                          select f;

            return fieldsWithPawnsOfPlayer;
        }

        public IEnumerable<IField> GetAllFieldsWithAvailableMoves(int player)
        {
            IEnumerable<IField> fieldsWithPawnsOfPlayer = FindPawnsThatBelongsToPlayer(player);

            var allFieldsWithAvailabeMoves = from a in fieldsWithPawnsOfPlayer
                                             where GetAvailableMovesFor(a.Position).Count > 0
                                             select a;

            return allFieldsWithAvailabeMoves;
        }

        public IBoard Board()
        {
            return _board;
        }

        public List<IField> FindEnemiesPawnsInNeighborhood(IField target, int activePlayer)
        {
            List<IField> neighbors = FindFieldsinDistance(target.Position, 1);
            List<IField> fieldWithEnemyPawns = new List<IField>();

            foreach (var n in neighbors)
            {
                if (n == null || n.IsEmpty()) continue;
                if (n.Pawn == null) continue;
                if (n.Pawn.Owner == activePlayer) continue;
                fieldWithEnemyPawns.Add(n);
            }

            return fieldWithEnemyPawns;
        }

        public int CheckDistanceBetween(IField start, IField target)
        {
            int distanceX = Mathf.Abs(start.Position.x - target.Position.x);
            int distanceY = Mathf.Abs(start.Position.y - target.Position.y);
            return Mathf.Max(distanceX, distanceY);
        }

        public List<IField> GetAvailableMovesFor(Vector2Int position)
        {
            List<IField> fieldsWithAvailableCoordinates = FindFieldsinDistance(position, _availableDistanceToMove);
            List<IField> finalAvailableMoves = new List<IField>();

            foreach (var n in fieldsWithAvailableCoordinates)
            {
                if (n == null || !n.IsEmpty()) continue;
                if (n.Pawn == null) finalAvailableMoves.Add(n);
            }

            return finalAvailableMoves;
        }

        public List<IField> FindFieldsinDistance(Vector2Int position, int distance)
        {
            int length = distance * 2 + 1;
            Vector2Int positionOfLeftDownAvailableField = new Vector2Int(position.x - distance, position.y - distance);
            List<Vector2Int> availableCoordinates = new List<Vector2Int>();

            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    Vector2Int availableCoordinate = positionOfLeftDownAvailableField + new Vector2Int(i, j);
                    if (availableCoordinate == position) continue;
                    availableCoordinates.Add(availableCoordinate);
                }
            }

            return _board.CoordinatesToFields(availableCoordinates).ToList();
        }

        public List<MoveData> FindMovesInDistance(List<MoveData> moves, int distance)
        {
            List<MoveData> movesInDistance= new List<MoveData>();

            foreach(var move in moves)
            {
                if (CheckDistanceBetween(move.Start, move.Destination) == distance) movesInDistance.Add(move);
            }

            return movesInDistance;
        }

    }
}
