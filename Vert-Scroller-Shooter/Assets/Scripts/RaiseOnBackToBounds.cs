using RyanHipplesArchitecture.SO_Events;
using RyanHipplesArchitecture.SO_Variables;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Raises an event informing other systems that player is in bounds.
/// </summary>
public class RaiseOnBackToBounds : MonoBehaviour
{
    [SerializeField] private GameEvent_bool OnIsInBounds_true;
    [SerializeField] private IntVariable playerCharacterLayer;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer != playerCharacterLayer.Value) return;

        OnIsInBounds_true.Raise(true);
        //Debug.Log("BTB ev raised");

    }
}
