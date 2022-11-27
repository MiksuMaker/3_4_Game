using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KS_ObjectPooler : MonoBehaviour
{
    public static KS_ObjectPooler KS_SharedInstance;
    public List<GameObject> pooledObjects;
    public GameObject objectToPool;
    public int amountToPool;

    [SerializeField] private GameObject playerObj;


    private void Awake()
    {
        KS_SharedInstance = this;
    }

    private void Start()
    {
        pooledObjects = new List<GameObject>();
        GameObject tmp;
        for(int i = 0; i < amountToPool; i++)
        {
            tmp = Instantiate(objectToPool);
            tmp.GetComponent<KS_Bullet>().SetPlayer(playerObj);
            tmp.SetActive(false);
            pooledObjects.Add(tmp);
        }
    }


    public GameObject GetPooledObject()
    {
        for(int i = 0; i < amountToPool; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }
        return null;
    }

}
