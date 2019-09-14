using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MrHat.Utility;
using Lean.Touch;
using System;
using DG.Tweening;

public class Cup : MonoBehaviour
{
    public static event Action<Cup, Cup> OnCupsSwapCompleted = delegate { };

    public (int, int) Index { get; set; } = (-1, -1);
    public CupColor Color { get; set; }

    private LeanSelectable _thisLeanSelectable;
    private LeanSelectable ThisLeanSelectable
        => _thisLeanSelectable ? _thisLeanSelectable : (_thisLeanSelectable = GetComponent<LeanSelectable>());

    private MeshRenderer _meshRenderer;
    private MeshRenderer MeshRenderer => _meshRenderer ? _meshRenderer : (_meshRenderer = GetComponentInChildren<MeshRenderer>());

    private bool _selected = false;
    private LeanFinger _finger;
    private Vector2 _fingerStartPosition = Vector2.zero;

    private void OnEnable()
    {
        LeanSelectable.OnSelectGlobal += OnSelect;
        LeanSelectable.OnDeselectGlobal += OnDeselect;
    }

    private void OnDisable()
    {
        LeanSelectable.OnSelectGlobal -= OnSelect;
        LeanSelectable.OnDeselectGlobal -= OnDeselect;
    }

    private void OnSelect(LeanSelectable leanSelectable, LeanFinger leanFinger)
    {
        if (leanSelectable != ThisLeanSelectable) return;

        _selected = true;
        _finger = leanFinger;
        _fingerStartPosition = _finger.ScreenPosition;
    }

    private void OnDeselect(LeanSelectable leanSelectable)
    {
        _selected = false;
    }

    private void Update()
    {
        if (!_selected) return;

        var delta = _finger.ScreenPosition - _fingerStartPosition;

        if (delta.magnitude < 50) return;

        Swap(Utility.GetSide(delta));
        ThisLeanSelectable.Deselect();
    }

    public void SetColor(CupColor color)
    {
        Color = color;
        MeshRenderer.material.SetColor("_Color", Utility.GetColor(color));
    }

    public void Swap(Side direction)
    {
        var shelfIndex = Index.Item1;
        var cupIndex = Index.Item2;
        switch (direction)
        {
            case Side.Left:
                cupIndex = (Index.Item2 - 1) % ShelfContainer.CupsMap.GetLength(1);
                break;
            case Side.Right:
                cupIndex = (Index.Item2 + 1) % ShelfContainer.CupsMap.GetLength(1);
                break;
            case Side.Up:
                shelfIndex = Mathf.Clamp(Index.Item1 + 1, 0, ShelfContainer.CupsMap.GetLength(0) - 1);
                break;
            case Side.Down:
                shelfIndex = Mathf.Clamp(Index.Item1 - 1, 0, ShelfContainer.CupsMap.GetLength(0) - 1);
                break;
        }
        var swapWith = ShelfContainer.CupsMap[shelfIndex, cupIndex];
        if (swapWith == this) return;

        var thisPos = transform.position;
        var swapWithPos = swapWith.transform.position;

        var moveSequence = DOTween.Sequence()
            .Append(transform.DOMove(swapWithPos, 0.3f)
                .OnStart(() =>
                {
                    swapWith.transform.DOMove(thisPos, 0.3f);
                }))
            .OnComplete(() =>
            {
                OnCupsSwapCompleted(this, swapWith);
            });

        ShelfContainer.CupsMap[Index.Item1, Index.Item2] = swapWith;
        ShelfContainer.CupsMap[shelfIndex, cupIndex] = this;

        var temp = Index;
        Index = swapWith.Index;
        swapWith.Index = temp;
    }
}