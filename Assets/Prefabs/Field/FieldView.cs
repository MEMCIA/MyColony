using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldView : MonoBehaviour
{
    [System.NonSerialized]
    public IBoard Board;
    [System.NonSerialized]
    public Vector2Int Position;

    public GameObject Pawn;

    IField _field;

    private void Start()
    {
        if (Board != null)
            _field = Board.GetField(Position);
        Refresh();
    }

    public void Refresh()
    {
        if (_field == null)
            return;
        Pawn.SetActive(_field.Pawn != null);
    }

    public IField GetField()
    {
        return _field;
    }
}
