using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour
{
    public GameObject objectSpawn;
    public int count;
    public Transform parent;
    public Transform spawnRegion;
    public bool respawn = false;

    public bool Player = false;
    public GameObject dataBase;

    private void Start()
    {
     
        for (int i = 0; i < count; i++)
        {
            CreateObject();
        }
     
    }

    private void Update()
    {
        if (respawn)
        {
            if (parent.childCount < count)
            {
                CreateObject();
            }
        }
    }

    public void CountCreateObject(int count)
    {
        for (int i = 0; i < count; i++)
        {
            CreateObject();
        }
    }


    public void CreateObject()
    {
        float positionX = Random.Range(spawnRegion.position.x - spawnRegion.localScale.x/2, spawnRegion.position.x + spawnRegion.localScale.x/2);
        float positionY = Random.Range(spawnRegion.position.y - spawnRegion.localScale.y/2, spawnRegion.position.y + spawnRegion.localScale.y/2);

        Instantiate(objectSpawn, new Vector2(positionX, positionY), new Quaternion(0f, 0f, 0f, 0f), parent);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(spawnRegion.position, spawnRegion.localScale);

    }
}
