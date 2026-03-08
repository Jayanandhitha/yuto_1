using UnityEngine;

public class SoulReap : MonoBehaviour
{
    public float decayTime = 5f;
    public float corruptedLifeTime = 6f;
    public float chaseSpeed = 2f;
    public int damage = 1;

    public Sprite goodSpirit;     // normal sprite
    public Sprite corruptedSpirit; // red sprite

    private float timer;
    private float corruptedTimer;
    private bool playerNear = false;
    private bool corrupted = false;

    private PlayerMovement player;
    private SpriteRenderer sr;

    void Start()
    {
        timer = decayTime;
        sr = GetComponent<SpriteRenderer>();

        sr.sprite = goodSpirit; // start as normal spirit
    }

    void Update()
    {
        if (!corrupted)
        {
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                Corrupt();
            }
        }
        else
        {
            corruptedTimer -= Time.deltaTime;

            if (corruptedTimer <= 0)
            {
                Destroy(gameObject);
            }

            if (player != null)
            {
                ChasePlayer();
            }
        }

        if (playerNear && Input.GetKeyDown(KeyCode.E) && !corrupted)
        {
            player.AddEssence();
            Destroy(gameObject);
        }
    }

    void Corrupt()
    {
        corrupted = true;
        corruptedTimer = corruptedLifeTime;

        sr.sprite = corruptedSpirit; // swap sprite
    }

    void ChasePlayer()
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;
        transform.position += direction * chaseSpeed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = true;
            player = other.GetComponent<PlayerMovement>();

            if (corrupted)
            {
                player.TakeDamage(damage);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = false;
        }
    }
}