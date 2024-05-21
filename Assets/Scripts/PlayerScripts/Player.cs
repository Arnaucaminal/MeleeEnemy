using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private LayerMask terrainLayer;
    [SerializeField] private LayerMask obstacleLayer;
    private Rigidbody2D rb2D;
    private Animator animator;
    private BoxCollider2D boxCollider2D;
    private float cooldownWallJump;
    private float inputHorizontal;
    private AudioSource audioSource;
    public AudioClip jumpSound;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        inputHorizontal = Input.GetAxis("Horizontal");

        if (inputHorizontal > 0.01f)
            transform.localScale = Vector3.one;
        else if (inputHorizontal < -0.01f)
            transform.localScale = new Vector3(-1, 1, 1);

        animator.SetBool("run", inputHorizontal != 0);
        animator.SetBool("grounded", CheckIfGrounded());

        if (cooldownWallJump > 0.2f)
        {
            rb2D.velocity = new Vector2(inputHorizontal * movementSpeed, rb2D.velocity.y);

            if (CheckIfOnWall() && !CheckIfGrounded())
            {
                rb2D.gravityScale = 0;
                rb2D.velocity = Vector2.zero;
            }
            else
                rb2D.gravityScale = 7;

            if (Input.GetKey(KeyCode.Space))
                PerformJump();
        }
        else
            cooldownWallJump += Time.deltaTime;
    }

    private void PerformJump()
    {
        if (CheckIfGrounded())
        {
            rb2D.velocity = new Vector2(rb2D.velocity.x, jumpForce);
            animator.SetTrigger("jump");
            audioSource.PlayOneShot(jumpSound);
        }
        else if (CheckIfOnWall() && !CheckIfGrounded())
        {
            if (inputHorizontal == 0)
            {
                rb2D.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 10, 0);
                transform.localScale = new Vector3(-Mathf.Sign(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else
                rb2D.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 3, 6);

            cooldownWallJump = 0;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
    }

    private bool CheckIfGrounded()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0, Vector2.down, 0.1f, terrainLayer);
        return hit.collider != null;
    }
    private bool CheckIfOnWall()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, obstacleLayer);
        return hit.collider != null;
    }

    public bool CanPerformAttack()
    {
        return CheckIfGrounded() && !CheckIfOnWall() && inputHorizontal == 0;
    }
}
