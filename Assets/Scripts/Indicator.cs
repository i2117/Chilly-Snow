using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Indicator : MonoBehaviour {

    public Text text;

    private void Awake()
    {
        if (!text)
            text = GetComponent<Text>();
    }

    public void SetText<T>(T val)
    {
        text.text = val.ToString();
    }
}
