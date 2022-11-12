using Assets.Scripts.Game;
using System.Collections.Generic;
using UnityEngine;

public class BoardGame : MonoBehaviour
{
    public enum Mode
    {
        EasyAI,
        StandardAI,
        Humans
    }

    public static Board StartingBoard;
    public static Mode CurrentMode { get; internal set; }
    public static BoardGame CurrentGame;

    public Vector2Int Dimensions = new Vector2Int(3, 3);

    Game _game;
    Board _board;
    BoardView _view;
    BoardAnimator _animator;
    Players _players;

    IField _selectedPawnField;
    List<Move> _pendingMoves = new List<Move>();

    void Awake()
    {
        CurrentGame = this;

        _animator = GetComponent<BoardAnimator>();
        _view = GetComponent<BoardView>();
        _view.OnFieldClicked.AddListener(OnFieldClicked);
        _view.OnPawnClicked.AddListener(OnPawnClicked);


        // only highlight pawns of current player
        _view.PawnSelectionFilter = (IField field) =>
        {
            if (!IsCurrentPlayerHuman()) return false;
            return field.Pawn.Owner == _game.GetActivePlayer();
        };

        // only highlight fields if pawn is selected, and field is a valid move
        _view.FieldSelectionFilter = IsFieldValidMoveTarget;

        LoadStartingBoard();
    }

    bool IsFieldValidMoveTarget(IField field)
    {
        if (!IsCurrentPlayerHuman()) return false;
        if (_selectedPawnField == null)
            return false;
        return _game.IsValidMove(_selectedPawnField, field);
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
        _game.OnMoveMade.AddListener(move => _pendingMoves.Add(move));

        CreatePlayers();

        _view.SetBoard(board);
        _players.OnTurnStart();
        _view.RefreshAllFields();
    }

    void CreatePlayers()
    {
        switch (CurrentMode)
        {
            case Mode.EasyAI:
                _players = Players.CreateHumanAndAIs(_game, new SimpleAI());
                break;
            case Mode.StandardAI:
                _players = Players.CreateHumanAndAIs(_game, new GoodAI());
                break;
            case Mode.Humans:
                _players = Players.CreateHumans(_game);
                break;
        }
    }

    public Board GetBoard()
    {
        return _board;
    }

    void OnFieldClicked(IField field)
    {
        if (_animator.IsAnimating)
            return;

        Debug.Log($"Clicked on field {field.Position}");
        if (_selectedPawnField != null)
        {
            MakeAMove(_selectedPawnField, field);
            SetSelectedPawnField(null);
        }
    }

    void OnPawnClicked(IField field)
    {
        if (_animator.IsAnimating)
            return;

        Debug.Log($"Clicked on pawn {field.Position}");
        SetSelectedPawnField(field);
    }

    void MakeAMove(IField start, IField target)
    {
        if (_animator.IsAnimating)
            return;

        // this will cause game to send OnMoveMade events, which will cause Move to be added to  _pendingMoves
        _game.Turn(start, target);
        // this will change turn to next player, if this player is an AI it will immediately make it's move
        _players.OnTurnStart();

        // animate all collected human & AI moves moves
        _animator.AnimateMoves(_pendingMoves);
        _pendingMoves.Clear();
    }

    void SetSelectedPawnField(IField field)
    {
        _selectedPawnField = field;
        _view.UpdateTargetableFields();
    }

    bool IsCurrentPlayerHuman()
    {
        if (_players == null) return false;
        if (!_players.IsCurrentPlayerHuman()) return false;
        return true;
    }

}
