using Assets.Scripts.Game;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardAnimator : MonoBehaviour
{
    public float MoveSpeed = 0.2f;
    public float DelayAfterMove = 0.2f;

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

        _view.RefreshAllFields();
        IsAnimating = false;
    }

    IEnumerator AnimateMove(Move move)
    {
        yield return AnimatePawnMove(move, MoveSpeed);

        if (move.CapturedFields.Count > 0)
            yield return new WaitForSeconds(0.1f);

        foreach (var captured in move.CapturedFields)
        {
            var capturedField = _view.FieldViewForPosition(captured.Position);
            capturedField.ShowPawn(move.Owner);
        }

        yield return new WaitForSeconds(DelayAfterMove);
    }

    IEnumerator AnimatePawnMove(Move move, float duration)
    {
        var start = _view.FieldViewForPosition(move.Start.Position);
        var target = _view.FieldViewForPosition(move.Target.Position);

        if (move.Jump)
            start.HidePawn();

        target.ShowPawn(move.Owner);

        var pawn = target.Pawn.gameObject;
        pawn.transform.position = start.PawnPosition.position;

        yield return pawn.transform
            .DOMove(target.PawnPosition.position, duration)
            .WaitForCompletion();
    }

}
