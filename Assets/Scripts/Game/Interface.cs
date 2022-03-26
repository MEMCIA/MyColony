

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


interface IBoard
{
    // width & height
    public Vector2Int Dimensions { get; }

    public IField GetField(Vector2Int position);
    public List<IField> GetAllFields();

    public List<IField> GetAvailableMovesFor(Vector2Int position);

    // try to place pawn at spe
    public bool PlacePawnAt(Vector2Int position, int player);
}

