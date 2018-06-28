﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SpaceShipController : MonoBehaviour {
    public static SpaceShipController instance;
    public float speed,turnSpeed,smooth, threshold;
    //public Text thresholdTxt;
    public Transform camPivot,player,playerPivot;
    bool isGameOver;
    public int score;
    public GameObject destroyParticle;
    // Use this for initialization
    void Awake () {

        instance = this;

    }
	
	// Update is called once per frame
	void Update () {


        if (Input.GetMouseButtonDown(0))
        {

            if (isGameOver)
            {
                GameManager.instance.EndGame();
            }
                
            else
            {
                if (speed == 0)
                {
                    speed = 15;
                    InvokeRepeating("IncreaseSpeed", 12, 12);
                    GameManager.instance.StartGame();
                }
                    
               
            }
                
        }


        if (Input.GetMouseButton(0))
        {
           // thresholdTxt.text = Mathf.Abs(MouseHelper.mouseDelta.x).ToString();



            if (Mathf.Abs(MouseHelper.mouseDelta.x) > threshold)
            {
                playerPivot.transform.Rotate(Vector3.right * MouseHelper.mouseDelta.x * turnSpeed * Time.deltaTime);

            }
            
        }

        if (!isGameOver)
        {
            player.transform.rotation = Quaternion.Lerp(player.transform.rotation, playerPivot.transform.rotation, smooth * Time.deltaTime);
            camPivot.Rotate(Vector3.up * speed * Time.deltaTime);
        }
            
    }

    private void OnTriggerEnter(Collider other)
    {
       
        if (other.tag == "Wall")
        {
            isGameOver = true;
            Destroy(transform.GetChild(0).gameObject);
            Instantiate(destroyParticle, transform);
            Handheld.Vibrate();
        }

        if (other.tag == "Score")
        {
            score++;
            GameManager.instance.UpdateScore(score);
            other.GetComponentInParent<WallProperties>().Reset();
        }

    }

    public void IncreaseSpeed()
    {
        speed++;

        speed = Mathf.Clamp(speed, 0, 30);
        GameManager.instance.playerCam.RotateTest();
    }
}
