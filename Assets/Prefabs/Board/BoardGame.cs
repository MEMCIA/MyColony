using Assets.Scripts.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BoardGame : MonoBehaviour
{
    public static Board StartingBoard;
    public static BoardGame CurrentGame;

    public Vector2Int Dimensions = new Vector2Int(3, 3);

    Game _game;
    Board _board;
    BoardView _view;
    Players _players;

    IField _selectedPawnField;

    void Awake()
    {
        CurrentGame = this;

        _view = GetComponent<BoardView>();
        _view.OnFieldClicked.AddListener(OnFieldClicked);
        _view.OnPawnClicked.AddListener(OnPawnClicked);

        
        // only hilight pawns of current player
        _view.PawnSelectionFilter = (IField field) => 
        {
            if (!IsCurrentPlayerHuman()) return false;
            return field.Pawn.Owner == _game.GetActivePlayer(); 
        };

        // only hilight fields if pawn is selected, and field is a valid move
        _view.FieldSelectionFilter = (IField field) =>
        {
            if (!IsCurrentPlayerHuman()) return false;
            if (_selectedPawnField == null)
                return false;
            return _game.IsValidMove(_selectedPawnField, field);
        };

        LoadStartingBoard();
    }

    void LoadStartingBoard()
    {
        if (StartingBoard != null)
        {
            LoadBoard(StartingBoard);
        }
        else
            LoadBoard(new Board(new Vector2Int(3, 3)));
    }

    public void NewBoard()
    {
        var board = new Board(Dimensions);
        LoadBoard(board);
    }

    public void LoadBoard(Board board)
    {
        _board = board;
        _game = new Game(_board);
        _players = Players.CreateHumans(_game);
        _view.SetBoard(board);
        _players.OnTurnStart();
        _view.RefreshAllFields();
    }

    public Board GetBoard()
    {
        return _board;
    }

    void OnFieldClicked(IField field)
    {
        Debug.Log($"Clicked on field {field.Position}");
        if (_selectedPawnField != null)
        {
            MakeAMove(_selectedPawnField, field);
            _selectedPawnField = null;
        }
    }

    void OnPawnClicked(IField field)
    {
        Debug.Log($"Clicked on pawn {field.Position}");
        _selectedPawnField = field;
    }

    void MakeAMove(IField start, IField target)
    {
        _game.Turn(start, target);
        _players.OnTurnStart();
        _view.RefreshAllFields();
    }

    bool IsCurrentPlayerHuman()
    {
        if (_players == null) return false;
        if (!_players.IsCurrentPlayerHuman()) return false;
        return true;
    }

}
