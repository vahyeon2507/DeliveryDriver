using UnityEngine;

public class Delivery : MonoBehaviour
{
    [SerializeField] Color noChickenColor = new Color(1, 1, 1, 1);
    [SerializeField] Color hasChickenColor = new Color(1, 1, 1, 1);
    [SerializeField] float delay = 1.0f;

    [SerializeField] bool hasChicken = false;
    SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Chicken") && !hasChicken)
        {
            hasChicken = true;
            Debug.Log("Ä¡Å² ÇÈ¾÷µÊ");
            spriteRenderer.color = hasChickenColor;
            Destroy(collision.gameObject, delay);
        }

        if (collision.gameObject.CompareTag("Customer") && hasChicken)
        {
            Debug.Log("Ä¡Å² ¹è´ÞµÊ");
            hasChicken = false;
            spriteRenderer.color = noChickenColor;
        } 
    }
}
