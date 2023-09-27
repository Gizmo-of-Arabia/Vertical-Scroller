using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ToggleResetHint : MonoBehaviour
{
    [SerializeField] private Animator textAnimator;


    public void DoToggleTextObject(bool isInBounds)
    {
        if (isInBounds)
        textAnimator.SetBool("IsInBounds", true);
        else
        textAnimator.SetBool("IsInBounds", false);
    }

}
