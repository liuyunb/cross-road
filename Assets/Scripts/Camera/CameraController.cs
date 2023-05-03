using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform frog;

    public float offsetY = 3;

    public float baseSize = 12;

    private float _ratio;

    private void Awake()
    {
        _ratio = Screen.height * 1.0f / Screen.width;

        Camera.main.orthographicSize = baseSize * _ratio * 0.5f;
    }

    private void LateUpdate()
    {
        transform.position = new Vector3(transform.position.x, frog.position.y + offsetY * _ratio, transform.position.z);
    }
}
