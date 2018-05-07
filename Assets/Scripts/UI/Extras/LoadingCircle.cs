﻿using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class LoadingCircle : MonoBehaviour
{
    private RectTransform rectComponent;
    public float rotateSpeed = -200f;

    private void Start()
    {
        rectComponent = GetComponent<RectTransform>();
    }

    private void Update()
    {
        rectComponent.Rotate(0f, 0f, rotateSpeed * Time.deltaTime);
    }
}