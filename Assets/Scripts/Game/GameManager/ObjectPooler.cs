using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string name;
        public GameObject prefab;
        public int size;
    }

    public Dictionary<string, Queue<GameObject>> poolDictionary;
    public List<Pool> pools;

    #region Singleton
    protected static ObjectPooler _instance;

    public static ObjectPooler Instance
    {
        get
        {
            return _instance;
        }
    }

    void Awake()
    {
        _instance = this;
    }

    public void Init()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(_instance);
        }
        _instance = this;
    }
    #endregion

    // Use this for initialization
    void Start ()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> queue = new Queue<GameObject>();
            GameObject parent = Instantiate(new GameObject(pool.name + " parent"), gameObject.transform);
            for (int i = 0; i < pool.size; i++)
            {
                GameObject go = Instantiate(pool.prefab);
                go.SetActive(false);
                go.transform.parent = parent.transform;
                queue.Enqueue(go);
            }

            poolDictionary.Add(pool.name, queue);
        }
	}

    public GameObject SpawnFromPool(string name, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(name))
        {
            Debug.LogWarning("Pool with name " + name + " doesn't exist.");
            return null;
        }

        GameObject GOTospawn = poolDictionary[name].Dequeue();

        GOTospawn.SetActive(true);
        GOTospawn.transform.position = position;
        GOTospawn.transform.rotation = rotation;
        IPoolObject poolGO = GOTospawn.GetComponent<IPoolObject>();

        if (poolGO != null)
        {
            poolGO.OnGOSpawn();
        }
        
        poolDictionary[name].Enqueue(GOTospawn);

        return GOTospawn;
    }
	
}
