using Assets.Scripts.Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
#if UNITY_EDITOR
using UnityEngine;
#endif

public class BoardObject : ScriptableObject
{
    public string Name;
    [TextArea(10, 100)]
    public string JSON;

    public Board Load()
    {
        // return Board.FromJson(JSON);
        return Board.CreateFromSaveBoard(JSON);
    }

    static public void Save(Board b)
    {
#if UNITY_EDITOR
        // if we are in editor, let's create an BoardObject in 
        var boardData = CreateInstance<BoardObject>();
        boardData.Name = "Default";
       
        // boardData.JSON = b.ToJson();
        boardData.JSON = SaveBoard.CreateFromBoard(b);

        var date = DateTime.Now.ToString("yyyy-MM-dd--HH-mm-ss");
        var path = $"Assets/Levels/{date}.asset";
        AssetDatabase.CreateAsset(boardData, path);
#endif
    }
}
