using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector3 lastVelocity;

    [Header("Player Movement")]
    [SerializeField] public float speed;
    [SerializeField] public float bounceMultiplier;

    //Debug section
    [Header("Debug section")]
    [SerializeField] public float jumpCharge;
    [SerializeField] public bool isGrounded;
    [SerializeField] public float rayCastDistance;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        lastVelocity = rb.velocity;

        GroundCheck();
        PlayerMovements();
    }

    void GroundCheck()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, rayCastDistance);
        isGrounded = hit.collider != null && hit.collider.tag == "Ground";
    }

    void PlayerMovements()
    {
        float horizontal = Input.GetAxis("Horizontal");

        if (!isGrounded)
        {
            jumpCharge = 5;
            return;
        }


        if (Input.GetKeyUp(KeyCode.Space) || jumpCharge >= 20)
        {
            jumpCharge = MathF.Min(jumpCharge, 20);
            rb.velocity = new Vector2(horizontal * speed, MathF.Floor(jumpCharge));
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

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (!isGrounded)
        {
            float currentVelocity = lastVelocity.magnitude;
            var direction = Vector3.Reflect(lastVelocity.normalized, coll.contacts[0].normal);

            rb.velocity = direction * (currentVelocity * bounceMultiplier);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * rayCastDistance);
    }
}
