using Assets.Scripts.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class BoardEditor : MonoBehaviour
{
    IBoard _board;
    BoardView _view;

    void Awake()
    {
        // TODO use sample board here
        _board = new Board(new Vector2Int(3, 3));


        _view = GetComponent<BoardView>();
        _view.Board = _board;
        _view.OnFieldClicked.AddListener(OnClicked);
    }

    void Update()
    {

    }

    void OnClicked(IField field)
    {
        _board.PlacePawnAt(field.Position, 1);
        _view.RefreshField(field.Position);
    }
}
