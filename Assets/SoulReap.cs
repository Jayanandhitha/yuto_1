using UnityEngine;

public class SoulReap : MonoBehaviour
{
    public float decayTime = 5f;
    public float corruptedLifeTime = 6f;
    public float chaseSpeed = 2f;
    public int damage = 1;

    public float reapDistance = 1.5f;

    public Sprite goodSpirit;
    public Sprite corruptedSpirit;

    private float timer;
    private float corruptedTimer;

    private bool corrupted = false;
    private bool destroyed = false;

    private PlayerMovement player;
    private SpriteRenderer sr;

    void Start()
    {
        timer = decayTime;

        sr = GetComponent<SpriteRenderer>();
        sr.sprite = goodSpirit;

        player = FindObjectOfType<PlayerMovement>();
    }

    void Update()
    {
        if (destroyed || player == null) return;

        if (!corrupted)
        {
            timer -= Time.deltaTime;

            if (timer <= 0f)
            {
                Corrupt();
            }

            float dist = Vector3.Distance(player.transform.position, transform.position);

            if (dist <= reapDistance && Input.GetKeyDown(KeyCode.Space))
            {
                Reap();
                return;
            }
        }
        else
        {
            corruptedTimer -= Time.deltaTime;

            if (corruptedTimer <= 0f)
            {
                destroyed = true;
                Destroy(gameObject);
                return;
            }

            ChasePlayer();
        }
    }

    void Reap()
    {
        if (player != null)
        {
            player.AddEssence();
        }

        destroyed = true;
        Destroy(gameObject);
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

        if (dist < 0.8f)
        {
            player.TakeDamage(damage);
        }
    }
}