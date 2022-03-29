using Assets.Scripts.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BoardEditor : MonoBehaviour
{
    IBoard _board;
    BoardView _view;
    [SerializeField] private Button btn;

    void Awake()
    {
        // TODO use sample board here
        _board = new Board(new Vector2Int(3, 3));


        _view = GetComponent<BoardView>();
        _view.BoardModel = _board;
        _view.OnFieldClicked.AddListener(OnClicked);

        btn.onClick.AddListener(OnClickButton);
    }

    void Update()
    {

    }

    void OnClicked(IField field)
    {
        _board.PlacePawnAt(field.Position, 1);
        _view.RefreshField(field.Position);
    }

    private void OnClickButton()
    {
        string json = SaveBoard.ToJSON(SaveBoard.CreateFromBoard(_board));
        Debug.Log(json);
    }
}
