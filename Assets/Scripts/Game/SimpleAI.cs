using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Game
{
    class SimpleAI : AIPlayer
    {
        public override void OnTurnStart(Game game)
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
}
