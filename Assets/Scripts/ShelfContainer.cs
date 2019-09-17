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

    public static List<Shelf> Shelves => Instance._shelves;

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
        for (var i = 0; i < Shelves.Count; i++)
        {
            Shelves[i].SpawnCups();
        }

        CupsMap = new Cup[Shelves.Count, Shelves[0].Cups.Length];

        for (var i = 0; i < CupsMap.GetLength(0); i++)
        {
            for (var j = 0; j < CupsMap.GetLength(1); j++)
            {
                CupsMap[i, j] = Shelves[i].Cups[j];
                Shelves[i].Cups[j].Index = (i, j);
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
        DestroyMatches();
        FillShelves();
    }

    public static void DestroyMatches()
    {
        var cupsToDestroy = new List<Cup>();
        var watchedCups = new HashSet<Cup>();

        var i = 0;
        var j = 0;

        while (watchedCups.Count < Shelves.Count * CupsCount)
        {
            var watchCup = CupsMap[i, j];
            watchCup.GetNearCups(ref watchCup, ref watchedCups, ref cupsToDestroy);

            j++;
            if(j >= CupsCount)
            {
                i++;
                j = 0;

                if (i >= Shelves.Count) break;
            }
        }

        Debug.Log(cupsToDestroy.Count);
        while (cupsToDestroy.Count > 0)
        {
            CupsMap[cupsToDestroy[0].Index.Item1, cupsToDestroy[0].Index.Item2] = null;
            Destroy(cupsToDestroy[0].gameObject);
            cupsToDestroy.RemoveAt(0);
        }
    }

    public static void FillShelves()
    {
        for (var i = 0; i < CupsMap.GetLength(0); i++)
        {
            for (var j = 0; j < CupsMap.GetLength(1); j++)
            {
                if (CupsMap[i, j] != null) continue;

                Cup cupToMove = null;
                for (var k = i + 1; k < CupsMap.GetLength(0); k++)
                {
                    if (CupsMap[k, j] != null)
                    {
                        cupToMove = CupsMap[k, j];
                        break;
                    }
                }

                var destinationPosition = Shelves[i].CupsPositions[j];
                if (!cupToMove)
                {
                    var spawnPosition = destinationPosition;
                    spawnPosition.y = 10f;
                    var cup = Instantiate(CupPrefab, spawnPosition, Quaternion.identity, Shelves[i].transform);
                    var colors = (CupColor[])System.Enum.GetValues(typeof(CupColor));
                    cup.SetColor(colors[Random.Range(0, colors.Length)]);
                    cupToMove = cup;
                }
                else
                {
                    CupsMap[cupToMove.Index.Item1, cupToMove.Index.Item2] = null;
                }

                CupsMap[i, j] = cupToMove;
                cupToMove.transform.parent = Shelves[i].transform;
                cupToMove.transform.DOLocalMove(destinationPosition, 0.5f);

            }
        }
    }
}
