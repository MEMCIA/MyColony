using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldView : MonoBehaviour
{
    [System.NonSerialized]
    public IBoard BoardModel;
    [System.NonSerialized]
    public Vector2Int Position;

    public GameObject Pawn;

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
        Pawn.SetActive(_fieldModel.Pawn != null);
    }

    public IField GetField()
    {
        return _fieldModel;
    }
}
