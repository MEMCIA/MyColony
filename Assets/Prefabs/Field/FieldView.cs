using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldView : MonoBehaviour
{
    [System.NonSerialized]
    public IBoard BoardModel;
    [System.NonSerialized]
    public Vector2Int Position;

    public MeshRenderer Field;
    public MeshRenderer Pawn;
    
    public List<Color> PawnColors;
    public Color PawnSelectedColor = Color.white;
    public Material FieldSelectedMaterial;

    Material _standardFieldMaterial;

    bool _fieldSelected;
    bool _pawnSelected;
    IField _fieldModel;

    private void Start()
    {
        _standardFieldMaterial = Field.sharedMaterial;
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
            if (_pawnSelected)
            {
                Pawn.material.color = PawnSelectedColor;
            }
            else 
            {
                Pawn.material.color = PawnColors[pawn.Owner % PawnColors.Count];
            }
            
        }
    }

    public IField GetField()
    {
        return _fieldModel;
    }

    public void SetFieldSelected(bool selected)
    {
        _fieldSelected = selected;
        Field.sharedMaterial = _fieldSelected ? FieldSelectedMaterial : _standardFieldMaterial;
    }

    public void SetPawnSelected(bool selected)
    {
        _pawnSelected = selected;
        Refresh();
    }
}
