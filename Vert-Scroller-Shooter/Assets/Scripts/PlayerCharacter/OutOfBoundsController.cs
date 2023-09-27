using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfBoundsController : MonoBehaviour
{

    public void DoOutOfBoundsNotice(bool val)
    {
        if (val)
        {
            // Debug.Log("BTB");
        } 
        else
        {
           // Debug.Log("OOB");
        }
    }
}
