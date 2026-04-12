
using UnityEngine;
using UnityEngine.UI;

public class HotbarScript : MonoBehaviour
{
    [SerializeField] public Texture2D texture;
    private Image image;

    private Vector2 pos;

    void Start()
    {
        image = GetComponent<Image>();
        pos = GetComponent<RectTransform>().position;
        image.sprite = Texture2DToSprite(texture);
        
        RectTransform rect = GetComponent<RectTransform>();
        rect.anchoredPosition = Vector2.zero;
        rect.sizeDelta = new Vector2(100, 100);
        rect.position = pos;
    }

    private Sprite Texture2DToSprite(Texture2D texture)
    {
        return Sprite.Create(
            texture,
            new Rect(0, 0, texture.width, texture.height),
            new Vector2(0.5f, 0.5f)
        );
    }
}