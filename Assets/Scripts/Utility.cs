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

        public static List<Cup> GetNearCups(this Cup startCup)
        {
            var outCups = new List<Cup>() { startCup };

            var cupsInRow = 1;
            //LEFT
            var checkIndex = (startCup.Index.Item2 - 1) % ShelfContainer.CupsMap.GetLength(1);
            while (true)
            {
                var checkCup = ShelfContainer.CupsMap[startCup.Index.Item1, checkIndex];
                if (checkCup.Color == startCup.Color)
                {
                    if (!outCups.Contains(checkCup))
                    {
                        outCups.Add(checkCup);
                        checkIndex = (checkIndex - 1) % ShelfContainer.CupsMap.GetLength(1);
                        cupsInRow++;
                    }
                    else break;
                }
                else break;
            }

            //RIGHT
            checkIndex = (startCup.Index.Item2 + 1) % ShelfContainer.CupsMap.GetLength(1);
            while (true)
            {
                var checkCup = ShelfContainer.CupsMap[startCup.Index.Item1, checkIndex];
                if (checkCup.Color == startCup.Color)
                {
                    if (!outCups.Contains(checkCup))
                    {
                        outCups.Add(checkCup);
                        checkIndex = (checkIndex + 1) % ShelfContainer.CupsMap.GetLength(1);
                        cupsInRow++;
                    }
                    else break;
                }
                else break;
            }

            if(cupsInRow <3) cupsInRow = 1;

            //UP
            checkIndex = (startCup.Index.Item1 + 1);
            while (true)
            {
                if (checkIndex >= ShelfContainer.CupsMap.GetLength(0)) break;
                var checkCup = ShelfContainer.CupsMap[checkIndex, startCup.Index.Item2];
                if (checkCup.Color == startCup.Color)
                {
                    if (!outCups.Contains(checkCup))
                    {
                        outCups.Add(checkCup);
                        checkIndex += 1;
                        cupsInRow++;
                    }
                    else break;
                }
                else break;
            }

            //DOWN
            checkIndex = (startCup.Index.Item1 - 1);
            while (true)
            {
                if (checkIndex < 0) break;
                var checkCup = ShelfContainer.CupsMap[checkIndex, startCup.Index.Item2];
                if (checkCup.Color == startCup.Color)
                {
                    if (!outCups.Contains(checkCup))
                    {
                        outCups.Add(checkCup);
                        checkIndex -= 1;
                        cupsInRow++;
                    }
                    else break;
                }
                else break;
            }

            if(cupsInRow < 3) outCups.Clear();
            return outCups;
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
