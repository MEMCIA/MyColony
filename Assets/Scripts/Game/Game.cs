using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Game
{
    class Game
    {
        IBoard _board;
        GameUtils _utils;
        int _numberOfPlayers = 2;
        int _activePlayer = 0;
        int _distanceInWhichPawnIsNotDeleted = 1;
        bool _gameOver = false;
        int[] _pawnsOfPlayer;

        public Game(IBoard board)
        {
            _board = board;
            _utils = new GameUtils(board);
            _numberOfPlayers = _utils.GetOwnerWithHighestId() + 1;
            _pawnsOfPlayer = new int[_numberOfPlayers];
            CalculateAmountOfPawns();
        }

        public GameUtils Utils()
        {
            return _utils;
        }

        public int AmountOfPawns(int player)
        {
            return _pawnsOfPlayer[player];
        }

        public void Turn(IField start, IField target)
        {
            if (MakeMove(start, target))
            {
                if (!SetNextActivePlayer()) _gameOver = true;
            }
            CalculateAmountOfPawns();
        }

        public int GetNumberOfPlayers()
        {
            return _numberOfPlayers;
        }

        public int GetActivePlayer()
        {
            return _activePlayer;
        }

        public bool IsGameOver()
        {
            // TODO detect end of game
            return _gameOver;
        }

        public IBoard Board()
        {
            return _board;
        }

        void SetNextPlayer()
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

        public bool SetNextActivePlayer()
        {
            int playersThatCannotMove = 0;

            for (int i = 0; i < _numberOfPlayers; i++)
            {
                SetNextPlayer();
                IEnumerable<IField> fieldsWithavailableMovesOfPlayer = _utils.GetAllFieldsWithAvailableMoves(_activePlayer);

                if (fieldsWithavailableMovesOfPlayer.Count() != 0) break;

                playersThatCannotMove++;
            }

            return playersThatCannotMove != _numberOfPlayers;
        }

        bool CheckIfPawnBelongsToCurrentPlayer(IField start)
        {
            if (start.Pawn.Owner == _activePlayer) return true;
            return false;
        }

        public bool MakeMove(IField start, IField target)
        {
            if (!IsValidMove(start, target))
                return false;

            _board.PlacePawnAt(target.Position, start.Pawn.Owner);

            int distanceBetween = _utils.CheckDistanceBetween(start, target);
            if (CheckIfStartPawnMustBeDeleted(distanceBetween))
                _board.RemovePawn(start.Position);

            ChangeOwnerOfNeighboringPawns(target);
            return true;
        }

        void ChangeOwnerOfNeighboringPawns(IField target)
        {
            List<IField> fieldWithEnemyPawns = _utils.FindEnemiesPawnsInNeighborhood(target);

            foreach (var f in fieldWithEnemyPawns)
            {
                f.Pawn.SetOwner(target.Pawn.Owner);
            }
        }


        bool CheckIfStartPawnMustBeDeleted(int distanceBetween)
        {
            if (distanceBetween > _distanceInWhichPawnIsNotDeleted) return true;
            return false;
        }


        public bool IsValidMove(IField start, IField target)
        {
            if (start.Pawn == null)
                return false;
            if (!target.IsEmpty())
                return false;
            if (!CheckIfPawnBelongsToCurrentPlayer(start))
                return false;

            List<IField> availableMoves = _utils.GetAvailableMovesFor(start.Position);
            return availableMoves.Contains(target);
        }


        void CalculateAmountOfPawns()
        {
            for (int i = 0; i < _numberOfPlayers; i++)
                _pawnsOfPlayer[i] = 0;
            
            foreach (var field in _board.GetAllFields())
            {
                if (field.Pawn == null) continue;
                _pawnsOfPlayer[field.Pawn.Owner] ++;
            }
        }
    }
}
