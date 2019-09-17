using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MrHat.Utility
{
    public static class Utility
    {
        public static Side GetSide(Vector2 direction)
        {
            return Mathf.Abs(direction.x) > Mathf.Abs(direction.y)
                ? direction.x > 0
                    ? Side.Right
                    : Side.Left
                : direction.y > 0
                    ? Side.Up
                    : Side.Down;
        }

        public static Color GetColor(CupColor cupColor)
        {
            switch (cupColor)
            {
                case CupColor.Red:
                    return Color.red;
                case CupColor.Blue:
                    return Color.blue;
                case CupColor.Green:
                    return Color.green;
                default:
                    return Color.white;
            }
        }

        public static void GetNearCups(this Cup startCup, ref Cup prevCup, ref HashSet<Cup> watchedCups, ref List<Cup> nearCups)
        {
            if (watchedCups.Contains(startCup)) return;
            else
            {
                watchedCups.Add(startCup);

                if (prevCup.Color != startCup.Color) return;
                else nearCups.Add(startCup);
            }

            var i = startCup.Index.Item1;
            var j = startCup.Index.Item2;

            var left = ShelfContainer.CupsMap[i, (j - 1 + ShelfContainer.CupsCount) % ShelfContainer.CupsCount];
            var right = ShelfContainer.CupsMap[i, (j + 1) % ShelfContainer.CupsCount];
            var up = i < ShelfContainer.Shelves.Count - 1 ? ShelfContainer.CupsMap[i + 1, j] : null;
            var down = i > 0 ? ShelfContainer.CupsMap[i - 1, j] : null;

            if (left) left.GetNearCups(ref startCup, ref watchedCups, ref nearCups);
            if (right) right.GetNearCups(ref startCup, ref watchedCups, ref nearCups);
            if (up) up.GetNearCups(ref startCup, ref watchedCups, ref nearCups);
            if (down) down.GetNearCups(ref startCup, ref watchedCups, ref nearCups);
        }
    }

    public enum Side
    {
        Left,
        Right,
        Up,
        Down
    }

    public enum CupColor
    {
        Red,
        Green,
        Blue
    }
}
