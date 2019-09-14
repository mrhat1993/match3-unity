using Lean.Touch;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShelfContainer : MonoBehaviour
{
    [SerializeField] private List<Shelf> _shelves;

    private static ShelfContainer _instance;
    private static ShelfContainer Instance => 
        _instance ? _instance : _instance = FindObjectOfType<ShelfContainer>();

    public static Cup[,] CupsMap { get; set; }

    public static bool RotateEnabled { get; set; } = true;

    private bool _ignoreStartedOverGui = true;
    private bool _ignoreIsOverGui = false;

    protected virtual void Start()
    {
        for (var i = 0; i < _shelves.Count; i++)
        {
            _shelves[i].SpawnCups();
        }

        CupsMap = new Cup[_shelves.Count, _shelves[0].Cups.Count];

        for (var i = 0; i < CupsMap.GetLength(0); i++)
        {
            for (var j = 0; j < CupsMap.GetLength(1); j++)
            {
                CupsMap[i, j] = _shelves[i].Cups[j];
                _shelves[i].Cups[j].Index = (i, j);
            }
        }
    }

    protected virtual void Update()
    {
        if (!RotateEnabled) return;

        var fingers = LeanSelectable.GetFingers(_ignoreStartedOverGui, _ignoreIsOverGui);
        if (fingers.Count <= 0) return;

        var firstFinger = fingers[0];
        var direction = Mathf.Sign(firstFinger.ScreenDelta.x);
        var magnitude = firstFinger.ScreenDelta.magnitude;

        transform.Rotate(Vector3.down, direction * magnitude, Space.Self);
    }
}
