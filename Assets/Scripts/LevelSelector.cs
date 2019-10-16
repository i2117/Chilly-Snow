using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour {

    public static Action<int> OnClickPlay;

    int minLevel = 0;
    int maxLevel;

    int selectedLevel;
    int SelectedLevel
    {
        get
        {
            return selectedLevel;
        }
        set
        {
            Debug.Log(maxLevel);
            selectedLevel = value;
            levelText.text = Util.LevelNumberToString(value);
            next.interactable = value < maxLevel;
            previous.interactable = value > minLevel;

            play.interactable = Prefs.GetBool(PrefTypeBool.LevelUnlocked, selectedLevel);
        }
    }

    public Text levelText;

    public Button 
        previous, 
        next, 
        play;

    private void OnEnable()
    {
        //if (SelectedLevel == 0)
            SelectedLevel = GameLogic.currentLevelNumber;
    }

    public void Previous()
    {
        Debug.Log(SelectedLevel);
        SelectedLevel--;
    }

    public void Next()
    {
        SelectedLevel++;
    }

    public void PlayClicked()
    {
        OnClickPlay(SelectedLevel);
    }

    private void Start()
    {
        Debug.Log("started!");
        maxLevel = Storage.instance.levelPrefabs.Length - 1;
        previous.onClick.AddListener(Previous);
        next.onClick.AddListener(Next);
        play.onClick.AddListener(PlayClicked);

        SelectedLevel = GameLogic.currentLevelNumber;
    }
}
