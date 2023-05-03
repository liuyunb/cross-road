using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TerrainManager : MonoBehaviour
{
    public float offsetY = 16f;

    public List<GameObject> newPlaces;
    
    //记录上一次序号，以防重复生成相同场景
    private int _lastIndex;

    private void Start()
    {
        EventsManager.GetPoint += CheckPos;
    }

    //检测是否需要生成新地图
    public void CheckPos(int point)
    {
        if (transform.position.y - Camera.main.transform.position.y < offsetY * 0.5f)
        {
            transform.position = new Vector3(0, Camera.main.transform.position.y + offsetY, 0);
            
            PutNewPlace();
        }
    }

    public void PutNewPlace()
    {
        var index = Random.Range(0, newPlaces.Count);

        while (index == _lastIndex)
        {
            index = Random.Range(0, newPlaces.Count);
        }

        _lastIndex = index;

        Instantiate(newPlaces[index], transform.position, Quaternion.identity);
    }
    
    private void OnDisable()
    {
        EventsManager.GetPoint -= CheckPos;
    }
    
}
