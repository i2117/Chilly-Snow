using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VibrCaller : MonoBehaviour {

	public void Vibrate(int ms)
    {
        Vibration.Vibrate((long)ms);
    }

    public void Cancel()
    {
        Vibration.Cancel();
    }
}
