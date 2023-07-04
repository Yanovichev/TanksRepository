using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowLevelTextManager : MonoBehaviour
{
    private void OnEnable()
    {
        Animator animator = GetComponent<Animator>();
        animator.Play("Delay");
    }

}
