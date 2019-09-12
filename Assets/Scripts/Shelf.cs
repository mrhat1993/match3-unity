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

    private Color[] _colors = new[] { Color.red, Color.green, Color.blue };
    private List<Cup> _cups = new List<Cup>();
    private List<Cup> Cups => _cups;

    private void Start() 
    {
    }

    public void SpawnCups(Shelf downShelf)
    {
        var angleStep = 360 / cupsCount;

        for (var i = 0; i < 360; i+=angleStep)
        {
            var angle = i * Mathf.Deg2Rad;
            var x = spawnRadius * Mathf.Cos(angle);
            var y = spawnRadius * Mathf.Sin(angle);
            var cupPosition = new Vector3(x, 0, y) + transform.position;
            var cup = Instantiate(cupPrefab, cupPosition, Quaternion.identity, transform);
            cup.SetColor(_colors[Random.Range(0,_colors.Length)]);
            Cups.Add(cup);
        }

        for (var i = 0; i < Cups.Count; i++)
        {
            var prevIndex = i <= 0 ? Cups.Count-1 : i - 1;
            var nextIndex = (i + 1) % Cups.Count;

            Cups[i].SiblingsCups[Utility.Side.Left] = Cups[prevIndex];
            Cups[prevIndex].SiblingsCups[Utility.Side.Right] = Cups[i];

            Cups[i].SiblingsCups[Utility.Side.Right] = Cups[nextIndex];
            Cups[nextIndex].SiblingsCups[Utility.Side.Left] = Cups[nextIndex];

            //if (upShelf) Cups[i].SiblingsCups.Add(Utility.Side.Up, upShelf.Cups[i]);
            if (downShelf)
            {
                Cups[i].SiblingsCups[Utility.Side.Down] = downShelf.Cups[i];
                downShelf.Cups[i].SiblingsCups[Utility.Side.Up] = Cups[i];
            }
        }
    }
}
