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

    public float attackRange = 2f;
    public float attackAngle = 90f;

    public GameObject attackArcPrefab;

    private SpriteRenderer sr;

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

        UpdateUI();
    }

    void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(x, y, 0);
        transform.position += move * speed * Time.deltaTime;

        if (move != Vector3.zero)
            transform.up = move;
    }

    void Attack()
{
    // Create the silver arc slash using code
    GameObject arc = new GameObject("ScytheArc");
    arc.transform.position = transform.position;
    arc.transform.rotation = transform.rotation;
    arc.AddComponent<ScytheArc>();

    // Find all souls in the scene
    SoulReap[] souls = FindObjectsByType<SoulReap>(FindObjectsSortMode.None);

    foreach (SoulReap soul in souls)
    {
        Vector3 dir = soul.transform.position - transform.position;

        // Check attack range
        if (dir.magnitude > attackRange)
            continue;

        // Check if soul is inside the attack cone
        float angle = Vector3.Angle(transform.up, dir);

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
    }

    public void TakeDamage(int amount)
    {
        health -= amount;

        sr.color = Color.red;
        Invoke("ResetColor", 0.2f);

        if (health <= 0)
        {
            gameText.text = "GAME OVER";
            Time.timeScale = 0f;
        }

        UpdateUI();
    }

    void ResetColor()
    {
        sr.color = Color.white;
    }
}