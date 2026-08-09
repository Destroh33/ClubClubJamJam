using UnityEngine;

public class OscilateOnY : MonoBehaviour
{
    [SerializeField] private Transform objectPos;
    private float originY;
    [SerializeField] private float amplitude = 0.1f;
    [SerializeField] private float speed = 1f;
    float timer = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        originY = transform.position.y;
    }

    private void OnEnable()
    {
        timer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        objectPos.position = new Vector2(objectPos.position.x, originY + Mathf.Sin(timer * speed) * amplitude);
    }
}
