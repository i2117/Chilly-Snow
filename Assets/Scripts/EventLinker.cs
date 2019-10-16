using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventLinker : MonoBehaviour {

    GameLogic gameLogic;
    Following following;
    ForceController forceController;
    Indicator indicator;

    private void Awake()
    {
        gameLogic = GetComponent<GameLogic>();
        following = FindObjectOfType<Following>();
        indicator = FindObjectOfType<Indicator>();
    }

    private void OnEnable()
    {
        Player.OnCollision += gameLogic.ResolveCollision;
        Player.OnTrigger += gameLogic.ResolveTrigger;
        Player.OnLeavingCameraArea += gameLogic.WhenPlayerLeftCameraView;

        LevelSelector.OnClickPlay += gameLogic.PlayLevel;

        GameLogic.OnContinue += following.StartFollowing;
        GameLogic.OnLose += following.StopFollowing;
        GameLogic.OnWin += following.StopFollowing;
        GameLogic.OnScoreChange += indicator.SetText;
        ADSComponent.OnRewardedWatched += gameLogic.Continue;

        ForceController.OnDeltaMove += gameLogic.AddDistance;
    }

    private void OnDisable()
    {
        Player.OnCollision -= gameLogic.ResolveCollision;
        Player.OnTrigger -= gameLogic.ResolveTrigger;
        Player.OnLeavingCameraArea -= gameLogic.WhenPlayerLeftCameraView;

        LevelSelector.OnClickPlay -= gameLogic.PlayLevel;

        GameLogic.OnContinue -= following.StartFollowing;
        GameLogic.OnLose -= following.StopFollowing;
        GameLogic.OnWin -= following.StopFollowing;
        GameLogic.OnScoreChange -= indicator.SetText;
        ADSComponent.OnRewardedWatched -= gameLogic.Continue;

        ForceController.OnDeltaMove -= gameLogic.AddDistance;
    }
}
