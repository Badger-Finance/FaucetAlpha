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
    private static extern void Mlty_Authenticate(int requestId, Action<int,bool> callback);

    public string server;
    public string project;
    
    // This is the callback, whose pointer we'll send to javascript and is called by emscripten's Runtime.dynCall.
    public delegate void LoginCSharpCallback(int requestID, bool outcome);

    /// <summary>
    /// Everytime a request is issued, give it the current id and increment this for next request.
    /// </summary>
    static int requestIDIncrementer = 0;

    /// <summary>
    /// Keeps track of pending callbacks by their id, once callback is received it is executed and removed from the book.
    /// </summary>
    static Dictionary<int, object> callbacksBook = new Dictionary<int, object>();



    [MonoPInvokeCallback(typeof(LoginCSharpCallback))]
    private static void GlobalCallback(int requestID, bool outcome)
    {
        if(callbacksBook.TryGetValue(requestID, out object callback))
        {
            (callback as Action<bool>)?.Invoke(outcome);
        }
        // Remove this request from the tracker as it is done.
        callbacksBook.Remove(requestID);
    }
    
    void Start()
    {
        // Example external code:
        this.Initialize(server, project);
        this.Authenticate((outcome)=>
        {
            Debug.Log("Login " + (outcome ? "Successful" : "Failed"));
        });
    }

    public void Initialize(string server, string project)
    {
        Mlty_Initialize(server, project);
    }

    public void Authenticate(Action<bool> callback)
    {
        int requestID = requestIDIncrementer++;
        callbacksBook.Add(requestID, callback);

        Mlty_Authenticate(requestID, GlobalCallback);
    }
}
