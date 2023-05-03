using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawn : MonoBehaviour
{
    //生成列表
    public List<GameObject> prefabs;

    private void Start()
    {
        InvokeRepeating(nameof(Spawner), Random.Range(0.2f, 0.5f), Random.Range(4f, 6f));
    }

    public void Spawner()
    {
        int index = Random.Range(0, prefabs.Count);
        var prefab = Instantiate(prefabs[index], transform);
    }
}
