using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Move Properties")]
    public float m_MoveSpeed = 5.0f;
    public float m_MaxAcceleration = 5.0f;
    public float m_MaxAirAcceleration = 5.0f;
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

    [Header("Connections")]
    public Transform ConnectionOverride;

    private Rigidbody2D m_Rigidbody;
    private Transform m_ConnectedBody;

    private Vector2 m_ConnectedBodyWorldPosition;
    private Vector2 m_ConnectedBodyLocalPosition;
    private Vector2 m_ConnectionVelocity;

    private float DesiredX;
    private float DesiredY;
    private bool bDesiresJump;

    private bool m_IsGrounded;
    private bool m_WasGrounded;

    private bool m_IsOnWall;
    private bool m_IsWallJumping;
    private bool m_CanMove = true;
    private Vector2 m_GroundNormal = Vector2.up;

    private bool m_OnRightWall, m_OnLeftWall;


    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();

        if (ConnectionOverride != null)
        {
            m_ConnectedBody = ConnectionOverride;
            m_ConnectedBodyWorldPosition = transform.position;
            m_ConnectedBodyLocalPosition = m_ConnectedBody.InverseTransformPoint(m_ConnectedBodyWorldPosition);
        }
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

        ApplyConnectionForces();

        CalculateFallingSpeed();

        ApplyGravity();


        if (m_CanMove)
        {
            Move(direction);
        }


        //add more friction if you are grounded and not moving
        AddGroundedFriction();

        //Jumping
        if (bDesiresJump)
        {
            bDesiresJump = false;
            if (m_IsGrounded)
            {
                Jump();
            }
        }

        UpdatePlayerRotation();

        m_WasGrounded = m_IsGrounded;

        //Debug.DrawLine(transform.position, (Vector2)transform.position + m_Rigidbody.linearVelocity * 5f, Color.white);

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

    private void ApplyGravity()
    {
        m_Rigidbody.linearVelocity += m_GroundNormal * Physics2D.gravity.y * Time.fixedDeltaTime;

        Debug.DrawLine(transform.position, ((Vector2)transform.position + m_GroundNormal * Physics2D.gravity.y * Time.fixedDeltaTime), Color.magenta);


    }

    private void AddGroundedFriction()
    {
        if (m_IsGrounded && !bDesiresJump)
        {
            if (Mathf.Abs(DesiredX) <= 0.2f)
            {
                float newLinearX = m_Rigidbody.linearVelocity.x * m_GroundedFrictionDrag;
                m_Rigidbody.linearVelocity = new Vector2(newLinearX, m_Rigidbody.linearVelocity.y);
            }
        }
    }

    private void Move(Vector2 moveDirection)
    {
        if (moveDirection.x > 0 && m_OnRightWall)
        {
            moveDirection.x = 0;
            Debug.Log("Killing Right X");
        }
        else if (moveDirection.x < 0 && m_OnLeftWall)
        {
            moveDirection.x = 0;
            Debug.Log("Killing Left X");
        }

        Vector2 normalPerpendicular = Vector2.Perpendicular(m_GroundNormal);
        Vector2 projectedMoveDirection = normalPerpendicular * Vector2.Dot(normalPerpendicular, moveDirection);
        projectedMoveDirection = projectedMoveDirection.normalized;

        //float desiredLinearX = Mathf.MoveTowards(m_Rigidbody.linearVelocity.x, projectedMoveDirection.x * m_MoveSpeed * Time.fixedDeltaTime, m_MaxAcceleration * Time.fixedDeltaTime);
        float desiredLinearX = projectedMoveDirection.x * m_MoveSpeed * Time.fixedDeltaTime;
        //float desiredLinearY = Mathf.MoveTowards(m_Rigidbody.linearVelocity.y, projectedMoveDirection.y * m_MoveSpeed * Time.fixedDeltaTime, m_MaxAcceleration * Time.fixedDeltaTime);
        float desiredLinearY = projectedMoveDirection.y * m_MoveSpeed * Time.fixedDeltaTime;

        Vector2 desiredLinear = new Vector2(desiredLinearX, desiredLinearY);
        desiredLinear = Vector2.ClampMagnitude(desiredLinear, m_MoveSpeed);

        float newX = Mathf.MoveTowards(m_Rigidbody.linearVelocity.x, desiredLinear.x, GetAcceleration() * Time.fixedDeltaTime);
        float newY = Mathf.MoveTowards(m_Rigidbody.linearVelocity.y, desiredLinear.y, GetAcceleration() * Time.fixedDeltaTime);

        m_Rigidbody.linearVelocity += new Vector2((Mathf.Abs(projectedMoveDirection.x) * (newX - m_Rigidbody.linearVelocity.x)), (Mathf.Abs(projectedMoveDirection.y) * (newY - m_Rigidbody.linearVelocity.y)));

        Debug.DrawLine(transform.position, (Vector2)transform.position + new Vector2(m_Rigidbody.linearVelocity.x, m_Rigidbody.linearVelocity.y), Color.white);
        Debug.DrawLine(transform.position, (Vector2)transform.position + new Vector2((Mathf.Abs(projectedMoveDirection.x) * (newX - m_Rigidbody.linearVelocity.x)), (Mathf.Abs(projectedMoveDirection.y) * (newY - m_Rigidbody.linearVelocity.y))), Color.blue);
    }

    private void ApplyConnectionForces()
    {
        if (m_ConnectedBody != null && m_IsGrounded)
        {
            // Just landed this frame — initialize but DO NOT move
            if (!m_WasGrounded)
            {
                m_ConnectedBodyLocalPosition =
                   m_ConnectedBody.InverseTransformPoint(m_Rigidbody.position);

                m_ConnectedBodyWorldPosition = m_Rigidbody.position;

                return;
            }

            Vector2 connectionMovement = (Vector2)m_ConnectedBody.transform.TransformPoint(m_ConnectedBodyLocalPosition) - m_ConnectedBodyWorldPosition;
            m_ConnectionVelocity = connectionMovement / Time.deltaTime;

            Vector2 newWorldPos = m_ConnectedBody.TransformPoint(m_ConnectedBodyLocalPosition);

            Vector2 platformDelta = newWorldPos - m_ConnectedBodyWorldPosition;

            Debug.DrawLine(transform.position, (Vector2)transform.position + platformDelta, Color.red);

            //Vector2 relativeVelocity = m_ConnectionVelocity - m_Rigidbody.linearVelocity;

            m_Rigidbody.position += platformDelta;

            m_ConnectedBodyWorldPosition = newWorldPos;

            m_ConnectedBodyLocalPosition = m_ConnectedBody.InverseTransformPoint(m_ConnectedBodyWorldPosition);
        }
        else
        {
            //if there is no connected body, then we have functionally 0 relative velocity
            m_ConnectionVelocity = m_Rigidbody.linearVelocity;
        }

        
    }

    private void UpdatePlayerRotation()
    {
        float zRotation = Vector2.SignedAngle(Vector2.up, m_GroundNormal);
        //PlayerParent.rotation = Quaternion.Euler(0, 0, zRotation);

        m_Rigidbody.MoveRotation(zRotation);
    }

    private void CalculateCollisions()
    {
        Collider2D groundCollider = Physics2D.OverlapCircle((Vector2)transform.position + m_BottomColliderOffset, m_SphereRadius, m_GroundLayerMask);
        Collider2D leftWallCollider = Physics2D.OverlapCircle((Vector2)transform.position + m_LeftColliderOffset, m_SphereRadius, m_WallLayerMask);
        Collider2D rightWallCollider = Physics2D.OverlapCircle((Vector2)transform.position + m_RightColliderOffset, m_SphereRadius, m_WallLayerMask);

        if (groundCollider == null) { m_IsGrounded = false; }
        else 
        { 
            m_IsGrounded = true;
            m_ConnectedBody = groundCollider.transform;
        }

        if (leftWallCollider != null)
        {
            m_IsOnWall = true;
            m_OnLeftWall = false;
        }
        else if (rightWallCollider != null)
        {
            m_IsOnWall = true;
            m_OnRightWall = true;
        }
        else
        {
            m_IsOnWall = false;
            m_OnRightWall = false;
            m_OnLeftWall = false;
        }


        RaycastHit2D Hit = Physics2D.Raycast(transform.position, Vector2.down, 2, m_GroundLayerMask);

        //TODO remove magic number
        if (Hit.normal != Vector2.zero && Vector2.Dot(Hit.normal, Vector2.down) > 0.6f)
        {
            m_GroundNormal = Hit.normal;
        }
    }

    private void CalculateFallingSpeed()
    {
        //if falling
        if (m_Rigidbody.linearVelocity.y < 0 && !m_IsGrounded)
        {
            //- 1 to account for physics already applying 1 force of gravity
            m_Rigidbody.linearVelocity += m_GroundNormal * Physics2D.gravity.y * (m_FallMultiplier - 1) * Time.deltaTime;
        }
        //if we are rising
        //needs to be done because once player lets go of jump button harder gravity needs to be applied
        else if (m_Rigidbody.linearVelocity.y > 0 && !Input.GetButton("Jump") && !m_IsGrounded)
        {
            m_Rigidbody.linearVelocity += m_GroundNormal * Physics2D.gravity.y * (m_LowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    private void Jump()
    {
        m_Rigidbody.linearVelocity += new Vector2(m_Rigidbody.linearVelocity.x, m_JumpAmount);
    }

    private float GetAcceleration()
    {
        if (m_IsGrounded)
        {
            return m_MaxAcceleration;
        }
        else
        {
            return m_MaxAirAcceleration;
        }
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
        GUI.Label(new Rect(10, 10, 150, 90), "IsGrounded: " + m_IsGrounded + "\nIsOnWall: " + m_IsOnWall + "\nIsWallJumping: " + m_IsWallJumping + "\n<color=red>Error: </color>AssetBundle not found" + "\nIsOnLeftWall: " + m_OnLeftWall + "\nIsOnRightWall: " + m_OnRightWall);
    }
}
