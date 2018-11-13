using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using GameAnalyticsSDK;
using Facebook.Unity;

public class GameManager : MonoBehaviour {
    public static GameManager instance;
    public GameObject MainMenuGUI,InGameUI,EndGameUI,continueButton,CountDownUI;
    public PlayerCam playerCam;
    public WallProperties[] Walls;

    public Text HighScoreTxt,ScoreTxt,coinTxt, HighScoreTxt2, ScoreTxt2, coinTxt2,LevelTxt, LevelTxt2,CountDownText;

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
        isGameOver = false;
        AppLovin.SetSdkKey("Mw_ES5QLA1N9SQwMF5ZANA5GH26ffm80f8E7DfSNgqX8ERUUX3evEkAVnGAQ0JRgRfofKE203Z8I0cnTgPSv8c");
        AppLovin.InitializeSdk();
        AppLovin.SetUnityAdListener("_GameManager");
        AppLovin.PreloadInterstitial();
        AppLovin.LoadRewardedInterstitial();

    }

    private void OnEnable()
    {
    }


    void Start () {

        #region SDK Initialisation
            GameAnalytics.Initialize();
            FB.Init();
                //Applovin SDK Integration



        #endregion /SDK Initialisation
        LoadScore();
        LevelCompleteBar.DOFillAmount(((float)totalScore % 150) / 150, 0.5f);
        SetAdsCount();

    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public void LoadScore()
    {
        HighScoreTxt.text = "High Score : " + PlayerPrefs.GetInt("HScore").ToString();

        totalCoinCount = PlayerPrefs.GetInt("TotalCoins");

        totalScore = PlayerPrefs.GetInt("TotalScore");

        levelNumber = (totalScore / 150) + 1;

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
        HideMainMenu();
        foreach (WallProperties wp in Walls)
            wp.ChangeWall();

        Spawner.instance.SetRandomWallColor();
        coinCount = 0;
        coinTxt.text = coinCount.ToString();
    }


    public void EndGame()
    {
        if(PlayerPrefs.GetInt("GameAdsCount",0) >= 3)
        {
            if (AppLovin.HasPreloadedInterstitial())
            {
                PlayerPrefs.SetInt("GameAdsCount",0);
                AppLovin.ShowInterstitial();
            }
        }
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

                LevelCompleteBar.DOFillAmount(((float)totalScore% 150) / 150, 0.5f);
                retryButton.DOScale(Vector3.one, 0.2f);
                continueButton.transform.DOScale(Vector3.one * 6, 0.5f).OnComplete(() =>
                {
                    AnimatoContinueButton();
                });

            });
        });
        
    }

    public void ContinuePlaying()
    {
        if (AppLovin.IsIncentInterstitialReady())
        {
            AppLovin.ShowRewardedInterstitial();
        }
        else
        {
            AppLovin.LoadRewardedInterstitial();
        }
    }

    void onAppLovinEventReceived(string ev)
    {
        if(ev.Contains("REWARDAPPROVEDINFO"))
        {
            
        } else if (ev.Contains("LOADEDREWARDED"))
        {
            StartCoroutine(CountDown());
        }
        else if (ev.Contains("LOADREWARDEDFAILED"))
        {
            // A rewarded video failed to load.
        }
        else if (ev.Contains("HIDDENREWARDED"))
        {
            // A rewarded video has been closed.  Preload the next rewarded video.
            AppLovin.LoadRewardedInterstitial();
        }
    }

    public void ResumeGame()
    {
        InGameUI.SetActive(true);
        CountDownUI.SetActive(false);
        SpaceShipController.instance.ResumeGame();
    }


    int i;
    public IEnumerator CountDown()
    {
        if (isGameOver)
        {

            PlayerPrefs.SetInt("GameAdsCount", PlayerPrefs.GetInt("GameAdsCount"));
            EndGameUI.SetActive(false);
            CountDownUI.SetActive(true);
            for (i = 3; i > 0; i--)
            {
                CountDownText.text = i.ToString();
                CountDownText.gameObject.transform.localScale = Vector3.one;
                CountDownText.gameObject.transform.DOScale(Vector3.zero, 1f).SetEase(Ease.Linear);
                yield return new WaitForSeconds(1f);
            }
            if (i == 0)
            {
                ResumeGame();
            }
        } else{
            yield return null;
        }
    }

    public void Test()
    {
        StartCoroutine(CountDown());
    }

    public void SetAdsCount()
    {
        int Count = PlayerPrefs.GetInt("GameAdsCount", 0);
        Count += 1;
        PlayerPrefs.SetInt("GameAdsCount", Count);
        AppLovin.ShowAd(AppLovin.AD_POSITION_CENTER, AppLovin.AD_POSITION_BOTTOM);
    }

    void AnimatoContinueButton()
    {
        //continueButton.gameObject.GetComponent<Image>().DOFillAmount(0, 5f).SetEase(Ease.Linear).OnComplete(() =>
        //{
        //    continueButton.SetActive(false);
        //});
        PingPongScaleContinueButton();
    }

    void PingPongScaleContinueButton()
    {
        continueButton.transform.DOScale(Vector3.one * 5, 0.3f).SetEase(Ease.Linear).OnComplete(() =>
        {
            continueButton.transform.DOScale(Vector3.one * 5.5f, 0.3f).SetEase(Ease.Linear).OnComplete(() => {
                PingPongScaleContinueButton();
            });
        });
    }
}
