using RyanHipplesArchitecture.SO_Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Raises an event informing other systems that player is in bounds.
/// </summary>
public class RaiseOnBackToBounds : MonoBehaviour
{
    [SerializeField] private GameEvent_bool OnIsInBounds_true;


    private void OnTriggerEnter2D(Collider2D other)
    {
        OnIsInBounds_true.Raise(true);
        //Debug.Log("BTB ev raised");

    }
}
