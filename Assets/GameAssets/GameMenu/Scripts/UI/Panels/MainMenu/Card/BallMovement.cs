using DG.Tweening;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    [SerializeField]
    private RectTransform boundary;
    [SerializeField]
    private float speed = 1000f;
    [SerializeField]
    private float weightFactor = 0.95f;
    [SerializeField]
    private float speedRotate = 300f;

    private Vector2 direction;
    private Vector2 minBound, maxBound;
    private RectTransform ball;

    public float scaleFactor = 0.95f; 
    public float scaleDuration = 0.15f;
    private bool isScaling = false;

    private void Awake()
    {
        ball = GetComponent<RectTransform>();
    }

    void Start()
    {
        direction = Random.insideUnitCircle.normalized;
        CalculateBounds();
    }

    private void CalculateBounds()
    {
        Vector2 size = boundary.rect.size * 0.5f;
        Vector2 ballSize = ball.rect.size * 0.5f;
        minBound = new Vector2(-size.x, -size.y) + ballSize;
        maxBound = new Vector2(size.x, size.y) - ballSize;
    }

    void Update()
    {
        MoveAndRotateBallBounce();
    }

    private void MoveAndRotateBallBounce()
    {
        if (!boundary.gameObject.activeSelf) return;

        Vector2 newPos = ball.anchoredPosition + direction * speed * /*Time.deltaTime*/Time.unscaledDeltaTime;
        bool hitX = false, hitY = false;

        if (newPos.x < minBound.x)
        {
            direction.x *= -1;
            newPos.x = minBound.x;
            hitX = true;
        }
        else if (newPos.x > maxBound.x)
        {
            direction.x *= -1;
            newPos.x = maxBound.x;
            hitX = true;
        }

        if (newPos.y < minBound.y)
        {
            direction.y *= -1;
            newPos.y = minBound.y;
            hitY = true;
        }
        else if (newPos.y > maxBound.y)
        {
            direction.y *= -1;
            newPos.y = maxBound.y;
            hitY = true;
        }

        if (hitX || hitY)
        {
            float angle = Random.Range(5f, 25f) * (Random.value > 0.5f ? 1 : -1);
            Vector2 newDirection = Quaternion.Euler(0, 0, angle) * direction;
            direction = Vector2.Lerp(direction, newDirection, weightFactor);
            direction.Normalize();
        }

        ball.anchoredPosition = newPos;

        ball.Rotate(Vector3.forward * speedRotate * /*Time.deltaTime*/Time.unscaledDeltaTime);
    }
}




