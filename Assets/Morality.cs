using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices;
using AOT;
using UnityEngine;

public class Morality : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void Mlty_Initialize(string server, string project);

    [DllImport("__Internal")]
    private static extern void Mlty_Authenticate(int requestId, LoginCSharpCallback callback);

    [DllImport("__Internal")]
    private static extern void Mlty_WalletAddress(int requestId, WalletAddressCSharpCallback callback);
    
    // This is the callback, whose pointer we'll send to javascript and is called by emscripten's Runtime.dynCall.
    public delegate void LoginCSharpCallback(int requestID, int outcome);    
    public delegate void WalletAddressCSharpCallback(int requestID, string outcome);

    /// <summary>
    /// Everytime a request is issued, give it the current id and increment this for next request.
    /// </summary>
    static int requestIDIncrementer = 0;

    /// <summary>
    /// Keeps track of pending callbacks by their id, once callback is received it is executed and removed from the book.
    /// </summary>
    static Dictionary<int, object> callbacksBook = new Dictionary<int, object>();

    public static Morality Instance = null;

    public string server;
    public string project;
    public bool isInitialized = false;
    public bool isAuthenticated = false;
    
    [MonoPInvokeCallback(typeof(LoginCSharpCallback))]
    private static void GlobalCallback(int requestID, int outcome)
    {
        if(callbacksBook.TryGetValue(requestID, out object callback))
        {
            (callback as Action<int>)?.Invoke(outcome);
        }
        // Remove this request from the tracker as it is done.
        callbacksBook.Remove(requestID);
    }

    [MonoPInvokeCallback(typeof(WalletAddressCSharpCallback))]
    private static void GlobalCallback(int requestID, string outcome)
    {
        if(callbacksBook.TryGetValue(requestID, out object callback))
        {
            (callback as Action<string>)?.Invoke(outcome);
        }
        // Remove this request from the tracker as it is done.
        callbacksBook.Remove(requestID);
    }
    
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

    public void GetWalletAddress(Action<string> callback)
    {
        Mlty_WalletAddress(NewRequest(callback), GlobalCallback);
    }

    private int NewRequest(object callback)
    {
        int requestID = requestIDIncrementer++;
        callbacksBook.Add(requestID, callback);

        return requestID;
    }
}
