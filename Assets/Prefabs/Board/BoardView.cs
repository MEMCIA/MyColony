using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// this code will be also run in the editor, so fields will be created for preview
[ExecuteAlways]
public class BoardView : MonoBehaviour
{
    // prefab used for each field
    public FieldView Field;
    public Vector2Int Dimensions = new Vector2Int(3, 3);
    public float FieldSize = 1.0f;
    public float DistanceBetweenFields = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        CreateFields();
    }

    void Update()
    {
        if (!Application.isPlaying)
            CreateFields();
    }

    public void CreateFields()
    {
        // destroy previous children
        DestroyChildren();
        if (!Field) 
            return;

        for(int x = 0; x < Dimensions.x; x ++)
            for(int y = 0; y < Dimensions.y; y ++)
            {
                CreateField(new Vector2Int(x, y));
            }
    }

    FieldView CreateField(Vector2Int position)
    {
        var field = Instantiate(Field, transform);
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

        float x_start = -localWidth / 2.0f;
        float y_start = -localHeight / 2.0f;

        float x = x_start + (FieldSize + DistanceBetweenFields) * position.x;
        float y = y_start + (FieldSize + DistanceBetweenFields) * position.y;

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
