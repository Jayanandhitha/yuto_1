using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public int essence = 0;
    public int health = 5;

    public float invincibilityDuration = 1.5f;

    private bool isInvincible = false;
    private float invincibilityTimer;
    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(x, y, 0);
        transform.position += move * speed * Time.deltaTime;

        if (isInvincible)
        {
            invincibilityTimer -= Time.deltaTime;

            if (invincibilityTimer <= 0)
            {
                isInvincible = false;
                sr.color = Color.white;
            }
        }
    }

    public void AddEssence()
    {
        essence++;
        Debug.Log("Essence: " + essence);
    }

    public void TakeDamage(int amount)
    {
        if (isInvincible)
            return;

        health -= amount;
        Debug.Log("Player hit! Health: " + health);

        isInvincible = true;
        invincibilityTimer = invincibilityDuration;
        sr.color = new Color(1f, 1f, 1f, 0.5f);

        if (health <= 0)
        {
            Debug.Log("Player died");
        }
    }
}