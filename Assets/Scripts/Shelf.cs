using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MrHat.Utility;

public class Shelf : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private float spawnRadius = 2.1f;
    [SerializeField] private int cupsCount = 15;
    [SerializeField] private Cup cupPrefab;

    private List<Cup> _cups = new List<Cup>();
    public List<Cup> Cups => _cups;

    private void Start() 
    {
    }

    public void SpawnCups()
    {
        var angleStep = 360 / cupsCount;

        for (var i = 0; i < 360; i += angleStep)
        {
            var angle = i * Mathf.Deg2Rad;
            var x = spawnRadius * Mathf.Cos(angle);
            var y = spawnRadius * Mathf.Sin(angle);
            var cupPosition = new Vector3(x, 0, y) + transform.position;
            var cup = Instantiate(cupPrefab, cupPosition, Quaternion.identity, transform);
            var colors = (CupColor[])System.Enum.GetValues(typeof(CupColor));
            cup.SetColor(colors[Random.Range(0,colors.Length)]);
            Cups.Add(cup);
        }
    }
}
