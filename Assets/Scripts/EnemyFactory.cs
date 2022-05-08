using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFactory : MonoBehaviour
{
    // Assets>Resources 이후 경로(path) 지정
    public const string EnemyPath = "Prefabs/Enemy";

    Dictionary<string, GameObject> EnemyFileCache = new Dictionary<string, GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject Load(string resourcePath)
    {
        GameObject go = null;

        if(EnemyFileCache.ContainsKey(resourcePath))
        {
            go = EnemyFileCache[resourcePath];
        }
        else
        {
            go = Resources.Load<GameObject>(resourcePath);
            if(!go)
            {
                Debug.LogError("Load error! path = " + resourcePath);
            }

            EnemyFileCache.Add(resourcePath, go);
        }

        Debug.Log("EnemyFileCache -> " + EnemyFileCache);

        GameObject InstancedGo = Instantiate<GameObject>(go);

        return InstancedGo;
    }
}
