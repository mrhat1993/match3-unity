using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MrHat.Utility;
using Lean.Touch;
using System;

//TODO: REWRITE CUPS MAPPING ON 2dim ARRAY!!!
public class Cup : MonoBehaviour
{
    private LeanSelectable _thisLeanSelectable;
    private LeanSelectable ThisLeanSelectable
        => _thisLeanSelectable ? _thisLeanSelectable : (_thisLeanSelectable = GetComponent<LeanSelectable>());

    private MeshRenderer _meshRenderer;
    private MeshRenderer MeshRenderer => _meshRenderer ? _meshRenderer : (_meshRenderer = GetComponentInChildren<MeshRenderer>());

    private Dictionary<Utility.Side, Cup> _siblingsCups = new Dictionary<Utility.Side, Cup>();
    public Dictionary<Utility.Side, Cup> SiblingsCups => _siblingsCups;

    private bool _selected = false;
    private LeanFinger _finger;
    private Vector2 _sumDelta = Vector2.zero;

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

    private void OnSelect(LeanSelectable arg1, LeanFinger arg2)
    {
        if (arg1 != ThisLeanSelectable) return;

        _selected = true;
        _finger = arg2;
    }

    private void OnDeselect(LeanSelectable arg1)
    {
        _selected = false;
    }

    private void Update()
    {
        if (!_selected) return;

        var delta = _finger.ScreenDelta;

        _sumDelta += delta;
        if (_sumDelta.magnitude < 1) return;

        Utility.Side direction;
        if (delta.x > delta.y)
        {
            if (delta.x > 0) direction = Utility.Side.Right;
            else direction = Utility.Side.Left;
        }
        else
        {
            if (delta.y > 0) direction = Utility.Side.Up;
            else direction = Utility.Side.Down;
        }

        Swap(direction);
        ThisLeanSelectable.Deselect();
        _sumDelta = Vector2.zero;
    }

    public void SetColor(Color color)
    {
        MeshRenderer.material.SetColor("_Color", color);
    }

    public void Swap(Utility.Side direction)
    {
        if (!SiblingsCups.ContainsKey(direction)) return;

        var swapCap = SiblingsCups[direction];

        var thisPos = transform.position;
        var siblingPos = swapCap.transform.position;

        transform.position = siblingPos;
        swapCap.transform.position = thisPos;

        var oppositeDirection = Utility.Side.Left;
        switch (direction)
        {
            case Utility.Side.Left:
                oppositeDirection = Utility.Side.Right;
                break;
            case Utility.Side.Right:
                oppositeDirection = Utility.Side.Left;
                break;
            case Utility.Side.Up:
                oppositeDirection = Utility.Side.Down;
                break;
            case Utility.Side.Down:
                oppositeDirection = Utility.Side.Up;
                break;
        }

        var sides = new[] { Utility.Side.Left, Utility.Side.Right, Utility.Side.Down, Utility.Side.Up };
        for (var i = 0; i < sides.Length; i++)
        {
            if (sides[i] == oppositeDirection || sides[i] == direction) continue;

            if (!SiblingsCups.ContainsKey(sides[i])) SiblingsCups.Add(sides[i], null);
            if (!swapCap.SiblingsCups.ContainsKey(sides[i])) swapCap.SiblingsCups.Add(sides[i], null);
            var temp = SiblingsCups[sides[i]];
            SiblingsCups[sides[i]] = swapCap.SiblingsCups[sides[i]];
            swapCap.SiblingsCups[sides[i]] = temp;
        }

        if (!SiblingsCups.ContainsKey(oppositeDirection)) SiblingsCups.Add(oppositeDirection, null);
        SiblingsCups[oppositeDirection] = swapCap;
        swapCap.SiblingsCups[direction] = this;

    }
}