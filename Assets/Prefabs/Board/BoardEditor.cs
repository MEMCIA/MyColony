using Assets.Scripts.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class BoardEditor : MonoBehaviour
{
    public static BoardEditor CurrentEditor;

    public Vector2Int Dimensions = new Vector2Int(3,3);

    List<KeyCode> keyCodesNumbers = new List<KeyCode> { KeyCode.Keypad0, KeyCode.Keypad1, KeyCode.Keypad2, KeyCode.Keypad3, KeyCode.Keypad4, KeyCode.Keypad5, KeyCode.Keypad6, KeyCode.Keypad7, KeyCode.Keypad8, KeyCode.Keypad9 }; 

    Board _board;
    BoardView _view;
    int _currentPawnOwner = 1;

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
        ChangeCurrentPawnOwner();
        _board.PlacePawnAt(field.Position, _currentPawnOwner);
        field.Pawn.SetOwner(_currentPawnOwner);
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
