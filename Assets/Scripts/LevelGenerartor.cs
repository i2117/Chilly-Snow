using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LevelGenerartor : MonoBehaviour {

    public GameObject[] prefabs;

    float firstY = 5;
    float lastY;

    float defaultRandomProcChance;
    float randomProcChance;
    float maxX;

    float range = 35;

    GameObject currentLevel;

    private void OnEnable()
    {
        GameLogic.OnStartLevel += Initialize;
    }

    private void OnDisable()
    {
        GameLogic.OnStartLevel -= Initialize;
    }

    private void Initialize()
    {
        lastY = firstY;
        currentLevel = GetComponent<GameLogic>().currentLevelObject;
        defaultRandomProcChance = GetComponent<GameLogic>().defaultRandomProcChance;
        randomProcChance = defaultRandomProcChance;

        Debug.Log("Generator initialized");
    }

    private void Awake()
    {
        //Initialize();
        
    }

    private void Start()
    {
        maxX = Storage.instance.maxX;
    }

    GameObject NewObject (GameObject prefab, float x, float y)
    {
        return Instantiate(
            prefab,
            new Vector3(x, y, 0),
            Quaternion.identity,
            currentLevel.transform);
    }

    void AddLoop<T> (GameObject obj) where T: Loop
    {
        var loop = obj.AddComponent<T>();
        loop.SetRandomIterationDatas();
        loop.StartLooping();
    }

    void SetRandomBehaviour(GameObject obj)
    {
        // Move
        if (Util.randomProc(randomProcChance))
        {
            AddLoop<LoopMovement>(obj);
        }

        // Resize
        if (Util.randomProc(randomProcChance))
        {
            AddLoop<LoopResizing>(obj);
        }

        // Rotate
        if (Util.randomProc(randomProcChance))
        {
            AddLoop<LoopRotating>(obj);
        }
    }

    void CreateNewObject (float y)
    {
        var prefab = prefabs[Random.Range(0, prefabs.Length)];
        var x = Random.Range(-maxX, maxX);
        var go = NewObject(prefab, x, y);
        go.transform.localScale = go.transform.localScale * Random.Range(0.3F, 0.8F);

        SetRandomBehaviour(go);
    }

    private void Update()
    {
        if (!Player._transform || lastY > Player._transform.position.y + range)
            return;

        lastY += 2;
        CreateNewObject(lastY);

        randomProcChance = defaultRandomProcChance + lastY / 100;
        //Debug.Log(randomProcChance);
    }
}
