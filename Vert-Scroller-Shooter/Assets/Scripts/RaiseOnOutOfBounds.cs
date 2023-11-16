using RyanHipplesArchitecture.SO_Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Raises an event informing other systems that player is out of bounds.
/// Accounts for colliders merely being disabled for animation purposes
/// by having the extra "someColliderStillPresent" check.
/// </summary>
public class RaiseOnOutOfBounds : MonoBehaviour
{
    [SerializeField] private GameEvent_bool OnIsInBounds_false;

    [SerializeField] private bool someColliderStillPresent;


    private void FixedUpdate()
    {
        someColliderStillPresent = false;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        someColliderStillPresent = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (someColliderStillPresent)
        {
            return;
        }

        OnIsInBounds_false.Raise(false);
        //Debug.Log("OOB ev raised");
    }


}
