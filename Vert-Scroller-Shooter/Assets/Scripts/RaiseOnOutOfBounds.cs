using RyanHipplesArchitecture.SO_Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Raises an event informing other systems that player is out of bounds.
/// </summary>
public class RaiseOnOutOfBounds : MonoBehaviour
{
    [SerializeField] private GameEvent_bool OnIsInBounds_false;




    private void OnTriggerExit2D(Collider2D other)
    {

        OnIsInBounds_false.Raise(false);
        //Debug.Log("OOB ev raised");
    }


}
