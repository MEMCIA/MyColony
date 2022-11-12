using Assets.Scripts.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardAnimator : MonoBehaviour
{
    BoardView _view;

    void Awake()
    {
        _view = GetComponent<BoardView>();
    }

    public void AnimateMoves(List<Move> moves)
    {
        StartCoroutine(AsyncAnimateMoves(new List<Move>(moves)));
    }

    IEnumerator AsyncAnimateMoves(List<Move> moves)
    {
        foreach (Move move in moves)
        {
            yield return AnimateMove(move);
        }
    }

    IEnumerator AnimateMove(Move move)
    {
        var start = _view.FieldViewForPosition(move.Start.Position);
        var target = _view.FieldViewForPosition(move.Target.Position);

        start.Refresh();
        target.Refresh();

        foreach (var captured in move.CapturedFields)
        {
            var capturedField = _view.FieldViewForPosition(captured.Position);
            capturedField.Refresh();
        }
        yield return new WaitForSeconds(0.5f);
    }

}
