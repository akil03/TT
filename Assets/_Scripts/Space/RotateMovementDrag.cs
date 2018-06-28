using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateMovementDrag : MonoBehaviour {
    public float threshold;
    public float speed;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetMouseButton(0))
		{
			if (Mathf.Abs(MouseHelper.mouseDelta.x) > threshold)
			{
				transform.Rotate(Vector3.forward * MouseHelper.mouseDelta.x * speed * Time.deltaTime);
                		
			}
		}
		


	}
}
