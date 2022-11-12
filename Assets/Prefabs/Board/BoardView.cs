using Assets.Scripts.Game;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class BoardView : MonoBehaviour
{
    // prefab used for each field
    public FieldView Field;
    public float FieldSize = 1.0f;
    public float DistanceBetweenFields = 0.1f;
    public LayerMask FieldsLayers = new LayerMask();
    public LayerMask PawnsLayers = new LayerMask();

    public UnityEvent<IField> OnFieldClicked = new UnityEvent<IField>();
    public UnityEvent<IField> OnPawnClicked = new UnityEvent<IField>();

    public UnityEvent<IField> OnFieldHover = new UnityEvent<IField>();
    public UnityEvent<IField> OnPawnHover = new UnityEvent<IField>();

    FieldView _currentField;
    FieldView _currentPawn;

    public Func<IField, bool> PawnSelectionFilter;
    public Func<IField, bool> FieldSelectionFilter;

    List<FieldView> _fields;
    protected IBoard _boardModel;
    Vector2Int _dimensions = new Vector2Int(3, 3);

    // Start is called before the first frame update
    void Start()
    {
        CreateFields();
    }

    public void SetBoard(Board board)
    {
        _boardModel = board;
        _dimensions = _boardModel.Dimensions;
        CreateFields();
    }

    void Update()
    {
        if (!Application.isPlaying)
        {
            CreateFields();
            return;
        }

        MouseHoverCheckField();
        MouseHoverCheckPawn();

        if (Input.GetMouseButtonUp(0))
        {
            MouseClickCheck();
        }
    }

    void MouseHoverCheckField()
    {
        FieldView field = CheckMouseRaycast(FieldsLayers);
        IField fieldModel = field ? field.GetField() : null;

        // check if field should be highlighted by calling FieldSelectionFilter
        if (FieldSelectionFilter != null && fieldModel != null)
            if (!FieldSelectionFilter(fieldModel))
                field = null;


        if (_currentField != field)
        {
            if (_currentField)
                _currentField.SetFieldHighlighted(false);
            _currentField = field;
            if (_currentField)
                _currentField.SetFieldHighlighted(true);

            OnFieldHover.Invoke(fieldModel);
        }
    }

    void MouseHoverCheckPawn()
    {
        FieldView field = CheckMouseRaycast(PawnsLayers);
        IField fieldModel = field ? field.GetField() : null;

        // check if pawn should be highlighted by calling FieldSelectionFilter
        if (PawnSelectionFilter != null && fieldModel != null)
            if (!PawnSelectionFilter(fieldModel))
                field = null;

        if (_currentPawn != field)
        {
            if (_currentPawn)
                _currentPawn.SetPawnHighlighted(false);
            _currentPawn = field;
            if (_currentPawn)
                _currentPawn.SetPawnHighlighted(true);

            OnPawnHover.Invoke(_currentPawn ? _currentPawn.GetField() : null);
        }
    }

    void MouseClickCheck()
    {
        if (_currentField)
            OnFieldClicked.Invoke(_currentField.GetField());

        if (_currentPawn)
            OnPawnClicked.Invoke(_currentPawn.GetField());
    }

    FieldView CheckMouseRaycast(int layerMask)
    {
        RaycastHit hit;
        var camera = Camera.main;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            var objectHit = hit.collider.gameObject;
            var field = objectHit.GetComponentInParent<FieldView>();

            if (field)
                return field;
        }
        return null;
    }

    internal void UpdateTargetableFields()
    {
        foreach (var field in _fields)
        {
            bool targetable = true;
            if (FieldSelectionFilter != null && !FieldSelectionFilter(field.GetField()))
                targetable = false;
            field.SetTargetable(targetable);
        }
    }

    public void RefreshField(Vector2Int position)
    {
        var field = FieldViewForPosition(position);
        field.Refresh();
    }

    public void RefreshAllFields()
    {
        foreach (var field in _fields)
            field.Refresh();
    }

    public FieldView FieldViewForPosition(Vector2Int position)
    {
        return _fields[position.x + position.y * _dimensions.x];
    }

    public void CreateFields()
    {
        // destroy previous children
        DestroyChildren();
        if (!Field)
            return;

        _fields = new List<FieldView>();
        for (int y = 0; y < _dimensions.y; y++)
            for (int x = 0; x < _dimensions.x; x++)
            {
                var newField = CreateField(new Vector2Int(x, y));
                _fields.Add(newField);
            }
    }

    FieldView CreateField(Vector2Int position)
    {
        var field = Instantiate(Field, transform);
        field.BoardModel = _boardModel;
        field.Position = position;
        field.transform.localPosition = FieldPositionToLocalPosition(position);
        return field;
    }

    Vector3 FieldPositionToLocalPosition(Vector2Int position)
    {
        // point 0,0 should be in center of our field, so let's calculate board size in local space
        // by summing dimensions of fields and space between them
        float localWidth = FieldSize * _dimensions.x + DistanceBetweenFields * (_dimensions.x - 1);
        float localHeight = FieldSize * _dimensions.y + DistanceBetweenFields * (_dimensions.y - 1);

        // board should be centerd on (0,0), so lets shift start position by half of board width
        float x_start = -localWidth / 2.0f;
        float y_start = -localHeight / 2.0f;

        // calculate position of each field
        float x = x_start + (FieldSize + DistanceBetweenFields) * position.x;
        float y = y_start + (FieldSize + DistanceBetweenFields) * position.y;

        // add half of field size - now we calculated center of this board field/tile
        x += FieldSize / 2f;
        y += FieldSize / 2f;

        // use x,z coordinates for fields
        return new Vector3(x, 0, y);
    }

    void DestroyChildren()
    {
        // destroy cannot be called from edit mode, we need to call DestroyImmediate
        if (!Application.isPlaying)
        {
            while (transform.childCount > 0)
            {
                var child = transform.GetChild(0);
                DestroyImmediate(child.gameObject);
            }
            return;
        }

        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}
