using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class TweenUI : MonoBehaviour {
    public Vector2 min, max;
    public float time;
	// Use this for initialization
	void Start () {
        DoMax();

    }
	
	// Update is called once per frame
	void Update () {
		
	}


    void DoMax()
    {
        transform.DOLocalMove(max, time, false).OnComplete(() =>
        {
            DoMin();
        });
    }


    void DoMin()
    {
        transform.DOLocalMove(min, time, false).OnComplete(() =>
        {
            DoMax();
        });
    }
}
