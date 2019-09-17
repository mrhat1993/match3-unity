using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MrHat.Utility;

public class Shelf : MonoBehaviour
{
    public Cup[] Cups { get; private set; }

    public Vector3[] CupsPositions { get; private set; }

    public void SpawnCups()
    {
        var angleStep = 360 / ShelfContainer.CupsCount;
        Cups = new Cup[ShelfContainer.CupsCount];
        CupsPositions = new Vector3[ShelfContainer.CupsCount];

        for (var i = 0; i < 360; i += angleStep)
        {
            var angle = i * Mathf.Deg2Rad;
            var x = ShelfContainer.SpawnRadius * Mathf.Cos(angle);
            var y = ShelfContainer.SpawnRadius * Mathf.Sin(angle);
            var cupPosition = new Vector3(x, 0, y) + transform.position;
            var cup = Instantiate(ShelfContainer.CupPrefab, cupPosition, Quaternion.identity, transform);
            var colors = (CupColor[])System.Enum.GetValues(typeof(CupColor));
            cup.SetColor(colors[Random.Range(0, colors.Length)]);
            Cups[i / angleStep] = cup;
            CupsPositions[i / angleStep] = cup.transform.localPosition;
        }
    }
}
