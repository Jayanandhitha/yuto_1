using UnityEngine;

public class ScytheArc : MonoBehaviour
{
    public float radius = 3f;
    public int segments = 20;
    public float arcAngle = 120f;
    public float lifeTime = 0.15f;

    LineRenderer lr;

    void Start()
    {
        lr = gameObject.AddComponent<LineRenderer>();

        lr.useWorldSpace = false;   // Important
        lr.widthMultiplier = 0.12f;

        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.startColor = Color.white;
        lr.endColor = Color.white;

        lr.sortingOrder = 10; // Draw in front of player

        lr.positionCount = segments + 1;

        float startAngle = -arcAngle / 2f;

        for (int i = 0; i <= segments; i++)
        {
            float angle = startAngle + arcAngle * i / segments;
            float rad = angle * Mathf.Deg2Rad;

            Vector3 pos = new Vector3(Mathf.Sin(rad), Mathf.Cos(rad), 0) * radius;

            lr.SetPosition(i, pos);
        }

        transform.localPosition = Vector3.up * 0.5f;

        Destroy(gameObject, lifeTime);
    }
}