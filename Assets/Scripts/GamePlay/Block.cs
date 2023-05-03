using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    private void OnEnable()
    {
        EventsManager.GetPoint += CheckPos;
    }

    public void CheckPos(int point)
    {
        if(Camera.main.transform.position.y - transform.position.y >= 30)
            Destroy(this.gameObject);
    }

    private void OnDisable()
    {
        EventsManager.GetPoint -= CheckPos;
    }
}
