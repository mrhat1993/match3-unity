using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shelf : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private float spawnRadius = 2.1f;
    [SerializeField] private int cupsCount = 15;
    [SerializeField] private Cup cupPrefab;

    private Color[] _colors = new[] { Color.red, Color.green, Color.blue };

    private void Start() 
    {
        SpawnCups();
    }

    private void SpawnCups()
    {
        var angleStep = 360 / cupsCount;

        for (int i = 0; i < 360; i+=angleStep)
        {
            var angle = i * Mathf.Deg2Rad;
            var x = spawnRadius * Mathf.Cos(angle);
            var y = spawnRadius * Mathf.Sin(angle);
            var cupPosition = new Vector3(x, 0, y) + transform.position;
            var cup = Instantiate(cupPrefab, cupPosition, Quaternion.identity, transform);
            cup.SetColor(_colors[Random.Range(0,_colors.Length)]);
        }
    }
}
