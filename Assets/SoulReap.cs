using UnityEngine;

public class SoulReap : MonoBehaviour
{
    public float decayTime = 5f;
    public float corruptedLifeTime = 10f;

    public float chaseSpeed = 3f;

    public int damage = 1;

    public Sprite goodSpirit;
    public Sprite corruptedSpirit;

    public float knockbackForce = 2f;
    public float playerHitKnockback = 0.8f;

    public float damageInterval = 1f;

    private float timer;
    private float corruptedTimer;

    private bool corrupted = false;

    private PlayerMovement player;
    private SpriteRenderer sr;

    private int hitCount = 0;
    private float damageTimer = 0f;

    void Start()
    {
        timer = decayTime;
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = goodSpirit;
        player = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if (player == null) return;
        if (!corrupted)
        {
            timer -= Time.deltaTime;

            if (timer <= 0f)
            {
                Corrupt();
            }
        }
        else
        {
            corruptedTimer -= Time.deltaTime;
            if (corruptedTimer <= 0f)
            {
                Destroy(gameObject);
                return;
            }

            ChasePlayer();
        }
    }

    void Corrupt()
    {
        corrupted = true;
        corruptedTimer = corruptedLifeTime;
        sr.sprite = corruptedSpirit;
    }

    void ChasePlayer()
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;

        transform.position += direction * chaseSpeed * Time.deltaTime;

        float dist = Vector3.Distance(player.transform.position, transform.position);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (!corrupted) return;

        if (other.CompareTag("Player"))
        {
            damageTimer += Time.deltaTime;

            if (damageTimer >= damageInterval)
            {
                player.TakeDamage(damage);

                Vector3 knockDir = (transform.position - player.transform.position).normalized;

                StartCoroutine(SmoothKnockback(knockDir, playerHitKnockback));

                damageTimer = 0f;
            }
        }
    }

    public void HitByScythe(Vector3 playerPos)
    {
        if (!corrupted)
        {
            player.AddEssence();
            Destroy(gameObject);
            return;
        }

        hitCount++;

        Vector3 knockDir = (transform.position - playerPos).normalized;

        StartCoroutine(SmoothKnockback(knockDir, knockbackForce));

        if (hitCount >= 3)
        {
            player.AddEssence();
            Destroy(gameObject);
        }
    }
    System.Collections.IEnumerator SmoothKnockback(Vector3 dir, float force)
    {
        float duration = 0.15f;
        float elapsed = 0f;

        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + dir * force;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(startPos, endPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = endPos;
    }
}