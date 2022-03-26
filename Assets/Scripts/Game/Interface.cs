

using System.Collections.Generic;
using UnityEngine;


interface IField
{
    // 0 if none, 1 if we have a player 1 pawn here
    public int PlayerPawn { get; }

    // is it potentially possible to place pawn here?
    public bool IsPassable { get; }
}

interface IMove
{
    public Vector2Int TargetField { get; }
}

interface IBoard
{
    // width & height
    public Vector2Int Dimensions { get; }

    public IField GetField(int x, int y);
    public List<IField> GetAllFields();

    public List<IMove> GetAvailableMovesFor(int x, int y);
}

