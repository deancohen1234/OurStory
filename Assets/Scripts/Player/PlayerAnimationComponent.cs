using UnityEngine;
using System;

public class PlayerAnimationComponent : MonoBehaviour
{
    public Animator Animator;
    public PlayerController PlayerController;

    private Vector3 NewLocalScale;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        NewLocalScale = transform.localScale;

        if (Animator == null)
        {
            return;
        }

        PlayerController.AddEventJump(OnJump);
    }

    // Update is called once per frame
    void Update()
    {
        if (Animator == null)
        {
            return;
        }

        bool isFalling = PlayerController.IsFalling();
        bool isGrounded = PlayerController.IsGrounded();
        bool isMoving = PlayerController.IsMoving();

        Animator.SetBool("IsFalling", isFalling);
        Animator.SetBool("IsGrounded", isGrounded);
        Animator.SetBool("IsMoving", isMoving);

        UpdateDirection();
        
    }

    private void UpdateDirection()
    {
        float sign = Mathf.Sign(PlayerController.GetVelocityDirection().x);

        NewLocalScale.x = sign == 0 ? 1 : sign;
        transform.localScale = NewLocalScale;
    }

    private void OnJump()
    {
        Animator.SetTrigger("Jump");
    }
}
