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
        // TODO
        // return Board.FromJson(JSON);
        SaveBoard sv = SaveBoard.FromJSON(JSON);
        return Board.CreateFromSaveBoard(sv);
    }

    static public void Save(Board b)
    {
#if UNITY_EDITOR
        // if we are in editor, let's create an BoardObject in 
        var boardData = CreateInstance<BoardObject>();
        boardData.Name = "Default";
        // TODO
        // boardData.JSON = b.ToJson();
        SaveBoard sv = SaveBoard.CreateFromBoard(b);
        boardData.JSON = SaveBoard.ToJSON(sv);

        var date = DateTime.Now.ToString("yyyy-MM-dd--HH-mm-ss");
        var path = $"Assets/Levels/{date}.asset";
        AssetDatabase.CreateAsset(boardData, path);
#endif
    }
}
