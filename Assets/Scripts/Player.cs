using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector3 lastVelocity;
    private SpriteRenderer spriteRenderer;
    private Vector3 rightGroundRaycastOffset = new Vector3(0.77f, 0f, 0f);
    private Vector3 leftGroundRaycastOffset = new Vector3(-0.77f, 0f, 0f);
    private Vector3 facingBlockRaycastOffset = new Vector3(0f, -0.2f, 0f);
    private float horizontal;
    private bool isFacingRight = true;
    private bool isFacingBlock = false;

    [Header("Player Movement")]
    [SerializeField] public float speed;
    [SerializeField] public float bounceMultiplier;
    [SerializeField] public float maxVelocity;

    //Debug section
    [Header("Debug section")]
    [SerializeField] public float jumpCharge;
    [SerializeField] public bool isGrounded;
    [SerializeField] public float rayCastDistanceGround;
    [SerializeField] public float rayCastDistanceFacingBlock;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        //Set a max fall speed
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxVelocity);
    }

    void Update()
    {
        lastVelocity = rb.velocity;

        PlayerMovements();
        GroundCheck();
        Flip();
        facingBlockCheck();
    }

    //Flip the player sprite
    void Flip()
    {
        if ((isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f) && isGrounded)
        {
            isFacingRight = !isFacingRight;
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }
    }

    void GroundCheck()
    {
        RaycastHit2D hitMiddle = Physics2D.Raycast(transform.position, Vector2.down, rayCastDistanceGround);
        bool groundedMiddle = hitMiddle.collider != null && hitMiddle.collider.tag == "Ground";

        RaycastHit2D hitRight = Physics2D.Raycast(transform.position + rightGroundRaycastOffset, Vector2.down, rayCastDistanceGround);
        bool groundedRight = hitRight.collider != null && hitRight.collider.tag == "Ground";

        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position + leftGroundRaycastOffset, Vector2.down, rayCastDistanceGround);
        bool groundedLeft = hitLeft.collider != null && hitLeft.collider.tag == "Ground";

        isGrounded = groundedMiddle || groundedRight || groundedLeft;
    }

    void PlayerMovements()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        if (!isGrounded)
        {
            jumpCharge = 5;
            return;
        }


        if (Input.GetKeyUp(KeyCode.Space) || jumpCharge >= 20)
        {
            jumpCharge = MathF.Min(jumpCharge, 20);

            //If the player is facing a block, the player will jump in the opposite 
            //direction at a slower speed and slightly different angle
            if (isFacingBlock)
            {
                rb.velocity = new Vector2(-horizontal / 1.3f * speed, MathF.Floor(jumpCharge));
            }
            else
            {
                rb.velocity = new Vector2(horizontal * speed, MathF.Floor(jumpCharge));
            }

            isGrounded = false;
        }
        else if (Input.GetKey(KeyCode.Space))
        {
            rb.velocity = new Vector2(0, 0);
            jumpCharge += Time.deltaTime * 15;
        }

        if (jumpCharge == 5)
        {
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        }
    }

    //Rebound the player when it hits a wall
    void OnCollisionEnter2D(Collision2D coll)
    {
        GroundCheck();

        if (!isGrounded)
        {
            float currentVelocity = lastVelocity.magnitude;
            Vector3 direction;

            //If the player hits his head on the ceiling
            //The coll.contacts[0].normal.y allways return -1 when the player hits his head
            if (coll.contacts[0].normal.y == -1)
            {
                direction = Vector3.Reflect(lastVelocity.normalized, coll.contacts[0].normal) + new Vector3(0, 0.5f, 0);
            }
            else
            {
                direction = Vector3.Reflect(lastVelocity.normalized, coll.contacts[0].normal);
            }

            rb.velocity = direction * (currentVelocity * bounceMultiplier);
        }
    }

    //Check if the player is facing a block
    bool facingBlockCheck()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + facingBlockRaycastOffset, isFacingRight ? Vector2.right : Vector2.left, rayCastDistanceFacingBlock);
        isFacingBlock = hit.collider != null && hit.collider.tag == "Ground";
        return isFacingBlock;
    }


    //Draw the rays in the editor
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector2.down * rayCastDistanceGround);
        Gizmos.DrawRay(transform.position + rightGroundRaycastOffset, Vector2.down * rayCastDistanceGround);
        Gizmos.DrawRay(transform.position + leftGroundRaycastOffset, Vector2.down * rayCastDistanceGround);

        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position + facingBlockRaycastOffset, (isFacingRight ? Vector3.right : Vector3.left) * rayCastDistanceFacingBlock);
    }
}
