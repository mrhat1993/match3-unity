using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MrHat.Utility;

public class Shelf : MonoBehaviour
{
    private List<Cup> _cups = new List<Cup>();
    public List<Cup> Cups => _cups;

    private void Start() 
    {
    }

    public void SpawnCups()
    {
        var angleStep = 360 / ShelfContainer.CupsCount;

        for (var i = 0; i < 360; i += angleStep)
        {
            var angle = i * Mathf.Deg2Rad;
            var x = ShelfContainer.SpawnRadius * Mathf.Cos(angle);
            var y = ShelfContainer.SpawnRadius * Mathf.Sin(angle);
            var cupPosition = new Vector3(x, 0, y) + transform.position;
            var cup = Instantiate(ShelfContainer.CupPrefab, cupPosition, Quaternion.identity, transform);
            var colors = (CupColor[])System.Enum.GetValues(typeof(CupColor));
            cup.SetColor(colors[Random.Range(0,colors.Length)]);
            Cups.Add(cup);
        }
    }
}
