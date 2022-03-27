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
    public float DistanceBetweenFields = 1.1f;


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
        float x_start = (Dimensions.x - 1) / 2.0f;
        float y_start = (Dimensions.y - 1) / 2.0f;

        float x = x_start + position.x * DistanceBetweenFields;
        float y = y_start + position.y * DistanceBetweenFields;

        // use x,z coordinates for fields
        return new Vector3(x, 0, y);
    }

    void DestroyChildren()
    {
        while (transform.childCount > 0)
        {
            var child = transform.GetChild(0);
            // destroy cannot be called from edit mode, we need to call DestroyImmediate
            if (!Application.isPlaying)
                DestroyImmediate(child.gameObject);
            else
                Destroy(child.gameObject);
        }
    }


}
