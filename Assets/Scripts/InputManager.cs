using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

    public static event Action OnPressed;
    public static event Action<bool> OnHoldingKey;

    public bool isEnabled = true;
    bool isHolding;

    private void OnEnable()
    {
        GameLogic.OnLose += Disable;
        GameLogic.OnWin += Disable;
        GameLogic.OnPause += Disable;
        GameLogic.OnContinue += Enable;
    }

    private void OnDisable()
    {
        GameLogic.OnLose -= Disable;
        GameLogic.OnWin -= Disable;
        GameLogic.OnPause -= Disable;
        GameLogic.OnContinue -= Enable;
    }

    private void Enable()
    {
        isEnabled = true;
    }

    private void Disable()
    {
        isEnabled = false;
    }

    private void Update()
    {
        if (!isEnabled)
            return;

        /*
        if (Input.anyKeyDown)
        {
            OnPressed();
        } */

        OnHoldingKey?.Invoke(isHolding);
    }

    public void OnButtonPressed()
    {
        if (!isEnabled)
            return;

        isHolding = true;
        OnPressed();
        Debug.Log("Pressed");
    }

    public void OnButtonReleased()
    {
        if (!isEnabled)
            return;

        isHolding = false;
        Debug.Log("released");
    }
}
