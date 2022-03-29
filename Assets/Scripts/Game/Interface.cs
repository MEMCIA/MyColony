using System.Collections.Generic;
using UnityEngine;


public interface IPawn
{
    public int Owner { get; }
    public void SetOwner(int owner);
}


public interface IField
{
    // position of this field
    public Vector2Int Position { get; }

    // Pawn at this place, if any
    public IPawn Pawn { get; set; }

    // is field emtpy, and can be moved into?
    public bool IsEmpty();
}


public interface IBoard
{
    // width & height
    public Vector2Int Dimensions { get; }

    public IField GetField(Vector2Int position);
    public List<IField> GetAllFields();

    public List<IField> GetAvailableMovesFor(Vector2Int position);

    // try to place pawn at spe
    public bool PlacePawnAt(Vector2Int position, int owner);
}

