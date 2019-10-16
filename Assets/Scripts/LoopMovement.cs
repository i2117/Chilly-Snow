using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopMovement : Loop {

    protected override Vector3 CurrentValue
    {
        get
        {
            return transform.position;
        }
    }

    public override void ChangeValue(Vector3 newValue)
    {
        transform.position = newValue;
    }

    protected override Vector3 RandomTo()
    {
        return new Vector3(Random.Range(-7F, 7F), Random.Range(-7F, 7F), 0);
    }
}
