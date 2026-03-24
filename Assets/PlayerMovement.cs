using UnityEngine;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;

    public int essence = 0;
    public int health = 5;

    public TextMeshProUGUI healthText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gameText;

    public float attackRange = 4f;
    public float attackAngle = 90f;
    public GameObject boss;       // drag boss prefab here
    public GameObject gate;       // drag gate object here
    private bool bossTriggered = false;
    public SoulSpawner spawner; // drag in inspector

    private SpriteRenderer sr;

    // Stores last movement direction for attacking
    private Vector3 lastMoveDir = Vector3.up;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        UpdateUI();
    }

    void Update()
    {
        Move();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
        }
    }

    void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        Vector3 move = new Vector3(x, y, 0);
        transform.position += move * speed * Time.deltaTime;

        if (move != Vector3.zero)
        {
            lastMoveDir = move.normalized;
        }
        if (x > 0)
            sr.flipX = false;
        else if (x < 0)
            sr.flipX = true;
    }

    void Attack()
    {
        GameObject arc = new GameObject("ScytheArc");
        arc.transform.parent = transform;
        arc.transform.localPosition = Vector3.up * 0.5f;
        arc.transform.up = lastMoveDir;
        arc.AddComponent<ScytheArc>();

        SoulReap[] souls = FindObjectsByType<SoulReap>(FindObjectsSortMode.None);

        Boss[] bosses = FindObjectsByType<Boss>(FindObjectsSortMode.None);

        foreach (Boss boss in bosses)
        {
            Vector3 dir = boss.transform.position - transform.position;

            if (dir.magnitude > attackRange)
                continue;

            float angle = Vector3.Angle(lastMoveDir, dir);

            if (angle <= attackAngle / 2f)
            {
                boss.TakeDamage(1, transform.position);
            }
        }

        foreach (SoulReap soul in souls)
        {
            Vector3 dir = soul.transform.position - transform.position;

            if (dir.magnitude > attackRange)
                continue;

            float angle = Vector3.Angle(lastMoveDir, dir);

            if (angle <= attackAngle / 2f)
            {
                soul.HitByScythe(transform.position);
            }
        }
    }

    void UpdateUI()
    {
        if (healthText != null)
            healthText.text = "Health: " + health;

        if (scoreText != null)
            scoreText.text = "Essence: " + essence;
    }

    public void AddEssence()
    {
        essence++;
        UpdateUI();
        if (essence >= 1 && !bossTriggered)
        {
            ActivateBossArea();
        }
    }

    public void TakeDamage(int amount)
    {
        health -= amount;

        sr.color = Color.red;
        Invoke(nameof(ResetColor), 0.2f);

        if (health <= 0)
        {
            if (gameText != null)
                gameText.text = "GAME OVER";

            Time.timeScale = 0f;
        }

        UpdateUI();
    }
    void ActivateBossArea()
    {
        bossTriggered = true;
        if (gate != null)
            gate.SetActive(false);
        if (boss != null)
            boss.SetActive(true);
        if (spawner != null)
            spawner.canSpawn = false;
        SoulReap[] souls = FindObjectsByType<SoulReap>(FindObjectsSortMode.None);
        foreach (SoulReap soul in souls)
        {
            Destroy(soul.gameObject);
        }
    }

    void ResetColor()
    {
        sr.color = Color.white;
    }
}