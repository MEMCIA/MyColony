using Assets.Scripts.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class BoardEditor : MonoBehaviour
{
    public static BoardEditor CurrentEditor;

    public Vector2Int Dimensions = new Vector2Int(3,3);

    Board _board;
    BoardView _view;

    void Awake()
    {
        CurrentEditor = this;

        _view = GetComponent<BoardView>();
        _view.OnFieldClicked.AddListener(OnClicked);

        // TODO use sample board here
        LoadBoard(new Board(new Vector2Int(3, 3)));
    }

    void Update()
    {

    }

    public void NewBoard()
    {
        var board = new Board(Dimensions);
        LoadBoard(board);
    }

    public void LoadBoard(Board board)
    {
        _board = board;
        _view.SetBoard(board);
    }

    public Board GetBoard()
    {
        return _board;
    }

    void OnClicked(IField field)
    {
        _board.PlacePawnAt(field.Position, 1);
        _view.RefreshField(field.Position);
    }
}
