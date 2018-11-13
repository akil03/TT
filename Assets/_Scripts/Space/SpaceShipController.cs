using System.Collections;
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
    public GameObject destroyParticle,coinParticle;

    public AudioClip scoreClip, gemClip, deathClip;
    public AudioSource Engine;
    AudioSource speaker;
    GameObject hitObject;
    // Use this for initialization
    void Awake () {

        instance = this;
        speaker = GetComponent<AudioSource>();
        smooth = turnSpeed / 3;

    }
	
	// Update is called once per frame
	void Update () {


        if (Input.GetMouseButtonDown(0))
        {
            if (isGameOver)
            {
            }
                
            else
            {
                if (speed == 0)
                {
                    speed = 11f+(((float)GameManager.instance.levelNumber)/3);
                    speed = Mathf.Clamp(speed, 10, 15);
                    InvokeRepeating("IncreaseSpeed", 12, 12);
                    Engine.volume = 0.2f;
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
            player.transform.rotation = Quaternion.Lerp(player.transform.rotation, playerPivot.transform.rotation, smooth*Time.deltaTime);
            Engine.pitch = Mathf.Lerp(Engine.pitch, Mathf.Clamp(1.2f + (Mathf.Abs(player.transform.rotation.eulerAngles.x - playerPivot.transform.rotation.eulerAngles.x)/40),1.2f,2.2f),smooth*Time.deltaTime);
            camPivot.Rotate(Vector3.up * speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
       
        if (other.tag == "Wall"&& !isGameOver)
        {
            
            speaker.volume=0.4f;
            speaker.PlayOneShot(deathClip);
            isGameOver = true;
            hitObject = other.gameObject.transform.parent.transform.parent.gameObject;
            GameManager.isGameOver = true;
            GameManager.instance.EndGame();
            transform.GetChild(0).gameObject.SetActive(false);
            Instantiate(destroyParticle, transform);
            hitObject.SetActive(false);
            Handheld.Vibrate();;
        }

        if (other.tag == "Coin")
        {
            Instantiate(coinParticle, other.transform.position,Quaternion.identity);
            other.transform.parent.parent.gameObject.SetActive(false);
            speaker.PlayOneShot(scoreClip);
            GameManager.instance.UpdateCoin();
            Handheld.Vibrate();
        }

        if (other.tag == "Score")
        {
            //score++;
            speaker.PlayOneShot(scoreClip);
            //GameManager.instance.UpdateScore(score);
            //other.GetComponentInParent<WallProperties>().Reset();
        }

    }


    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Score")
        {
            score++;
            
            GameManager.instance.UpdateScore(score);
            other.GetComponentInParent<WallProperties>().Reset();
        }
    }

    public void IncreaseSpeed()
    {
        speed+=0.5f;

        speed = Mathf.Clamp(speed, 0, 30);

        GameManager.instance.playerCam.RotateTest();
    }

    public void ResumeGame()
    {
        Invoke("ResumeMovement", 0.5f);
        GameObject currentplayer = transform.GetChild(0).gameObject;
        currentplayer.SetActive(true);
        GameManager.instance.UpdateScore(score);
    }

    void ResumeMovement()
    {
        isGameOver = false;
        Invoke("EnablePlayerCollider", 2f);
    }

    public void EnablePlayerCollider()
    {
        hitObject.SetActive(true);
    }
}
