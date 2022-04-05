
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Game
{
    interface IPlayer
    {
        public bool IsHuman();
        public void OnTurnStart(Game game);
    }

    class HumanPlayer : IPlayer
    {
        public bool IsHuman()
        {
            return true;
        }

        public void OnTurnStart(Game game)
        {

        }
    }

    class AIPlayer : IPlayer
    {
        public bool IsHuman()
        {
            return false;
        }

        public void OnTurnStart(Game game)
        {
            IField start = FindRandomPawnOfPlayer(game);
            IField target = FindRandomMoveForPawn(start, game);
            game.Turn(start, target);
        }

        IField FindRandomPawnOfPlayer(Game game)
        {
            List<IField> pawnsOfPlayer = game.Utils().GetAllFieldsWithAvailableMoves(game.GetActivePlayer()).ToList();
            int randomIndex1 = Random.Range(0, pawnsOfPlayer.Count());
            return pawnsOfPlayer[randomIndex1];
        }

        IField FindRandomMoveForPawn(IField pawn, Game game)
        {
            List<IField> movesForRandomPawn = game.Utils().GetAvailableMovesFor(pawn.Position);
            int randomIndex = Random.Range(0, movesForRandomPawn.Count());
            return movesForRandomPawn[randomIndex];
        }
    }

    class Players
    {
        public Players(Game game)
        {
            _game = game;
        }

        static public Players CreateHumans(Game game)
        {
            int numerOfPlayers = game.GetNumberOfPlayers();
            var players = new Players(game);

            for (int i = 0; i < numerOfPlayers; i++)
                players._players.Add(new HumanPlayer());
            return players;
        }

        static public Players CreateHumanAndAIs(Game game)
        {
            int numerOfPlayers = game.GetNumberOfPlayers();
            var players = new Players(game);

            for (int i = 0; i < numerOfPlayers; i++)
            {
                if (i == 0)
                    players._players.Add(new HumanPlayer());
                else
                    players._players.Add(new AIPlayer());
            }

            return players;
        }

        public void OnTurnStart()
        {
            while (true)
            {
                if (_game.IsGameOver()) return;

                var currentPlayer = GetActivePlayer();
                if (currentPlayer.IsHuman()) return;
                currentPlayer.OnTurnStart(_game);
            }
        }

        public bool IsCurrentPlayerHuman()
        {
            return GetActivePlayer()?.IsHuman() ?? false;
        }

        IPlayer GetActivePlayer()
        {
            return _players[_game.GetActivePlayer()];
        }

        IPlayer GetPlayer(int id)
        {
            return _players[id];
        }

        List<IPlayer> _players = new List<IPlayer>();
        Game _game;
    }

}
