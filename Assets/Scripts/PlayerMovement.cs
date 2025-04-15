using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //DEPLACEMENT
    public float speed = 8f;
    private float horizontalMovement;

    // COYOTE TIME
    public float coyoteTime = 0.2f;
    private float coyoteTimeCounter;

    //SAUT
    public float jumpForce = 520f;
    public float fallMultiplier = 3f;
    public float lowJumpMultiplier = 3.5f;
    public bool isJumping;
    public bool isGrounded;
    private float jumpTimeCounter;
    public float maxJumpTime = 0.28f;
    private float jumpCooldown = 0.1f;
    private float jumpCooldownTimer;

    //DETECTION DU SOL
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask collisionLayers;

    //REFERENCES
    public Rigidbody2D rb;
    public Animator animator;
    public SpriteRenderer spriteRenderer;

    

    private void Update()
    {
        if (jumpCooldownTimer > 0)
        {
            jumpCooldownTimer -= Time.deltaTime;
        }

        this.isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, collisionLayers);

        // COYOTE TIME
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        // MOUVEMENT
        horizontalMovement = Input.GetAxis("Horizontal");


        // SAUT
        if (Input.GetButtonDown("Jump") && coyoteTimeCounter > 0f && jumpCooldownTimer <= 0)
        {
            isGrounded = false;
            this.isJumping = true;
            jumpTimeCounter = maxJumpTime;
            coyoteTimeCounter = 0f;
            jumpCooldownTimer = jumpCooldown; // Pas de nouveau saut immédiat
        }

        Flip(rb.linearVelocity.x);

        float velociteJoueur = Mathf.Abs(rb.linearVelocity.x);
        this.animator.SetFloat("Speed", velociteJoueur);
    }

    void FixedUpdate()
    {
        
        MovePlayer(horizontalMovement);
        BetterJump();
    }

    private void BetterJump()
    {
        // Le joueur tombe : on renforce la gravité
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
        // Le joueur monte, mais a relâché Jump
        else if (rb.linearVelocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }
        
        // Le joueur maintient le saut, mais on le force à s’arrêter après maxJumpTime
        if (Input.GetButton("Jump") && jumpTimeCounter > 0)
        {
            jumpTimeCounter -= Time.fixedDeltaTime;
        }
        else if (Input.GetButton("Jump") && jumpTimeCounter <= 0)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }
    }

    void MovePlayer(float _mouvementHorizontal)
    {
        rb.linearVelocity = new Vector2(_mouvementHorizontal * speed, rb.linearVelocity.y);

        if (isJumping) 
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
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
