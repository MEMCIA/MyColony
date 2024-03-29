﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Game
{
    class Game
    {
        public UnityEvent<Move> OnMoveMade = new UnityEvent<Move>();
        IBoard _board;
        GameUtils _utils;
        int _numberOfPlayers = 2;
        int _activePlayer = 0;
        public int DistanceInWhichPawnIsNotDeleted { get; private set; } = 1;
        bool _gameOver = false;
        int[] _pawnsOfPlayer;
        int _winnerIndex;

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

        public int FindWinnerNumber()
        {
            _winnerIndex = Array.IndexOf(_pawnsOfPlayer, _pawnsOfPlayer.Max());
            return _winnerIndex;
        }

        public bool IsGameOver()
        {
            return _gameOver;
        }

        public int AmountOfPawns(int player)
        {
            return _pawnsOfPlayer[player];
        }

        public void Turn(IField start, IField target)
        {
            if (_gameOver && CheckIfAllFieldsAreOccupied()) return;

            if (MakeMove(start, target))
            {
                if (!SetNextActivePlayer()) _gameOver = true;
            }
            CalculateAmountOfPawns();
            CheckGameOver();
            if (_gameOver)
            {
                while (!CheckIfAllFieldsAreOccupied())
                {
                    SetPawnOfWinnerOnFreeField();
                }
            }
        }

        public int GetNumberOfPlayers()
        {
            return _numberOfPlayers;
        }

        public int GetActivePlayer()
        {
            return _activePlayer;
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
            int otherPlayers = _numberOfPlayers - 1;

            for (int i = 0; i < _numberOfPlayers; i++)
            {
                SetNextPlayer();
                IEnumerable<IField> fieldsWithavailableMovesOfPlayer = _utils.GetAllFieldsWithAvailableMoves(_activePlayer);

                if (fieldsWithavailableMovesOfPlayer.Count() != 0) break;

                playersThatCannotMove++;
            }

            return playersThatCannotMove != otherPlayers;
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

            int owner = start.Pawn.Owner;
            bool jump = false;

            _board.PlacePawnAt(target.Position, owner);

            int distanceBetween = _utils.CheckDistanceBetween(start, target);
            if (CheckIfStartPawnMustBeDeleted(distanceBetween))
            {
                jump = true;
                _board.RemovePawn(start.Position);
            }

            var capturedFields = ChangeOwnerOfNeighboringPawns(target);

            var move = new Move(owner, jump, start, target, capturedFields);
            OnMoveMade.Invoke(move);

            return true;
        }

        List<IField> ChangeOwnerOfNeighboringPawns(IField target)
        {
            List<IField> fieldsWithEnemyPawns = _utils.FindEnemiesPawnsInNeighborhood(target, _activePlayer);

            foreach (var f in fieldsWithEnemyPawns)
            {
                f.Pawn.SetOwner(target.Pawn.Owner);
            }

            return fieldsWithEnemyPawns;
        }

        bool CheckIfStartPawnMustBeDeleted(int distanceBetween)
        {
            if (distanceBetween > DistanceInWhichPawnIsNotDeleted) return true;
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

        int CalculateAmountOfPawns()
        {
            int numberOfPawns = 0;

            for (int i = 0; i < _numberOfPlayers; i++)
                _pawnsOfPlayer[i] = 0;

            foreach (var field in _board.GetAllFields())
            {
                if (field.Pawn == null) continue;
                _pawnsOfPlayer[field.Pawn.Owner]++;
                numberOfPawns++;
            }

            return numberOfPawns;
        }

        int GetFieldsNumberInGame()
        {
            var fieldsInGame = from b in _board.GetAllFields()
                               where !b.IsObstacle
                               select b;
            return fieldsInGame.Count();
        }

        public bool CheckIfAllFieldsAreOccupied()
        {
            int allFieldsInGame = GetFieldsNumberInGame();
            int numberOfPawn = CalculateAmountOfPawns();

            return allFieldsInGame == numberOfPawn;
        }

        bool CheckIfIsOnlyOnePawnTypeOnBoard()
        {
            int playersNumberWithAtLeastOnePawn = 0;
            foreach (var item in _pawnsOfPlayer)
            {
                if (item > 0) playersNumberWithAtLeastOnePawn++;
            }

            return playersNumberWithAtLeastOnePawn == 1;
        }

        private bool CheckGameOver()
        {
            if (_gameOver)
            {
                Debug.Log("Game Over");
                return true;
            }

            bool areAllFieldsOccupied = CheckIfAllFieldsAreOccupied();
            bool isOnlyOneTypeOfPawnsOnBoard = CheckIfIsOnlyOnePawnTypeOnBoard();

            if (areAllFieldsOccupied || isOnlyOneTypeOfPawnsOnBoard) _gameOver = true;
            if (_gameOver) Debug.Log("Game Over");

            return _gameOver;
        }

        IField FindEmptyField()
        {
            List<IField> fields = _board.GetAllFields();

            foreach (var field in fields)
            {
                if (field.IsEmpty()) return field;
            }
            return null;
        }

        public void SetPawnOnFreeField()
        {
            IField emptyField = FindEmptyField();
            if (emptyField == null) return;
            Vector2Int position = emptyField.Position;
            _board.PlacePawnAt(position, _activePlayer);
        }

        public void SetPawnOfWinnerOnFreeField()
        {
            IField emptyField = FindEmptyField();
            if (emptyField == null) return;
            Vector2Int position = emptyField.Position;
            _board.PlacePawnAt(position, _winnerIndex);
        }
    }
}
