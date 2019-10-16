using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class FollowingRestrict : MonoBehaviour {

    [SerializeField]
    private string objectTag;
    [SerializeField]
    private float additionalY = 1;

    [HideInInspector]
    public float maxY;

    private void OnEnable()
    {
        GameLogic.OnStartLevel += SetRestrict;
    }

    private void OnDisable()
    {
        GameLogic.OnStartLevel -= SetRestrict;
    }

    public void SetRestrict()
    {
        var target = GameObject.FindGameObjectWithTag("Finish");
        if (!target)
            return;

        maxY = target.transform.position.y -
            GetComponent<CameraSizeHandler>().height / 2 +
            additionalY;

        Debug.Log(maxY);
    }
}
