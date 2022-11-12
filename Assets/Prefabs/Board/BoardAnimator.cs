using Assets.Scripts.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardAnimator : MonoBehaviour
{
    public bool IsAnimating { get; private set; }
    BoardView _view;
    Coroutine _currentAnimation;

    void Awake()
    {
        _view = GetComponent<BoardView>();
    }

    public void AnimateMoves(List<Move> moves)
    {
        if (IsAnimating)
        {
            Debug.LogWarning("BoardAnimator still is animating previous moves!");
            if (_currentAnimation != null)
                StopCoroutine(_currentAnimation);

            _view.RefreshAllFields();
            return;
        }

        IsAnimating = true;
        _currentAnimation = StartCoroutine(AsyncAnimateMoves(new List<Move>(moves)));
    }

    IEnumerator AsyncAnimateMoves(List<Move> moves)
    {
        foreach (Move move in moves)
        {
            yield return AnimateMove(move);
        }
        IsAnimating = false;
    }

    IEnumerator AnimateMove(Move move)
    {
        var start = _view.FieldViewForPosition(move.Start.Position);
        var target = _view.FieldViewForPosition(move.Target.Position);

        start.Refresh();
        target.Refresh();

        if (move.CapturedFields.Count > 0)
            yield return new WaitForSeconds(0.1f);

        foreach (var captured in move.CapturedFields)
        {
            var capturedField = _view.FieldViewForPosition(captured.Position);
            capturedField.Refresh();
        }

        yield return new WaitForSeconds(0.5f);
    }

}
