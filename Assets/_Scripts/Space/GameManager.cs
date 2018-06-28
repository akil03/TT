﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class GameManager : MonoBehaviour {
    public static GameManager instance;
    public GameObject MainMenuGUI,InGameUI;
    public PlayerCam playerCam;
    public WallProperties[] Walls;

    public Text HighScoreTxt,ScoreTxt;
    public UIPop updateTxt;
    // Use this for initialization
    private void Awake()
    {
        instance = this;
        
        
    }


    void Start () {
        LoadScore();

    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public void LoadScore()
    {
        HighScoreTxt.text = "High Score : " + PlayerPrefs.GetInt("HScore").ToString();
    }

    public void SaveScore()
    {
        if (SpaceShipController.instance.score > PlayerPrefs.GetInt("HScore"))
            PlayerPrefs.SetInt("HScore", SpaceShipController.instance.score);
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

    public void StartGame()
    {
        MainMenuGUI.transform.GetChild(0).GetComponent<Image>().DOFade(0, 0.1f).OnComplete(() =>
        {
            MainMenuGUI.SetActive(false);
            InGameUI.SetActive(true);
        });


        foreach (WallProperties wp in Walls)
            wp.ChangeWall();

        Spawner.instance.SetRandomWallColor();
        
    }


    public void EndGame()
    {
        SaveScore();
        Application.LoadLevel(0);
    }
}
