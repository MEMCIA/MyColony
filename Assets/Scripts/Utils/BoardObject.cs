using Assets.Scripts.Game;
using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class BoardObject : ScriptableObject
{
    public string Name;
    [TextArea(10, 100)]
    public string JSON;

    public Board Load()
    {
        return SaveBoard.CreateFromJSON(JSON);
    }

    static public void Save(Board b)
    {
#if UNITY_EDITOR
        // if we are in editor, let's create an BoardObject in 
        var boardData = CreateInstance<BoardObject>();
        boardData.Name = "Default";

        boardData.JSON = SaveBoard.BoardToJSON(b);

        var date = DateTime.Now.ToString("yyyy-MM-dd--HH-mm-ss");
        var path = $"Assets/Levels/{date}.asset";
        AssetDatabase.CreateAsset(boardData, path);
#endif
    }
}
