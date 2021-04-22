using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    private Animator animator;
    private bool isAnimating = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetInteger("arms", 5);
            animator.SetInteger("legs", 5);
        }
        else
        {
            Debug.LogWarning("<Color=Yellow><a>Missing</a></Color> Animator reference on player Prefab.", this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isAnimating)
        {
            animator.SetInteger("arms", 1);
            animator.SetInteger("legs", 1);
            isAnimating = true;
        } 
        else if (Input.GetKeyDown(KeyCode.Space) && isAnimating)
        {
            animator.SetInteger("arms", 5);
            animator.SetInteger("legs", 5);
            isAnimating = false;
        }
    }
}
