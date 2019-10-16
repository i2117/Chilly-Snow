using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum PrefType { };

public enum PrefTypeBool { LevelUnlocked, LevelCompleted, TutorialWasShown };
public enum PrefTypeInt { LevelBestScore };
public enum PrefTypeString { };
public enum PrefTypeFloat { LevelBestDistance };

public static class Prefs
{
    static string ParametersToString(object prefType, int index)
    {
        return prefType.ToString() + "_" + index.ToString();
    }

    // generic
    static T Get<T>(PrefType prefType, int index)
    {
        if (typeof(T) == typeof(bool))
        {
            return (T)(object)PlayerPrefs.HasKey(ParametersToString(prefType, index));
        }
        if (typeof(T) == typeof(int))
        {
            return (T)(object)PlayerPrefs.HasKey(ParametersToString(prefType, index));
        }

        return (T)(object)null;
    }

    // bool

    public static bool GetBool(PrefTypeBool prefType, int index)
    {
        return PlayerPrefs.HasKey(ParametersToString(prefType, index));
    }

    public static void SetBool(PrefTypeBool prefType, int index, bool val)
    {
        if (val)
        {
            PlayerPrefs.SetString(ParametersToString(prefType, index), "bool_true");
        }
        else
        {
            PlayerPrefs.DeleteKey(ParametersToString(prefType, index));
        }
    }

    // int

    public static int GetInt(PrefTypeInt prefType, int index)
    {
        return PlayerPrefs.GetInt(ParametersToString(prefType, index), 0);
    }

    public static void SetInt(PrefTypeInt prefType, int index, int val)
    {
        PlayerPrefs.SetInt(ParametersToString(prefType, index), val);
    }

    // string

    public static string GetString(PrefTypeString prefType, int index)
    {
        return PlayerPrefs.GetString(ParametersToString(prefType, index), "");
    }

    public static void SetString(PrefTypeString prefType, int index, string val)
    {
        PlayerPrefs.SetString(ParametersToString(prefType, index), val);
    }

    // float

    public static float GetFloat(PrefTypeFloat prefType, int index)
    {
        return PlayerPrefs.GetFloat(ParametersToString(prefType, index), 0);
    }

    public static void SetFloat(PrefTypeFloat prefType, int index, float val)
    {
        PlayerPrefs.SetFloat(ParametersToString(prefType, index), val);
    }

#if UNITY_EDITOR
    [MenuItem("Tools/Clear PlayerPrefs")]
    public static void ResetAll()
    {
        PlayerPrefs.DeleteAll();
    }
#endif

    /*
#region Implementation

// int

int IPref<int>.Get(PrefType prefType, int index)
{
    return PlayerPrefs.GetInt(ParametersToString(prefType, index), 0);
}

void IPref<int>.Set(PrefType prefType, int val, int index)
{
    PlayerPrefs.SetInt(ParametersToString(prefType, index), val);
}

// string

string IPref<string>.Get(PrefType prefType, int index)
{
    return PlayerPrefs.GetString(ParametersToString(prefType, index), "");
}

void IPref<string>.Set(PrefType prefType, string val, int index)
{
    PlayerPrefs.SetString(ParametersToString(prefType, index), val);
}

// float

float IPref<float>.Get(PrefType prefType, int index)
{
    return PlayerPrefs.GetFloat(ParametersToString(prefType, index), 0);
}

void IPref<float>.Set(PrefType prefType, float val, int index)
{
    PlayerPrefs.SetFloat(ParametersToString(prefType, index), val);
}

#endregion
*/
}
