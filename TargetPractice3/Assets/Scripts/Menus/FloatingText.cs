using TMPro;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    public float moveUpSpeed = 30f;
    public float fadeDuration = 1f;

    private TMP_Text text;
    private RectTransform rectTransform;
    private float timer;

    void Awake()
    {
        text = GetComponent<TMP_Text>();
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        float delta = Time.deltaTime;

        rectTransform.position += moveUpSpeed * delta * Vector3.up;

        timer += delta;
        float alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);

        Color color = text.color;
        color.a = alpha;
        text.color = color;

        if (timer >= fadeDuration)
        {
            Destroy(gameObject);
        }
    }

    public void SetText(string value)
    {
        text.text = value;
    }
}