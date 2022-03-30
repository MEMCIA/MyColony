using Assets.Scripts.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BoardEditor : MonoBehaviour
{
    public static BoardEditor CurrentEditor;

    public Vector2Int Dimensions = new Vector2Int(3, 3);

    List<KeyCode> keyCodesNumbers = new List<KeyCode> { KeyCode.Alpha0, KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9 };

    Board _board;
    BoardView _view;
    int _currentPawnOwner = 0;

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
        ChangeCurrentPawnOwner();
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
        if(!_board.RemovePawn(field.Position))
        {
            _board.PlacePawnAt(field.Position, _currentPawnOwner);
        }
        _view.RefreshField(field.Position);
    }

    void ChangeCurrentPawnOwner()
    {
        for (int i = 0; i < keyCodesNumbers.Count; i++)
        {
            if (Input.GetKey(keyCodesNumbers[i])) _currentPawnOwner = i;
        }
    }
}
