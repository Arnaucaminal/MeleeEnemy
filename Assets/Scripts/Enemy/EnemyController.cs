using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Animator animator;
    public Transform player;
    public float detectionRange = 5f;
    public float attackRange = 2f;
    public float moveSpeed = 2f;
    public int attackDamage = 1;
    public Collider2D attackCollider;
     private AudioSource audioSource;
     public AudioClip swordSound;
    [SerializeField] private AudioClip hurtSound;
    

    private bool isDead = false;
    private bool isFacingRight = true;

    private void Awake(){
         audioSource = GetComponent<AudioSource>();
    }
    private void Update()
    {
        if (isDead) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer < detectionRange && !isDead)
        {
            if (distanceToPlayer <= attackRange)
            {
                Attack();
            }
            else
            {
                ChasePlayer();
            }

            if (player.GetComponent<Health>().currentHealth <= 0)
            {
                Win();
                return;
            }
        }
        else
        {
            StopChasingPlayer();
        }
    }

    private void ChasePlayer()
    {
        animator.SetBool("IsWalking", true);
        MoveTowardsPlayer();
        LookAtPlayer();
    }

    private void StopChasingPlayer()
    {
        animator.SetBool("IsWalking", false);
    }

    private void MoveTowardsPlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(player.position.x, transform.position.y), moveSpeed * Time.deltaTime);
    }

    private void Attack()
    {
        animator.SetBool("IsWalking", false);
        animator.SetTrigger("IsAttacking");
        audioSource.PlayOneShot(swordSound);
        StartAttack();
    }

    private void LookAtPlayer()
    {
        var direction = player.position.x > transform.position.x ? 1 : -1;
        if ((direction > 0 && !isFacingRight) || (direction < 0 && isFacingRight))
        {
            Flip();
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) 
        {
            collision.GetComponent<Health>().TakeDamage(attackDamage);
        }
    }

    public void TakeDamage()
    {
        Debug.Log("Enemy takes damage"); 
        Die();
    }

    private void Die()
    {
        if (isDead) return;
        
        animator.SetTrigger("IsDead");
        audioSource.PlayOneShot(hurtSound);
        isDead = true;
        StopChasingPlayer();
        attackCollider.enabled = false;
         Invoke("DestroyObject", 1.0f); 
         
}

private void DestroyObject()
{
    Destroy(gameObject);
}

    private void StartAttack()
    {
        attackCollider.enabled = true; // Activa el colider de l'atac
    }

    private void Win()
    {
        animator.SetBool("IsWining", true);
        StopChasingPlayer();
    }
}
