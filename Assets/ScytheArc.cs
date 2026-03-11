using UnityEngine;

public class ScytheArc : MonoBehaviour
{
    public float radius = 1.5f;
    public int segments = 20;
    public float arcAngle = 120f;
    public float lifeTime = 0.15f;

    LineRenderer lr;

    void Start()
    {
        lr = gameObject.AddComponent<LineRenderer>();

        lr.widthMultiplier = 0.1f;
        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.startColor = Color.white;
        lr.endColor = Color.white;

        lr.positionCount = segments + 1;

        float startAngle = -arcAngle / 2;

        for (int i = 0; i <= segments; i++)
        {
            float angle = startAngle + arcAngle * i / segments;
            float rad = angle * Mathf.Deg2Rad;

            Vector3 pos = new Vector3(Mathf.Sin(rad), Mathf.Cos(rad), 0) * radius;
            lr.SetPosition(i, pos);
        }

        Destroy(gameObject, lifeTime);
    }
}