using System.Collections.Generic;
using UnityEngine;

public class FieldView : MonoBehaviour
{
    [System.NonSerialized]
    public IBoard BoardModel;
    [System.NonSerialized]
    public Vector2Int Position;

    public Transform PawnPosition;
    public MeshRenderer Field;
    public MeshRenderer Pawn;

    public List<Color> PawnColors;
    public Color PawnSelectedColor = Color.white;
    public Material FieldSelectedMaterial;

    protected static int PROPERTY_SELECTED = Shader.PropertyToID("_Selected");

    Material _standardFieldMaterial;

    bool _fieldSelected;
    IField _fieldModel;

    private void Start()
    {
        _standardFieldMaterial = Field.sharedMaterial;
        if (BoardModel != null)
            _fieldModel = BoardModel.GetField(Position);
        Refresh();
    }

    public Color ColorForPlayer(int playerIndex)
    {
        return PawnColors[playerIndex % PawnColors.Count];
    }

    public void HidePawn()
    {
        Pawn.gameObject.SetActive(false);
    }

    public void ShowPawn(int playerIndex)
    {
        Pawn.gameObject.SetActive(true);
        Pawn.material.color = ColorForPlayer(playerIndex);
    }

    public Material GetPawnMaterial()
    {
        return Pawn.material;
    }

    public void Refresh()
    {
        if (_fieldModel == null)
            return;

        var pawn = _fieldModel.Pawn;
        Pawn.gameObject.SetActive(pawn != null);
        if (pawn != null)
        {
            Pawn.material.color = ColorForPlayer(pawn.Owner);
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
        Pawn.material.SetFloat(PROPERTY_SELECTED, selected ? 1 : 0);
    }
}
