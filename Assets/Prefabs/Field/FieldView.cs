using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldView : MonoBehaviour
{
    [System.NonSerialized]
    public IBoard BoardModel;
    [System.NonSerialized]
    public Vector2Int Position;

    public MeshRenderer Pawn;
    public List<Color> PawnColors;


    IField _fieldModel;

    private void Start()
    {
        if (BoardModel != null)
            _fieldModel = BoardModel.GetField(Position);
        Refresh();
    }

    public void Refresh()
    {
        if (_fieldModel == null)
            return;

        var pawn = _fieldModel.Pawn;
        Pawn.gameObject.SetActive(pawn != null);
        if (pawn != null)
        {
            // set color for pawn
            Pawn.material.color = PawnColors[pawn.Owner % PawnColors.Count];
        }
    }

    public IField GetField()
    {
        return _fieldModel;
    }
}
