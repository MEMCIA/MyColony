

using System.Collections.Generic;
using UnityEngine;


interface IField
{
    // position of this field
    public Vector2Int Position { get; }

    // 0 if none, 1 if we have a player 1 pawn here
    public int PlayerPawn { get; }

    // is field emtpy, and can be moved into?
    public bool IsEmpty();
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

