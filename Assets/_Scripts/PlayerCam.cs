using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class PlayerCam : MonoBehaviour {
	
    
    public Vector2 rotationLimits,rotationTimeGaps;
    public Vector3[] RandomPositions,RandomRotations;
    public float[] validrotations;
    public Transform temp;

    public bool isTunnel;
    public Vector3[] tunnelPositions;

    public bool stopRotate;
	// Use this for initialization
	void Start () {
        // RandomRotations();
        //RandomMovements();

        if (isTunnel)
            TunnelTest();
        else
            Invoke("RotateTest", Random.Range(rotationTimeGaps.x, rotationTimeGaps.y));

    }
	
	// Update is called once per frame
	void Update () {
        if (isTunnel)
            transform.LookAt(temp);
        
	}


    void RandomMovements()
    {
        int pos = Random.Range(0, RandomPositions.Length);
        float timeTaken = Random.Range(rotationLimits.x, rotationLimits.y);
        transform.DOLocalMove(RandomPositions[pos], timeTaken).OnComplete(() =>
        {
            Invoke("RandomMovements", Random.Range(rotationTimeGaps.x, rotationTimeGaps.y));

        });
        transform.DOLocalRotate(new Vector3(RandomRotations[pos].x, RandomRotations[pos].y, Random.Range(0, 360)), timeTaken, RotateMode.Fast);
    }

    public void RotateTest()
    {
        if (stopRotate)
            return;

        stopRotate = true;

        float timeTaken = Random.Range(rotationLimits.x, rotationLimits.y);
        transform.DOLocalRotate(new Vector3(0.863f, -81.635f, validrotations[Random.Range(0, validrotations.Length)]), timeTaken, RotateMode.Fast).OnComplete(() =>
        {
            // Invoke("RotateTest", Random.Range(rotationTimeGaps.x, rotationTimeGaps.y));
            stopRotate = false;

        });
    }

    void TunnelTest()
    {
        if (stopRotate)
            return;

        int pos = Random.Range(0, tunnelPositions.Length);
        float timeTaken = Random.Range(rotationLimits.x, rotationLimits.y);
        transform.DOLocalMove(tunnelPositions[pos], timeTaken).OnComplete(() =>
        {
            Invoke("RandomMovements", Random.Range(rotationTimeGaps.x, rotationTimeGaps.y));

        });
    }

    //void RandomRotations()
    //{
    //    //transform.LookAt(player.transform);
    //    transform.DOLocalRotate(new Vector3(0, -90, Random.Range(0, 360)), Random.Range(rotationLimits.x, rotationLimits.y), RotateMode.Fast).OnComplete(() =>
    //    {
    //        Invoke("RandomRotations", Random.Range(rotationTimeGaps.x, rotationTimeGaps.y));
            
    //    });
    //}
}
