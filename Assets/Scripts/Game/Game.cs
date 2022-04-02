using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Game
{
    class Game
    {
        IBoard _board;
        int _numberOfPlayers = 2;
        int _activePlayer = 0;
        int _availableDistanceToMove = 2;
        int _distanceInWhichPawnIsNotDeleted = 1;

        public Game(IBoard board)
        {
            _board = board;
        }

        public void Turn(IField start, IField target)
        {
            if(MakeMove(start,target))
            {
                SetNextActivePlayer();
            }
        }

        void SetNumberOfPlayers(int number)
        {
            _numberOfPlayers = number;
        }

        void SetNextActivePlayer()
        {
            int lastPlayer = _numberOfPlayers - 1;
            if (_activePlayer == lastPlayer)
            {
                _activePlayer = 0;
            }
            else
            {
                _activePlayer++;
            }
        }

        bool CheckIfPawnBelongsToCurrentPlayer(IField start)
        {
            if (start.Pawn.Owner == _activePlayer) return true;
            return false;
        }

        bool MakeMove(IField start, IField target)
        {
            if (!IsValidMove(start, target)) 
                return false;
            
            _board.PlacePawnAt(target.Position, start.Pawn.Owner);

            int distanceBetween = CheckDistanceBetween(start, target);
            if (CheckIfStartPawnMustBeDeleted(distanceBetween))
                _board.RemovePawn(start.Position);

            ChangeOwnerOfNeighboringPawns(target);
            return true;
        }

        void ChangeOwnerOfNeighboringPawns(IField target)
        {
            List<IField> fieldWithEnemyPawns = FindEnemiesPawnsInNeighborhood(target);

            foreach (var f in fieldWithEnemyPawns)
            {
                f.Pawn.SetOwner(target.Pawn.Owner);
            }
        }

        List<IField> FindEnemiesPawnsInNeighborhood(IField target)
        {
            List<IField> neighbors = FindFieldsinDistance(target.Position, 1);
            List<IField> fieldWithEnemyPawns = new List<IField>();

            foreach (var n in neighbors)
            {
                if (n == null || n.IsEmpty()) continue;
                if (n.Pawn == null) continue;
                if (n.Pawn.Owner == target.Pawn.Owner) continue;
                fieldWithEnemyPawns.Add(n);
            }

            return fieldWithEnemyPawns;
        }

        bool CheckIfStartPawnMustBeDeleted(int distanceBetween)
        {
            if (distanceBetween > _distanceInWhichPawnIsNotDeleted) return true;
            return false;
        }

        int CheckDistanceBetween(IField start, IField target)
        {
            int distanceX = Mathf.Abs(start.Position.x - target.Position.x);
            int distanceY = Mathf.Abs(start.Position.y - target.Position.y);
            return Mathf.Max(distanceX, distanceY);
        }

        bool IsValidMove(IField start, IField target)
        {
            if (start.Pawn == null)
                return false;
            if (!target.IsEmpty())
                return false;
            if (!CheckIfPawnBelongsToCurrentPlayer(start))
                return false;

            List<IField> availableMoves = GetAvailableMovesFor(start.Position);
            return availableMoves.Contains(target);
        }

        public List<IField> GetAvailableMovesFor(Vector2Int position)
        {
            List<IField> fieldsWithAvailableCoordinates = FindFieldsinDistance(position, _availableDistanceToMove);
            List<IField> finalAvailableMoves = new List<IField>();

            foreach (var n in fieldsWithAvailableCoordinates)
            {
                if (n == null || !n.IsEmpty()) continue;
                if(n.Pawn ==null) finalAvailableMoves.Add(n);
            }
            return finalAvailableMoves;
        }

        List<IField> FindFieldsinDistance(Vector2Int position, int distance)
        {
            int length = distance * 2 + 1;
            Vector2Int positionOfLeftDownAvailableField = new Vector2Int(position.x - distance, position.y - distance);
            List<Vector2Int> availableCoordinates = new List<Vector2Int>();

            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    Vector2Int availableCoordinate = positionOfLeftDownAvailableField + new Vector2Int(i,j);
                    if (availableCoordinate == position) continue;
                    availableCoordinates.Add(availableCoordinate);
                }
            }

            return CoordinatesToFields(availableCoordinates);
        }

        List<IField> CoordinatesToFields(List<Vector2Int> coordinates)
        {
            var fields = from a in _board.GetAllFields()
                        where coordinates.Contains(a.Position)
                        select a;
            return fields.ToList();
        }

    }
}
