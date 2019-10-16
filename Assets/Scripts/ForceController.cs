using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(Rigidbody))]
public class ForceController : MonoBehaviour {

    #region Settings

    float yForceDefault = 9;
    float desiredXForce = 8.5F; //7
    float changeSpeedCoeff = 50; //20
    float xForceBonusCoeffMin = 0.79F; //1
    float xForceBonusCoeffMax = 20F; //2.5
    float xForceBonusCoeffIncreaseAmount = 1F; //0.5

    #endregion

    [HideInInspector]
    public bool isStarted;
    float yForce = 0;
    float xForce = 0;
    float xForceBonusCoeff = 0.8F; //xForceBonusCoeffMin
    bool increasingBonusCoeff;

    //Rigidbody _rigidbody;
    public static event Action<float> OnDeltaMove;

    delegate bool Condition();

    IEnumerator currentTween;

    IEnumerator TweeningXForceTo (float to)
    {

        var dir = Mathf.Sign(to - xForce);

        Condition shouldTween;
        if (dir > 0)
            shouldTween = () => { return xForce < to; };
        else
            shouldTween = () => { return xForce > to; };

        while (shouldTween())
        {
            yield return null;
            xForce += dir * changeSpeedCoeff * Time.deltaTime;
        }
    }

    public void StartChangingDirection ()
    {
        desiredXForce *= 
            isStarted ? 
            -1 : 
            Mathf.Sign(Util.MousePositionInTheWorld().x - transform.position.x);

        if (!isStarted)
        {
            isStarted = true;
            yForce = yForceDefault;
        }

        xForceBonusCoeff = xForceBonusCoeffMin;
        if (currentTween != null)
            StopCoroutine(currentTween);
        currentTween = TweeningXForceTo(desiredXForce);
        StartCoroutine(currentTween);
    }

    public void SetIncreasingBonusCoeff(bool state)
    {
        increasingBonusCoeff = state;
    }

    private void Awake()
    {
        //_rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        //StartChangingDirection();
    }

    private void Update()
    {
        var deltaTime = Time.deltaTime;

        if (increasingBonusCoeff && xForceBonusCoeff < xForceBonusCoeffMax)
        {
            xForceBonusCoeff += xForceBonusCoeffIncreaseAmount * deltaTime;
        }
        else if (!increasingBonusCoeff && xForceBonusCoeff > xForceBonusCoeffMin)
        {
            xForceBonusCoeff -= xForceBonusCoeffIncreaseAmount * deltaTime;
        }

        var delta = deltaTime * new Vector3(
                            xForce * xForceBonusCoeff,
                            yForce,
                            0);

        transform.position += delta;
        OnDeltaMove(delta.magnitude);

        //Debug.Log(xForceBonusCoeff);
    }
}
