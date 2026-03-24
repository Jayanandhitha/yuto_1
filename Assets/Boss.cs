using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    public int health = 1;
    public float speed = 2f;

    public GameObject teleporter; // assign in inspector
    public int contactDamage = 1;
    public float damageInterval = 1f;

    private float damageTimer = 0f;
    private Transform player;
    public float knockbackForce = 1.5f;
    public float knockbackDuration = 0.15f;
    public Image healthFill;

    private int maxHealth;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        maxHealth = health;
    }

    void Update()
    {
        if (player == null) return;

        // Simple chase AI
        Vector3 dir = (player.position - transform.position).normalized;
        transform.position += dir * speed * Time.deltaTime;
    }
    

    void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        damageTimer += Time.deltaTime;

        if (damageTimer >= damageInterval)
        {
            PlayerMovement player = other.GetComponent<PlayerMovement>();

            if (player != null)
            {
                player.TakeDamage(contactDamage);
            }

            damageTimer = 0f;
        }
    }

    public void TakeDamage(int amount, Vector3 attackerPos)
    {
        health -= amount;
        UpdateHealthBar();
        Vector3 knockDir = (transform.position - attackerPos).normalized;
        StartCoroutine(SmoothKnockback(knockDir, knockbackForce));

        if (health <= 0)
        {
            Die();
        }
    }
    System.Collections.IEnumerator SmoothKnockback(Vector3 dir, float force)
    {
        float elapsed = 0f;

        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + dir * force;

        while (elapsed < knockbackDuration)
        {
            transform.position = Vector3.Lerp(startPos, endPos, elapsed / knockbackDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = endPos;
    }
    void UpdateHealthBar()
    {
        if (healthFill != null)
        {
            healthFill.fillAmount = (float)health / maxHealth;
        }
    }
    void Die()
    {
        if (teleporter != null)
        {
            teleporter.SetActive(true);
        }

        Destroy(gameObject);
    }
}