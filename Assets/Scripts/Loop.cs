using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Loop : MonoBehaviour {

    [System.Serializable]
    public struct IterationData
    {
        public float preDelay;
        public Vector3 to;
        public float duration;
    }

    public IterationData[] iterationDatas;

    public abstract void ChangeValue(Vector3 newValue);

    protected Vector3 currentValue;
    protected abstract Vector3 CurrentValue { get; }
    protected Vector3 startVal;

    public void SetRandomIterationDatas()
    {
        var iter = RandomIteration();
        iterationDatas = new IterationData[]
        {
            iter,
            ReversedIteration(iter)
        };
    }

    protected float randomIterationsCount = 1;

    IterationData ReversedIteration(IterationData iter)
    {
        return new IterationData
        {
            preDelay = iter.preDelay,
            to = -iter.to,
            duration = iter.duration
        };
    }

    public IterationData RandomIteration()
    {
        return new IterationData
        {
            preDelay = RandomPreDelay(),
            to = RandomTo(),
            duration = RandomDuration()
        };
    }

    protected float RandomPreDelay()
    {
        return Random.Range(0, 4) * 0.4F;
    }

    protected abstract Vector3 RandomTo();
    
    protected float RandomDuration()
    {
        return Random.Range(0.4F, 3F);
    }

    void Awake()
    {
        startVal = currentValue;
    }

    IEnumerator Looping()
    {
        var currentN = 0;
        while (true)
        {
            var currentIter = iterationDatas[currentN];

            yield return new WaitForSeconds(currentIter.preDelay);

            var initVal = CurrentValue;
            var newVal = initVal + currentIter.to;

            var timeRun = 0F;
            while (timeRun <= currentIter.duration)
            {
                ChangeValue(Vector3.Lerp(initVal, newVal, timeRun / currentIter.duration));
                timeRun += Time.deltaTime;
                yield return new WaitForSeconds(Time.deltaTime);
            }

            yield return null;
            currentN = currentN < iterationDatas.Length - 1 ? currentN + 1 : 0;
        }
    }

    public void StartLooping()
    {
        StartCoroutine("Looping");
    }

    public void StopLooping()
    {
        StopCoroutine("Looping");
    }
}
