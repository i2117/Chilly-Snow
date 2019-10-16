using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Following : MonoBehaviour {

    [SerializeField]
    private bool followX;
    [SerializeField]
    private bool followY;
    [SerializeField]
    private bool followZ;

    [SerializeField]
    private Vector3 offset;

    [SerializeField]
    private string targetTag;

    Transform _transform;

    public GameObject targetObject;
    FollowingRestrict followingRestrict;

    bool isFollowing = true;

    private void OnEnable()
    {
        //GameLogic.OnStartLevel += StartFollowing;
    }

    private void OnDisable()
    {
        //GameLogic.OnStartLevel -= StartFollowing;
    }

    private void Awake()
    {
        _transform = transform;
        followingRestrict = GetComponent<FollowingRestrict>();
    }

    private void Start()
    {
        targetObject = Player._transform.gameObject;// GameObject.FindGameObjectWithTag(targetTag);
    }

    public void StartFollowing()
    {
        Debug.LogWarning("Started following");
        isFollowing = true;
    }

    public void StopFollowing()
    {
        //MoveToTarget(targetObject);
        Debug.LogWarning("Stopped following");
        isFollowing = false;
    }

    void MoveToTarget()
    {
        MoveToTarget(targetObject);
    }

    public void MoveToTarget(GameObject target)
    {
        var currentPosition = _transform.position;

        if (!target && !targetObject)
        {
            Debug.LogWarning("no obj");
            targetObject = Player._transform.gameObject;// GameObject.FindGameObjectWithTag(targetTag);
            return;
        }

        //Debug.Log("moving to " + target.name);

        var targetPosition = target.transform.position;
        _transform.position = new Vector3(
            followX ? targetPosition.x : currentPosition.x,
            followY ? targetPosition.y : currentPosition.y,
            followZ ? targetPosition.z : currentPosition.z
            ) + offset;
    }

    private void Update()
    {
        if (isFollowing && 
            followingRestrict &&
            _transform.position.y >= followingRestrict.maxY)
        {
            //StopFollowing();
            return;
        }
    }

    private void LateUpdate()
    {
        if (!isFollowing)
            return;

        MoveToTarget(targetObject);    
    }
}
