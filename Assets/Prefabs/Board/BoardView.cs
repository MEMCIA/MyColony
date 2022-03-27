using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


// this code will be also run in the editor, so fields will be created for preview
[ExecuteAlways]
public class BoardView : MonoBehaviour
{
    // prefab used for each field
    public FieldView Field;
    public Vector2Int Dimensions = new Vector2Int(3, 3);
    public float DistanceBetweenFields = 1.1f;
    public LayerMask FieldsLayers = new LayerMask();

    public UnityEvent<Vector2Int> OnFieldClicked = new UnityEvent<Vector2Int>();

    // Start is called before the first frame update
    void Start()
    {
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
            OnFieldClicked.Invoke(field.Position);
        }
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
        field.Position = position;
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
