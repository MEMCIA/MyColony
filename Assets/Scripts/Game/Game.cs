using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Game
{
    class Game
    {
        IBoard _board;
        int _activePlayer = 1;
        int _availableDistanceToMove = 2;
        int _distanceInWhichPawnIsNotDeleted = 1;
        public Game(IBoard board)
        {
            _board = board;
        }

        bool CheckIfPawnBelongsToCurrentPlayer(IField start)
        {
            if (start.Pawn.Owner == _activePlayer) return true;
            return false;
        }

        public bool MakeMove(IField start, IField target)
        {
            if (start.Pawn == null)
                return false;
            if (!target.IsEmpty())
                return false;
            if (!CheckIfPawnBelongsToCurrentPlayer(start)) return false;
            List<IField> availableMoves = GetAvailableMovesFor(start.Position);
            if (!IsValidMove(availableMoves, target)) return false;
            _board.PlacePawnAt(target.Position, start.Pawn.Owner);
            int distanceBetween = CheckDistanceOfMakedMove(start, target);
            if (!CheckIfStartPawnMustBeDeleted(distanceBetween)) return true;
            _board.RemovePawn(start.Position);
            return true;
        }

        void ChangeOwnerOfNeighboringPawns(IField target)
        {
            List<Vector2Int> newNeighborsCoordinates = FindAvailableCoordinatesInDistance(target.Position, 1);
            List<IField> newNeighborsFields = new List<IField>();
            //TODO sprawdź czy sąsiadujące pola mają pawna, jeśli tak, to zmień jego kolor
        }

        bool CheckIfStartPawnMustBeDeleted(int distanceBetween)
        {
            if (distanceBetween > _distanceInWhichPawnIsNotDeleted) return true;
            return false;
        }

        int CheckDistanceOfMakedMove(IField start, IField target)
        {
            int distanceX = Mathf.Abs(start.Position.x - target.Position.x);
            int distanceY = Mathf.Abs(start.Position.y - target.Position.y);
            return Mathf.Max(distanceX, distanceY);
        }

        bool IsValidMove(List<IField> availableMoves, IField target)
        {
            return availableMoves.Contains(target);
        }

        public List<IField> GetAvailableMovesFor(Vector2Int position)
        {
            List<Vector2Int> availableCoordinatesToCheck = FindAvailableCoordinatesInDistance(position, _availableDistanceToMove);
            List<IField> finalAvailableMoves = new List<IField>();

            foreach (var n in availableCoordinatesToCheck)
            {
                var field = _board.GetField(n);
                if (field == null || !field.IsEmpty()) continue;
                if(field.Pawn ==null) finalAvailableMoves.Add(field);
            }
            return finalAvailableMoves;
        }

        List<Vector2Int> FindAvailableCoordinatesInDistance(Vector2Int position, int distance)
        {
            int length = distance * 2 + 1;
            Vector2Int positionOfLeftDownAvaiableField = new Vector2Int(position.x - distance, position.y - distance);
            List<Vector2Int> avaiableCoordinates = new List<Vector2Int>();

            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    Vector2Int avaiableCoordinate = positionOfLeftDownAvaiableField + new Vector2Int(i,j);
                    if (avaiableCoordinate == position) continue;
                    avaiableCoordinates.Add(avaiableCoordinate);
                }
            }

            return avaiableCoordinates;
        }

    }
}
