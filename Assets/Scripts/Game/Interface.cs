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
    public void RemovePawn();
}


public interface IBoard
{
    // width & height
    public Vector2Int Dimensions { get; }

    public IField GetField(Vector2Int position);
    public List<IField> GetAllFields();

    // try to place pawn at position 
    public bool PlacePawnAt(Vector2Int position, int owner);

    public bool RemovePawn(Vector2Int position);
}

