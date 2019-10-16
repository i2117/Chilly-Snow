using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage : MonoBehaviour {

    public static Storage instance;

    public GameObject playerPrefab;
    public GameObject lastResultPrefab;
    public GameObject floatingTextPrefab;
    public GameObject[] levelPrefabs;

    public float maxX = 10;

    public AudioClip
        buttonSound,
        boomSound,
        collectSound,
        winSound,
        turnSound;

    private void Awake()
    {
        instance = this;
    }
}
