using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallProperties : MonoBehaviour {
    public GameObject[] Walls;
    public bool isFirst,isLast;
    bool isSwitched = false;
    // Use this for initialization
    void Start () {
        //Reset();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Reset()
    {
        
            


        Invoke("ChangeWall", 0.55f);
    }

    public void ChangeWall()
    {
        if (isLast)
        {
            Spawner.instance.SetRandomWallColor();
            GameManager.instance.playerCam.RotateTest();
        }
            

        if (isFirst || isLast)
        {
            transform.GetChild(0).localRotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
            return;
        }

        if (isSwitched)
            return;

        isSwitched = true;
        GameObject GO = Instantiate(Walls[Random.Range(0, Walls.Length)], transform.parent);
        GO.transform.position = transform.position;
        GO.transform.rotation = transform.rotation;
        GO.transform.GetChild(0).localRotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
        Destroy(gameObject);
    }
}
