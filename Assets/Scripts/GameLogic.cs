using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class GameLogic : MonoBehaviour {

    #region References

    public Camera mainCamera;
    public GameObject loseMenu, pauseMenu, tutorialPanel;
    public BarWithMask progressBar;
    public Text levelText;
    public Button pauseButton;
    public Text highScoreText;

    Vector3 initPlayerPos = Vector3.zero;

    #endregion

    [HideInInspector]
    public GameObject currentLevelObject;
    GameObject currentPlayerObject;
    Vector3 lastObjectPos;

    int scoreForDistance = 0;
    int bonusScore = 0;

    float scoreCoeff = 1;

    public static int currentLevelNumber;

    bool _isPlaying = true;
    bool isPlaying
    {
        get
        {
            return _isPlaying;
        }
        set
        {
            _isPlaying = value;
            pauseButton.interactable = value;

            if (value)
            {
                pauseMenu.SetActive(false);
                loseMenu.SetActive(false);
            }
            else
                return;

            StopCoroutine("TweenTimeScale");
            Time.timeScale = 1;
        }
    }

    public static event Action OnStartLevel, OnWin, OnLose, OnContinue, OnPause;
    public static event Action<long> OnScoreChange;
    public static event Action<float> OnProgressChange;

    public int defaultRandomProcChance = 10;

    float currentDistance;
    float finishDistance;

    int CurrentScore
    {
        get
        {
            return scoreForDistance + bonusScore;
        }
    }

    float _currentProgress;
    float CurrentProgress
    {
        get
        {
            return _currentProgress;
        }
        set
        {
            _currentProgress = value;
            if (currentLevelNumber > 0)
                progressBar.SetBarProgress(value);
            if (OnProgressChange != null)
                OnProgressChange(value);
        }
    }

    private void Awake()
    {
        Advertisement.Initialize("123", true);
        Prefs.SetBool(PrefTypeBool.LevelUnlocked, 0, true);
        highScoreText.text = Prefs.GetInt(PrefTypeInt.LevelBestScore, 0).ToString();
        PlayLevel(0);

        if (!Prefs.GetBool(PrefTypeBool.TutorialWasShown, 0))
            ShowHideTutorial(true);
    }

    public void AddDistance(float delta)
    {
        if (!isPlaying || !Player._transform)
            return;

        //currentDistance += delta;
        currentDistance = Player._transform.position.y - initPlayerPos.y;
        scoreForDistance = (int)(currentDistance * scoreCoeff);

        OnScoreChange(CurrentScore);

        CurrentProgress = currentDistance / finishDistance;
    }

    IEnumerator TweenTimeScale (float to, float delay)
    {
        var initialTimeScale = Time.timeScale;
        var timeRun = 0F;
        while (timeRun <= delay)
        {
            Time.timeScale = Mathf.Lerp(initialTimeScale, to, timeRun / delay);
            timeRun += Time.unscaledDeltaTime;
            yield return new WaitForSecondsRealtime(Time.unscaledDeltaTime);
        }
    }

    public void PlayLevel (int n)
    {
        currentLevelNumber = n;
        progressBar.gameObject.SetActive(n > 0);
        Debug.Log("start__ playing level" + n.ToString());

        if (currentLevelObject)
            Destroy(currentLevelObject);

        bonusScore = 0;
        scoreForDistance = 0;
        currentDistance = 0;
        
        levelText.text = "Level " + Util.LevelNumberToString(currentLevelNumber);
        currentLevelObject = Instantiate(Storage.instance.levelPrefabs[n]);
        
        CreatePlayer(initPlayerPos);
        finishDistance = currentLevelNumber > 0 ?
            GameObject.FindGameObjectWithTag("Finish").transform.position.y - initPlayerPos.y :
            1;

        CreateLastResultLine();

        isPlaying = true;
        OnStartLevel();
        OnContinue();
        //mainCamera.transform.position = Vector3.zero;
        //mainCamera.GetComponent<Following>().StartFollowing();
        //mainCamera.GetComponent<Following>().MoveToTarget(currentPlayerObject);
        Debug.Log("end__ playing level" + n.ToString());
    }

    IEnumerator PlayLevelDelayed(int n, float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        PlayLevel(n);
    }

    void SetPlaying(bool isPlaying)
    {
        this.isPlaying = isPlaying;
        Time.timeScale = isPlaying ? 1 : 0.000000001F;

        if (isPlaying)
            OnContinue();
        else
            OnPause();
    }

    public void OpenClosePauseMenu()
    {
        var shouldOpen = !pauseMenu.activeSelf;
        pauseMenu.SetActive(shouldOpen);
        SetPlaying(!shouldOpen);
    }


    void CreatePlayer(Vector3 pos)
    {
        if (currentPlayerObject)
            Destroy(currentPlayerObject);

        pos.z = -Storage.instance.playerPrefab.transform.lossyScale.z / 2;
        currentPlayerObject =
            Instantiate(Storage.instance.playerPrefab, pos, Quaternion.identity, currentLevelObject.transform);

        mainCamera.GetComponent<Following>().targetObject = currentPlayerObject;
    }

    void CreateLastResultLine()
    {
        if (Prefs.GetBool(PrefTypeBool.LevelCompleted, currentLevelNumber))
            return;

        var bestDistance = Prefs.GetFloat(PrefTypeFloat.LevelBestDistance, currentLevelNumber);
        if (bestDistance < 1)
            return;

        GameObject go = Instantiate(
            Storage.instance.lastResultPrefab,
            Vector3.up * bestDistance,
            Quaternion.identity,
            currentLevelObject.transform);

        go.GetComponentInChildren<Text>().text =
            Math.Round(bestDistance, 0).ToString() + "m";
    }

    public void ResolveCollision (GameObject go, Collision2D collision)
    {
        if (!isPlaying)
            return;

        if (go.CompareTag("Player"))
        {
            var player = go.GetComponent<Player>();
            if (collision.collider.CompareTag("Object"))
            {
                var objectColorScript = collision.gameObject.GetComponent<ColorScript>();
                var objectScript = collision.gameObject.GetComponent<ObjectInLevel>();
                lastObjectPos = collision.transform.position;
                if (player.colorScript.ColorName == objectColorScript.ColorName)
                {
                    // Collect

                    bonusScore += objectScript.BonusAmount();

                    OnScoreChange(CurrentScore);
                    objectScript.Collect();
                    SoundManager.instance.PlaySound(Storage.instance.collectSound);
                }
                else
                {
                    objectScript.CollideAsEnemy();
                    if (player.IsMortal)
                        Lose();
                }
            }
        }
    }

    void DestroyPlayer()
    {
        SoundManager.instance.PlaySound(Storage.instance.boomSound);
        currentPlayerObject.GetComponent<Player>().DestroyPlayer();
    }

    public void ResolveTrigger(GameObject go, Collider2D collider)
    {
        if (!isPlaying)
            return;

        if (go.CompareTag("Player"))
        {
            if (collider.CompareTag("Finish"))
            {
                Win();
            }
        }
    }

    public void WhenPlayerLeftCameraView()
    {
        if (!isPlaying || !currentPlayerObject)
            return;

        Debug.Log("Player left camera view");
        lastObjectPos = currentPlayerObject.transform.position;
        lastObjectPos.x = 0;

        Lose();
    }

    void SaveHighScore()
    {
        if (CurrentScore > Prefs.GetInt(PrefTypeInt.LevelBestScore, currentLevelNumber))
        {
            highScoreText.text = CurrentScore.ToString();
            Prefs.SetInt(PrefTypeInt.LevelBestScore, currentLevelNumber, CurrentScore);
        }

        if (currentDistance > Prefs.GetFloat(PrefTypeFloat.LevelBestDistance, currentLevelNumber))
        {
            Prefs.SetFloat(PrefTypeFloat.LevelBestDistance, currentLevelNumber, currentDistance);
        }
    }

    void Win()
    {
        SoundManager.instance.PlaySound(Storage.instance.winSound);
        isPlaying = false;
        //StartCoroutine(TweenTimeScale(0.1F, 1));
        SaveHighScore();
        OnWin();
        Prefs.SetBool(PrefTypeBool.LevelCompleted, currentLevelNumber, true);
        if (currentLevelNumber < Storage.instance.levelPrefabs.Length - 1)
        {
            Prefs.SetBool(PrefTypeBool.LevelUnlocked, currentLevelNumber + 1, true);
            StartCoroutine(PlayLevelDelayed(currentLevelNumber + 1, 1));
        }
            
        Debug.Log("Win!");
    }

    void EndLevel()
    {
        isPlaying = false;
        SaveHighScore();
        OnLose();
    }

    void Lose ()
    {
        DestroyPlayer();
        loseMenu.SetActive(true);
        EndLevel();
        //StartCoroutine(TweenTimeScale(0.1F, 1));
        
        Debug.Log("Game Over!");
    }

    public void OnRestartButton()
    {
        EndLevel();
        SoundManager.instance.PlaySound(Storage.instance.buttonSound);
        //CurrentScore = 0;
        PlayLevel(currentLevelNumber);
    }

    public void OnContinueButton()
    {
        SoundManager.instance.PlaySound(Storage.instance.buttonSound);
        //ADSComponent.UserOptToWatchAd();
        ShowRewardedAd();
#if UNITY_EDITOR
        //Continue();
#endif
    }

    public void PlayButtonSound()
    {
        SoundManager.instance.PlaySound(Storage.instance.buttonSound);
    }

    public void Continue()
    {
        //pauseMenu.SetActive(false);
        //loseMenu.SetActive(false);
        if (!currentPlayerObject)
            CreatePlayer(new Vector3 (0, lastObjectPos.y, 0));
        OnContinue();

        isPlaying = true;
    }

    public void ShowHideTutorial(bool state)
    {
        tutorialPanel.SetActive(state);
        if (state)
            Prefs.SetBool(PrefTypeBool.TutorialWasShown, 0, true);
    }


    public void ShowRewardedAd()
    {
        if (Advertisement.IsReady("rewardedVideo"))
        {
            var options = new ShowOptions { resultCallback = HandleShowResult };
            Advertisement.Show("rewardedVideo", options);
            Debug.Log("is watching");
        }
    }

    private void HandleShowResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                Debug.Log("The ad was successfully shown.");
                //
                // YOUR CODE TO REWARD THE GAMER
                Continue();
                break;
            case ShowResult.Skipped:
                Debug.Log("The ad was skipped before reaching the end.");
                break;
            case ShowResult.Failed:
                Debug.LogError("The ad failed to be shown.");
                break;
        }
    }

}
