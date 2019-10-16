using System;
using UnityEngine;

public class Util : MonoBehaviour {

    public static T RandomEnumValue<T>()
    {
        var v = Enum.GetValues(typeof(T));
        return (T)v.GetValue(new System.Random().Next(v.Length));
    }

    public static Vector3 MousePositionInTheWorld()
    {
        var vec = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        vec.z = 0;
        return vec;
    }

    public static string LevelNumberToString(int n)
    {
        return (n + 1).ToString();
    }

    public static bool randomProc(float perc)
    {
        return UnityEngine.Random.Range(0F, 1F) < perc / 100;
    }
}
