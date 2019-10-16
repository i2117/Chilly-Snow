using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopRotating : Loop
{
    protected override Vector3 CurrentValue
    {
        get
        {
            return transform.localEulerAngles;
        }
    }

    public override void ChangeValue(Vector3 newValue)
    {
        transform.localEulerAngles = newValue;
    }

    protected override Vector3 RandomTo()
    {
        return new Vector3(0, 0, Random.Range(-360, 360));
    }
}
