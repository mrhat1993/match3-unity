using Lean.Touch;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShelfContainer : MonoBehaviour
{
    [SerializeField] private List<Shelf> _shelves;

    private bool _ignoreStartedOverGui = true;
    private bool _ignoreIsOverGui = false;
    private Vector3 _axis = Vector3.down;
    private Space _space = Space.Self;

    private Cup[][] _cupsMap;

    public int RequiredFingerCount;

    public LeanSelectable RequiredSelectable;

    public static bool RotateEnabled = true;

    protected virtual void Start()
    {
        if (RequiredSelectable == null)
        {
            RequiredSelectable = GetComponent<LeanSelectable>();
        }

        for (var i = 0; i < _shelves.Count; i++)
        {
            var downShelf = (i > 0) ? _shelves[i - 1] : null;
            _shelves[i].SpawnCups(downShelf);
        }

    }

    protected virtual void Update()
    {
        if (!RotateEnabled) return;

        var fingers = LeanSelectable.GetFingers(_ignoreStartedOverGui, _ignoreIsOverGui, RequiredFingerCount, RequiredSelectable);
        if (fingers.Count <= 0) return;

        var firstFinger = fingers[0];
        var direction = Mathf.Sign(firstFinger.ScreenDelta.x);
        var magnitude = firstFinger.ScreenDelta.magnitude;

        transform.Rotate(_axis, direction * magnitude, _space);
    }
}
