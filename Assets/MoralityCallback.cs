using System;
using System.Collections;
using System.Collections.Generic;
using AOT;
using UnityEngine;

public partial class Morality : MonoBehaviour
{
    // These are the callbacks, whose pointer we'll send to javascript and is called by emscripten's Runtime.dynCall.
    public delegate void Int32CSharpCallback(int requestID, int outcome);
    public delegate void StringCSharpCallback(int requestID, string outcome);

    [MonoPInvokeCallback(typeof(Int32CSharpCallback))]
    private static void GlobalCallback(int requestID, int outcome)
    {
        if (callbacksBook.TryGetValue(requestID, out object callback))
        {
            (callback as Action<int>)?.Invoke(outcome);
        }

        // Remove this request from the tracker as it is done.
        callbacksBook.Remove(requestID);
    }

    [MonoPInvokeCallback(typeof(StringCSharpCallback))]
    private static void GlobalCallback(int requestID, string outcome)
    {
        if (callbacksBook.TryGetValue(requestID, out object callback))
        {
            (callback as Action<string>)?.Invoke(outcome);
        }
    
        // Remove this request from the tracker as it is done.
        callbacksBook.Remove(requestID);
    }

    /// <summary>
    /// Everytime a request is issued, give it the current id and increment this for next request.
    /// </summary>
    static int requestIDIncrementer = 0;

    /// <summary>
    /// Keeps track of pending callbacks by their id, once callback is received it is executed and removed from the book.
    /// </summary>
    static Dictionary<int, object> callbacksBook = new Dictionary<int, object>();

    private int NewRequest(object callback)
    {
        int requestID = requestIDIncrementer++;
        callbacksBook.Add(requestID, callback);

        return requestID;
    }
}