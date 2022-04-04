using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BoardEditor))]
public class BoardEditorEditor : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        BoardEditor myTarget = (BoardEditor)target;

        if (Application.isPlaying)
        {
            if (GUILayout.Button("New Level"))
            {
                myTarget.NewBoard();
            }

            if (GUILayout.Button("Save Level"))
            {
                var board = myTarget.GetBoard();
                BoardObject.Save(board);
            }
        }

    }
}

public class LoadLevelAssets
{
    [MenuItem("Assets/Load Level")]
    public static void LoadLevel()
    {
        var selection = Selection.activeObject as BoardObject;
        var board = selection.Load();

        if (BoardEditor.CurrentEditor != null)
            BoardEditor.CurrentEditor.LoadBoard(board);
        if (BoardGame.CurrentGame != null)
            BoardGame.CurrentGame.LoadBoard(board);
    }

    [MenuItem("Assets/Load Level", true)]
    public static bool LoadLevelValidation()
    {
        return Selection.activeObject as BoardObject && Application.isPlaying;
    }
}
