using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : MonoBehaviour
{
    // //方向
    // private float _dir;
    //行进速度
    public float speed = 5;
    //销毁点距离
    public float destroyPos = 30;
    //初始点
    private Vector2 _startPos;

    private void Start()
    {
        _startPos = transform.position;
    }

    private void Update()
    {
        if(MathF.Abs(transform.position.x - _startPos.x) > destroyPos)
            Destroy(this.gameObject);
        Move();
    }

    public void Move()
    {
        transform.position += transform.right * speed * Time.deltaTime * transform.lossyScale.x;
    }
}
