using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using GameAnalyticsSDK;
using Facebook.Unity;

public class GameManager : MonoBehaviour {
    public static GameManager instance;
    public GameObject MainMenuGUI,InGameUI,EndGameUI;
    public PlayerCam playerCam;
    public WallProperties[] Walls;

    public Text HighScoreTxt,ScoreTxt,coinTxt, HighScoreTxt2, ScoreTxt2, coinTxt2,LevelTxt, LevelTxt2;

    public UIPop updateTxt;
    public Transform gemPivot,retryButton;
    public static bool isGameOver;
    public Image LevelCompleteBar;

    public int levelNumber,totalScore,coinCount,totalCoinCount;
    // Use this for initialization
    private void Awake()
    {
        instance = this;

        Application.targetFrameRate = 60;
      
    }


    void Start () {

        GameAnalytics.Initialize();
        FB.Init();

        LoadScore();

        LevelCompleteBar.DOFillAmount(((float)totalScore % 100) / 100, 0.5f);

    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public void LoadScore()
    {
        HighScoreTxt.text = "High Score : " + PlayerPrefs.GetInt("HScore").ToString();

        totalCoinCount = PlayerPrefs.GetInt("TotalCoins");

        totalScore = PlayerPrefs.GetInt("TotalScore");

        levelNumber = (totalScore / 100) + 1;

        LevelTxt.text = "Level " + levelNumber;
        LevelTxt2.text = "Level " + levelNumber+ " / 100";

        coinTxt2.text = totalCoinCount.ToString();

    }

    public void SaveScore()
    {
        if (SpaceShipController.instance.score > PlayerPrefs.GetInt("HScore"))
            PlayerPrefs.SetInt("HScore", SpaceShipController.instance.score);

        PlayerPrefs.SetInt("TotalScore", SpaceShipController.instance.score+ totalScore);
        PlayerPrefs.SetInt("TotalCoins", totalCoinCount + coinCount);
    }

    public void UpdateScore(int score)
    {
        ScoreTxt.text = score.ToString();
        ScoreTxt.transform.DOScale(Vector3.one * 0.15f, 0.2f).OnComplete(() =>
        {
            ScoreTxt.transform.DOScale(Vector3.one, 0.1f);
        });
        updateTxt.OnScore("+1");
    }

    public void UpdateCoin()
    {
        coinCount++;
        coinTxt.transform.DOScale(Vector3.one * 0.15f, 0.2f).OnComplete(() =>
        {
            coinTxt.transform.DOScale(Vector3.one, 0.1f);
            coinTxt.text = coinCount.ToString();
        });
        
    }

    public void StartGame()
    {
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "game");
        //GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "game", "level", levelNumber.ToString(), int.Parse(ScoreTxt.text));

        isGameOver = false;

        HideMainMenu();

        foreach (WallProperties wp in Walls)
            wp.ChangeWall();

        Spawner.instance.SetRandomWallColor();
        coinCount = 0;
        coinTxt.text = coinCount.ToString();
    }


    public void EndGame()
    {
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "game", SpaceShipController.instance.score);
        ShowEndScreen();
        SaveScore();
        LoadScore();
        InGameUI.SetActive(false);
        EndGameUI.SetActive(true);
        HighScoreTxt2.text = HighScoreTxt.text;
        ScoreTxt2.text = ScoreTxt.text;
        


    }

    
    public void RestartGame()
    {
        Application.LoadLevel(0);
    }



    public void HideMainMenu()
    {
        MainMenuGUI.GetComponent<Image>().DOFade(0, 0.3f).OnComplete(() =>
        {
            MainMenuGUI.SetActive(false);
            Invoke("ShowInGame",0.5f);
        });
    }


    public void ShowInGame()
    {
        InGameUI.SetActive(true);
        ScoreTxt.transform.DOScale(Vector3.one, 0.2f).OnComplete(()=>{
            coinTxt.transform.parent.DOMoveY(gemPivot.transform.position.y, 0.2f);
        });
        

    }

    public void ShowEndScreen()
    {
        ScoreTxt2.transform.DOScale(Vector3.one , 0.5f).OnComplete(() =>
        {
            HighScoreTxt2.transform.DOScale(Vector3.one, 0.5f).OnComplete(() =>
            {

                LevelCompleteBar.DOFillAmount(((float)totalScore% 100) / 100, 0.5f);
                retryButton.DOScale(Vector3.one, 0.2f);
            });
        });
        
    }
}
