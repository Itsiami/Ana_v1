using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public float jumpForce;

    public Rigidbody2D rb;
    private Vector3 velocity = Vector3.zero;

    public bool isJumping;
    public bool isGrounded;

    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask collisionLayers;

    public Animator animator;
    public SpriteRenderer spriteRenderer;

    private float horizontalMovement;

    private void Update()
    {
        this.isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, collisionLayers);

        horizontalMovement = Input.GetAxis("Horizontal") * this.speed * Time.fixedDeltaTime;

        if (Input.GetButtonDown("Jump") && this.isGrounded == true)
        {
            this.isJumping = true;
        }

        Flip(rb.linearVelocity.x);

        float velociteJoueur = Mathf.Abs(rb.linearVelocity.x);
        this.animator.SetFloat("Speed", velociteJoueur);
    }

    void FixedUpdate()
    {
        MovePlayer(horizontalMovement);
    }

    void MovePlayer(float _mouvementHorizontal)
    {
        Vector3 velociteCible = new Vector2(_mouvementHorizontal, this.rb.linearVelocity.y);
        this.rb.linearVelocity = Vector3.SmoothDamp(rb.linearVelocity, velociteCible, ref this.velocity, .05f);

        if(this.isJumping == true) 
        {
            rb.AddForce(new Vector2(0f, this.jumpForce));
            this.isJumping = false;
        }
    }

    void Flip(float _velocite)
    {
        if(_velocite > 0.1f)
        {
            this.spriteRenderer.flipX = false;
        }else if(_velocite < -0.1f)
        {
            this.spriteRenderer.flipX = true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
