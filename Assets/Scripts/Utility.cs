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
    }

    public enum Side
    {
        Left,
        Right,
        Up,
        Down
    }
}
