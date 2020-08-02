using CucuTools;
using UnityEngine;

public class TestLog : MonoBehaviour
{
    public CucuColorPalette palette;

    private void Start()
    {
        var res = 0.Sin().Clamp01();
    }
}