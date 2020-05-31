﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PathFactory : MonoBehaviour
{
    public static PathFactory Instance;

    public GameObject PathPrefab;
    public GameObject NodePrefab;

#if UNITY_EDITOR
    private void OnGUI()
    {
        Instance = this;
    }
    private void OnEnable()
    {
        Instance = this;
    }
#endif

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }
}
