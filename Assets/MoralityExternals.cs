using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using AOT;
using UnityEngine;

public partial class Morality : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void Mlty_Initialize(string server, string project);

    [DllImport("__Internal")]
    private static extern void Mlty_Authenticate(int requestId, Int32CSharpCallback callback);

    [DllImport("__Internal")]
    private static extern void Mlty_GetUserAttributeString(int requestId, StringCSharpCallback callback, string attribute);
    
    // [DllImport("__Internal")]
    // private static extern void Mlty_GetUserAttributeInt32(int requestId, Int32CSharpCallback callback);
}
