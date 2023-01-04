using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunScriptOnClick : MonoBehaviour
{
    private MouseController mouseController;

    public void onClick()
    {
        mouseController.enabled = true;
    }
}