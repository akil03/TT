using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRotate : MonoBehaviour {
    public float speed;
    int direction = 1;
	// Use this for initialization
	void Start () {
        speed = 50;
        if (Random.Range(0, 10) < 5)
            direction = -1;
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(Vector3.up * speed * direction* Time.deltaTime);
	}
}
