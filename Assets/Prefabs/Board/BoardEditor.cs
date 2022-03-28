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

        btn.onClick.AddListener(OnclickButton);
    }

    void Update()
    {

    }

    void OnClicked(IField field)
    {
        _board.PlacePawnAt(field.Position, 1);
        _view.RefreshField(field.Position);
    }

    private void OnclickButton()
    {
        string json = SaveBoard.ToJSON(TransformBoardintoSaveBoard());
        Debug.Log(json);
    }

    private SaveBoard TransformBoardintoSaveBoard()
    {
        var allFieldsBoard = _board.GetAllFields();
        List<SaveField> savefields = new List<SaveField>();

        for (int i = 0; i < allFieldsBoard.Count; i++)
        {
            if(allFieldsBoard[i].Pawn==null)
            {
                savefields.Add(new SaveField(SaveField.NoPawn));
                continue;
            }
            savefields.Add(new SaveField(allFieldsBoard[i].Pawn.Owner));
        }

        return new SaveBoard(_board.Dimensions.x,_board.Dimensions.y, savefields);
    }
}
