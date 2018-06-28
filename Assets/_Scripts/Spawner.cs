using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {
    public static Spawner instance;

	public int obstacleCount;
    public GameObject[] Walls;
    public Transform spawnParent;
    public Texture[] wallTextures;
    public Material wallMat;

    public Color[] WallColours;
    public Material[] wallMats;
    // Use this for initialization
    private void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update () {
		
	}

    [ContextMenu("Spawn")]
    void SetSpawn()
	{
        wallMat.mainTexture = wallTextures[Random.Range(0, wallTextures.Length)];
        float obstacleGap = 360 / obstacleCount;

        for (int i=0;i<obstacleCount; i++)
		{
            GameObject GO = Instantiate(Walls[Random.Range(0, Walls.Length)], transform);
            transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + obstacleGap, 0);
            GO.transform.SetParent(spawnParent);
            GO.transform.GetChild(0).localRotation = Quaternion.Euler(0, Random.Range(0,360), 0);

        }
	}

    public void SetRandomWallColor()
    {
        int temp = Random.Range(0, WallColours.Length);
        foreach (Material mat in wallMats)
            mat.color = WallColours[temp];
    }
}
