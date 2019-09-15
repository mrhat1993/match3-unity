using Lean.Touch;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MrHat.Utility;
using DG.Tweening;

public class ShelfContainer : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private List<Shelf> _shelves;
    [SerializeField] private float spawnRadius = 2.1f;
    [SerializeField] private Cup cupPrefab;
    [SerializeField] private int cupsCount = 15;

    private static ShelfContainer _instance;
    private static ShelfContainer Instance => 
        _instance ? _instance : _instance = FindObjectOfType<ShelfContainer>();

    public static Cup[,] CupsMap { get; set; }

    public static Cup CupPrefab => Instance.cupPrefab;
    public static int CupsCount => Instance.cupsCount;
    public static float SpawnRadius => Instance.spawnRadius;
    public static bool RotateEnabled { get; set; } = true;

    private bool _ignoreStartedOverGui = true;
    private bool _ignoreIsOverGui = false;

    private void OnEnable()
    {
        Cup.OnCupsSwapCompleted += CheckMatches;
    }

    private void OnDisable()
    {
        Cup.OnCupsSwapCompleted -= CheckMatches;
    }

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

    public static void CheckMatches(Cup first, Cup second)
    {
        first.CheckMatches();
        second.CheckMatches();
        FillShelves();
    }

    public static void FillShelves()
    {
        for (var i = 0; i < CupsMap.GetLength(0); i++)
        {
            for (var j = 0; j < CupsMap.GetLength(1); j++)
            {
                if(CupsMap[i,j] == null)
                {
                    Cup cupToMove = null;
                    for (var k = i+1; k < CupsMap.GetLength(0); k++)
                    {
                        if(CupsMap[k,j]!=null)
                        {
                            cupToMove = CupsMap[k, j];
                            break;
                        }
                    }

                    if (!cupToMove)
                    {
                        var angleStep = 360 / CupsCount;

                        var angle = j * angleStep * Mathf.Deg2Rad;

                        var x = SpawnRadius * Mathf.Cos(angle);
                        var y = SpawnRadius * Mathf.Sin(angle);

                        var shelf = Instance._shelves[i].transform;
                        var cupPosition = new Vector3(x, 10, y) + shelf.position;
                        var cup = Instantiate(CupPrefab, cupPosition, Quaternion.identity, shelf.transform);
                        var colors = (CupColor[])System.Enum.GetValues(typeof(CupColor));
                        cup.SetColor(colors[Random.Range(0, colors.Length)]);
                        cupToMove = cup;
                    }

                    CupsMap[i, j] = cupToMove;
                    cupToMove.transform.DOMove(cupToMove.transform.position + Vector3.down * 10f, 1f);
                }
            }
        }
    }
}
