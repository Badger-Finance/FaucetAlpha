using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices;
using AOT;
using UnityEngine;

public partial class Morality : MonoBehaviour
{
    public static Morality Instance = null;

    public string server;
    public string project;
    public bool isInitialized = false;
    public bool isAuthenticated = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            DestroyImmediate(this);
        }
    }
    
    public void Initialize()
    {
        Mlty_Initialize(server, project);
        isInitialized = true;
    }

    public void Authenticate(Action<int> callback = null)
    {
        Mlty_Authenticate(NewRequest(callback), GlobalCallback);
        isAuthenticated = true;
    }

    public void GetUserArrtibute(Action<string> callback, string attribute)
    {
        Mlty_GetUserAttributeString(NewRequest(callback), GlobalCallback, attribute);
    }
}
