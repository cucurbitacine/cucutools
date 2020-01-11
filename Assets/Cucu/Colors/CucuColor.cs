using Cucu.Common;
using UnityEngine;

namespace Cucu.Colors
{
    public class CucuColor : MonoBehaviour
    {
        public static readonly Color[] Rainbow = new[] // TODO in "cucu colors"
        {
            Color.red,
            Color.red.LerpTo(Color.yellow),
            Color.yellow,
            Color.green,
            Color.cyan,
            Color.blue,
            Color.magenta
        };
    }
}
