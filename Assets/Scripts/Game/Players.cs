
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
            GoodAI2(game);
        }

        void SimpleAI(Game game)
        {
            IField start = FindRandomPawnOfPlayer(game);
            IField target = FindRandomMoveForPawn(start, game);
            game.Turn(start, target);
        }

        void GoodAI(Game game)
        {
            var moveDataListinOrder = CreateOrderedMoveDataList(game);
            MoveData bestMove = FindRandomBestMove(game, moveDataListinOrder);
            game.Turn(bestMove.Start, bestMove.Destination);
        }

        MoveData FindRandomBestMove(Game game, List<MoveData> moveDataListinOrder)
        {
            
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

        void GoodAI2(Game game)
        {
            MoveData theBestMove = FindBestMoveForGoodAI2(game);
            game.Turn(theBestMove.Start, theBestMove.Destination);
        }

        List<MoveData> CreateOrderedMoveDataList(Game game)
        {
            List<MoveData> moveDataList = CreateMoveDataList(game);
            return moveDataList.OrderByDescending(x => x.Value).ToList();
        }

        MoveData FindBestMoveForGoodAI2(Game game)
        {
            var moveDataListinOrder = CreateOrderedMoveDataList(game);
            MoveData bestMove = FindRandomBestMove(game, moveDataListinOrder);

            return ChangeMoveIfIsDangerous(game, bestMove, moveDataListinOrder);
        }

        MoveData ChangeMoveIfIsDangerous(Game game, MoveData bestMove, List<MoveData> moveDataListinOrder)
        {
            int distance = game.Utils().CheckDistanceBetween(bestMove.Start, bestMove.Destination);
            if (distance == 1) return bestMove;
            while (true)
            {
                int fieldsOfPLayerInNeighborhood = FindPawnsInNeighborhoodOfPlayer(bestMove.Start, game.GetActivePlayer(), game).Count;
                if (fieldsOfPLayerInNeighborhood > bestMove.Value) return bestMove;
                moveDataListinOrder.Remove(bestMove);
                if (moveDataListinOrder.Count == 0) return bestMove;
                bestMove = moveDataListinOrder[0];
            }
        }

        public List<IField> FindPawnsInNeighborhoodOfPlayer(IField target, int activePlayer, Game game)
        {
            List<IField> neighbors = game.Utils().FindFieldsinDistance(target.Position, 1);
            List<IField> fieldWithPawns = new List<IField>();

            foreach (var n in neighbors)
            {
                if (n == null || n.IsEmpty()) continue;
                if (n.Pawn == null) continue;
                if (n.Pawn.Owner == activePlayer) fieldWithPawns.Add(n);
            }

            return fieldWithPawns;
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
