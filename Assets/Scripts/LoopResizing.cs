using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopResizing : Loop
{
    protected override Vector3 CurrentValue
    {
        get
        {
            return transform.localScale;
        }
    }

    public override void ChangeValue(Vector3 newValue)
    {
        transform.localScale = newValue;
    }

    protected override Vector3 RandomTo()
    {
        return new Vector3(Random.Range(0F, 4F), Random.Range(0F, 4F), 1);
    }
}
