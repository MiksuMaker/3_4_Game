using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    #region PROPERTIES
    [SerializeField] int floor = 0;
    float height;

    Animator animator;
    #endregion

    #region BUILTIN
    private void Start()
    {
        // Cache height
        height = transform.position.y;

        // Get references
        animator = GetComponentInChildren<Animator>();
    }
    #endregion
}
