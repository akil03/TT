using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class RandomRotationCube : MonoBehaviour {
    public float turnDuration;
    public float speed;
    public bool isRandom;
	// Use this for initialization
	void Start () {
        if (isRandom)
            RandomRotation();
        else
            FixedRotate();

    }
	
	// Update is called once per frame
	void Update () {

        if (!isRandom)
            FixedRotate();

    }


    void RandomRotation()
    {

        transform.DORotate(new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360)), turnDuration, RotateMode.Fast).OnComplete(() =>
        {
            RandomRotation();
        });

    }


    void FixedRotate()
    {
        transform.Rotate(new Vector3(1, 1, 1) * speed);
    }
}
