using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class PlayerController : MonoBehaviour {
    public float speed;
    public GameObject playerX, playerO;
    public Transform camPivot;
    public bool isX;
    bool isGameOver;
    public bool isSwitching;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        

        

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {

            if (isGameOver)
                Application.LoadLevel(0);

            ChangeTile();

        }

        if (!isGameOver)
            camPivot.Rotate(Vector3.up * speed * Time.deltaTime);
    }


    public void ChangeTile()
    {
        if (isSwitching)
            return;

        if (isX)
        {
            //playerX.SetActive(false);
            //playerO.SetActive(true);
            isX = false;
            playerX.transform.DOScale(Vector3.zero, 0.1f).OnComplete(() =>
            {
                playerO.transform.DOScale(Vector3.one, 0.1f);
                isSwitching = false;
            });
        }
        else
        {
            //playerX.SetActive(true);
            //playerO.SetActive(false);
            isX = true;
            playerO.transform.DOScale(Vector3.zero, 0.1f).OnComplete(() =>
            {
                playerX.transform.DOScale(Vector3.one, 0.1f);
                isSwitching = false;
            });
        }

    }

    void GameOver()
    {
        Time.timeScale = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "X" && !isX)
            isGameOver = true;

        else if (other.tag == "O" && isX)
            isGameOver = true;
    }
}
