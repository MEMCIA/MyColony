using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


// this code will be also run in the editor, so fields will be created for preview
[ExecuteAlways]
public class BoardView : MonoBehaviour
{
    [System.NonSerialized]
    public IBoard BoardModel;

    // prefab used for each field
    public FieldView Field;
    public Vector2Int Dimensions = new Vector2Int(3, 3);
    public float FieldSize = 1.0f;
    public float DistanceBetweenFields = 0.1f;
    public LayerMask FieldsLayers = new LayerMask();
    public UnityEvent<IField> OnFieldClicked = new UnityEvent<IField>();

    List<FieldView> _fields;

    // Start is called before the first frame update
    void Start()
    {
        if (BoardModel != null)
            Dimensions = BoardModel.Dimensions;
        CreateFields();
    }

    void Update()
    {
        if (!Application.isPlaying)
        {
            CreateFields();
            return;
        }

        if (Input.GetMouseButtonUp(0))
        {
            CheckIfClicked();
        }
    }

    void CheckIfClicked()
    {
        RaycastHit hit;
        var camera = Camera.main;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, FieldsLayers))
        {
            var objectHit = hit.collider.gameObject;
            var field = objectHit.GetComponent<FieldView>();

            if (!field)
                return;

            Debug.Log($"Clicked on field {field.Position}");
            OnFieldClicked.Invoke(field.GetField());
        }
    }

    public void RefreshField(Vector2Int position)
    {
        var field = FieldViewForPosition(position);
        field.Refresh();
    }

    FieldView FieldViewForPosition(Vector2Int position)
    {
        return _fields[position.x + position.y * Dimensions.x];
    }

    public void CreateFields()
    {
        // destroy previous children
        DestroyChildren();
        if (!Field) 
            return;

        _fields = new List<FieldView>();
        for (int x = 0; x < Dimensions.x; x ++)
            for(int y = 0; y < Dimensions.y; y ++)
            {
                var newField = CreateField(new Vector2Int(x, y));
                _fields.Add(newField);
            }
    }

    FieldView CreateField(Vector2Int position)
    {
        var field = Instantiate(Field, transform);
        field.BoardModel = BoardModel;
        field.Position = position;
        field.transform.localPosition = FieldPositionToLocalPosition(position);
        // this object will be not saved as part of the scene
        field.hideFlags = HideFlags.DontSaveInEditor;
        return field;
    }

    Vector3 FieldPositionToLocalPosition(Vector2Int position)
    {
        // point 0,0 should be in center of our field, so let's calculate board size in local space
        // by summing dimensions of fields and space between them
        float localWidth = FieldSize * Dimensions.x + DistanceBetweenFields * (Dimensions.x - 1);
        float localHeight = FieldSize * Dimensions.y + DistanceBetweenFields * (Dimensions.y - 1);

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
