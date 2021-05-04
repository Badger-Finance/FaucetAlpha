using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class LoadMetamask : MonoBehaviour
{
    [SerializeField] InputField input;
    
    public void InputMetamaskWalletAddress()
    {
        if (!Morality.Instance.isInitialized)
        {
            Morality.Instance.Initialize();
        }

        if (!Morality.Instance.isAuthenticated)
        {
            Morality.Instance.Authenticate((outcome) =>
            {
                if (outcome != 0)
                {
                    CallbackExecution();
                }
            });
        }
        else
        {
            CallbackExecution();
        }
    }

    private void CallbackExecution()
    {
        Morality.Instance.GetWalletAddress((address) =>
        {
            input.text = address;
        });
    }
}


