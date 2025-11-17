using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Move Properties")]
    public float m_MoveSpeed = 5.0f;
    public float m_MaxAcceleration = 5.0f;
    public float m_JumpAmount = 10.0f;
    public float m_FallMultiplier = 2.5f;
    public float m_LowJumpMultiplier = 2.0f;
    public float m_SlowFallGravity = -1.0f;
    public float m_HorizontalGlideSpeed = 0.2f;
    public float m_VerticalGlideSpeed = 0.1f;
    public float m_GroundedFrictionDrag = 0.9f;

    [Header("Wall Jump Properties")]
    public float m_WallSlideSpeed = 0.5f;
    public float m_WallJumpHorizonatlStrength = 3.0f;
    public float m_WallJumpVerticalStrength = 3.0f;
    public float m_WallJumpDrag = 0.95f; //x momentum drag when wall jumping
    public float m_WallJumpLerpTime = 1.0f;

    [Header("Wind Power")]
    public float m_WindPower = 10.0f;
    public float m_MagnitudeThreshold = 10.0f;
    [Range(0.0f, 1.0f)]
    public float m_StickOuterRimRadius = 0.5f;
    public float m_TimeToCharge = 0.75f;
    public bool m_UseAlternateControlScheme = false;

    [Header("Collider Properties")]
    public LayerMask m_GroundLayerMask;
    public LayerMask m_WallLayerMask;
    public Vector2 m_BottomColliderOffset;
    public Vector2 m_RightColliderOffset;
    public Vector2 m_LeftColliderOffset;
    public float m_SphereRadius = 0.1f;

    private Rigidbody2D m_Rigidbody;

    private float DesiredX;
    private float DesiredY;
    private bool bDesiresJump;

    private bool m_IsGrounded;
    private bool m_IsOnWall;
    private bool m_IsWallJumping;
    private bool m_CanMove = true;

    private bool m_OnRightWall;
    private bool playerIndexSet = false;

    //wind power global variables
    private float m_ChargeStartTime;

    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();

        Debug.Log("<color=red>Error: </color>AssetBundle not found");
    }

    // Update is called once per frame
    void Update()
    {
        //get  input
        DesiredX = Input.GetAxis("Horizontal");
        DesiredY = Input.GetAxis("Vertical");

        bDesiresJump |= Input.GetButtonDown("Jump");
    }

    private void FixedUpdate()
    {
        Vector2 direction = new Vector2(DesiredX, DesiredY);

        CalculateCollisions();
        CalculateFallingSpeed();


        if (m_CanMove)
        {
            Move(direction);
        }

        //add more friction if you are grounded and not moving
        if (m_IsGrounded && !bDesiresJump)
        {
            if (Mathf.Abs(DesiredX) <= 0.2f)
            {
                m_Rigidbody.linearVelocity *= m_GroundedFrictionDrag;
            }
        }

        //Jumping
        if (bDesiresJump)
        {
            bDesiresJump = false;
            if (m_IsGrounded)
            {
                Jump();
            }
        }
    }

    public bool IsStickOnOuterRim(Vector2 stickPosition)
    {
        float rimXDist = Mathf.Abs((stickPosition.normalized * m_StickOuterRimRadius).x);
        float rimYDist = Mathf.Abs((stickPosition.normalized * m_StickOuterRimRadius).y);

        if (rimXDist < Mathf.Abs(stickPosition.x) && rimYDist < Mathf.Abs(stickPosition.y))
        {
            //stick is in outer radius
            return true;
        }
        else
        {
            return false;
        }

    }

    private void Move(Vector2 moveDirection)
    {
        if (!m_IsOnWall)
        {
            float linearX = Mathf.MoveTowards(m_Rigidbody.linearVelocity.x, moveDirection.x * m_MoveSpeed, m_MaxAcceleration * Time.fixedDeltaTime);
            m_Rigidbody.linearVelocity = new Vector2(linearX, m_Rigidbody.linearVelocity.y);
        }
    }

    private void CalculateCollisions()
    {
        Collider2D groundCollider = Physics2D.OverlapCircle((Vector2)transform.position + m_BottomColliderOffset, m_SphereRadius, m_GroundLayerMask);
        Collider2D leftWallCollider = Physics2D.OverlapCircle((Vector2)transform.position + m_LeftColliderOffset, m_SphereRadius, m_WallLayerMask);
        Collider2D rightWallCollider = Physics2D.OverlapCircle((Vector2)transform.position + m_RightColliderOffset, m_SphereRadius, m_WallLayerMask);

        if (groundCollider == null) { m_IsGrounded = false; }
        else { m_IsGrounded = true; }

        if (leftWallCollider != null)
        {
            m_IsOnWall = true;
            m_OnRightWall = false;
        }
        else if (rightWallCollider != null)
        {
            m_IsOnWall = true;
            m_OnRightWall = true;
        }
        else
        {
            m_IsOnWall = false;
        }
    }

    private void CalculateFallingSpeed()
    {
        //if falling
        if (m_Rigidbody.linearVelocity.y < 0)
        {
            //- 1 to account for physics already applying 1 force of gravity
            m_Rigidbody.linearVelocity += Vector2.up * Physics2D.gravity.y * (m_FallMultiplier - 1) * Time.deltaTime;
        }
        //if we are rising
        //needs to be done because once player lets go of jump button harder gravity needs to be applied
        else if (m_Rigidbody.linearVelocity.y > 0 && !Input.GetButton("Jump"))
        {
            m_Rigidbody.linearVelocity += Vector2.up * Physics2D.gravity.y * (m_LowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    private void Jump()
    {
        m_Rigidbody.linearVelocity += new Vector2(m_Rigidbody.linearVelocity.x, m_JumpAmount);
    }

    private void WallJump()
    {
        //if on right wall then make velocity negative so you go left
        int onRightWall = (m_OnRightWall) ? -1 : 1;
        m_Rigidbody.linearVelocity = new Vector2(m_Rigidbody.linearVelocity.x + (m_WallJumpHorizonatlStrength * onRightWall), m_Rigidbody.linearVelocity.y + m_WallJumpVerticalStrength);

        StopCoroutine(DisableMovement(0.1f));
        StartCoroutine(DisableMovement(0.1f));

        m_IsWallJumping = true;
        m_IsOnWall = false;
    }

    private IEnumerator DisableMovement(float time)
    {
        m_CanMove = false;
        yield return new WaitForSeconds(time);
        m_CanMove = true;
    }

    private IEnumerator LowerGravity(float time)
    {
        Physics2D.gravity = new Vector2(0.0f, -0.1f);
        yield return new WaitForSeconds(time);
        Physics2D.gravity = new Vector2(0.0f, -9.81f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawWireSphere((Vector2)transform.position + m_BottomColliderOffset, m_SphereRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + m_RightColliderOffset, m_SphereRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + m_LeftColliderOffset, m_SphereRadius);
    }

    private void OnPlayerDeath()
    {
        m_Rigidbody.linearVelocity = Vector2.zero;
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 150, 90), "IsGrounded: " + m_IsGrounded + "\nIsOnWall: " + m_IsOnWall + "\nIsWallJumping: " + m_IsWallJumping + "\n<color=red>Error: </color>AssetBundle not found");
    }
}
