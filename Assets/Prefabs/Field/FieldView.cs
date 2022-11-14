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

    public GameVisualSettings Settings;
    public Color PawnSelectedColor = Color.white;
    public Material FieldHighlightedMaterial;
    public Material FieldTargetableMaterial;

    protected static int PROPERTY_HIGHLIGHTED = Shader.PropertyToID("_Highlighted");

    Material _standardFieldMaterial;

    bool _fieldHighlighted;
    bool _fieldTargetable;
    IField _fieldModel;

    private void Start()
    {
        _standardFieldMaterial = Field.sharedMaterial;
        if (BoardModel != null)
            _fieldModel = BoardModel.GetField(Position);
        Refresh();
    }

    public void HidePawn()
    {
        Pawn.gameObject.SetActive(false);
    }

    public void ShowPawn(int playerIndex)
    {
        Pawn.gameObject.SetActive(true);
        Pawn.material.color = Settings.ColorOfPlayer(playerIndex);
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
            Pawn.material.color = Settings.ColorOfPlayer(pawn.Owner);
        }
    }

    public IField GetField()
    {
        return _fieldModel;
    }

    public void SetFieldHighlighted(bool selected)
    {
        _fieldHighlighted = selected;
        UpdateFieldMaterial();

    }
    public void SetTargetable(bool targetable)
    {
        _fieldTargetable = targetable;
        UpdateFieldMaterial();
    }

    public void SetPawnHighlighted(bool selected)
    {
        Pawn.material.SetFloat(PROPERTY_HIGHLIGHTED, selected ? 1 : 0);
    }

    void UpdateFieldMaterial()
    {
        if (_fieldHighlighted)
            Field.sharedMaterial = FieldHighlightedMaterial;
        else if (_fieldTargetable)
            Field.sharedMaterial = FieldTargetableMaterial;
        else
            Field.sharedMaterial = _standardFieldMaterial;
    }
}
