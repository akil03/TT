using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {
    public static Spawner instance;

	public int obstacleCount;
    public GameObject[] Walls;
    public GameObject CoinWall;
    public GameObject[] CoinWalls;

    public Transform spawnParent;
    public Texture[] wallTextures;
    public Material wallMat;

    public Color[] WallColours;
    public Material[] wallMats;
    public int maxCoins;
    // Use this for initialization
    private void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update () {
		
	}

    [ContextMenu("SpawnWalls")]
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

    [ContextMenu("SpawnCoin")]
    void SetSpawnCoin()
    {

        float obstacleGap = 360 / obstacleCount;
        float startRotation = transform.rotation.eulerAngles.y;
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + obstacleGap/2, 0);

        for (int i = 0; i < obstacleCount; i++)
        {
            GameObject GO = Instantiate(CoinWall, transform);
            transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + obstacleGap, 0);
            GO.transform.SetParent(spawnParent);
            GO.transform.GetChild(0).localRotation = Quaternion.Euler(0, Random.Range(0, 360), 0);

        }
        transform.rotation = Quaternion.Euler(0, startRotation, 0);
    }

    public void SetCoinLocation()
    {
        int i = 0;
        int rand;
        foreach(GameObject go in CoinWalls)
        {
            if (i == maxCoins)
                break;

            print("coin assign");
            rand = Random.Range(0, 30);
            if (rand < 5)
            {
                go.SetActive(true);
                i++;
            }
            else
                go.SetActive(false);
            

        }
    }

    public void SetRandomWallColor()
    {
        int temp = Random.Range(0, WallColours.Length);
        foreach (Material mat in wallMats)
        mat.color = WallColours[temp];

        SetCoinLocation();
    }
}
