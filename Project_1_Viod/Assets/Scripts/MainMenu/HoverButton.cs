using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;



public class HoverButton : MonoBehaviour
{

    [SerializeField] private Image buttonImage;
    [SerializeField] private Color hoverColor = new Color(1f, 0.82f, 0.4f, 1f);

    private Color originalColor;
    private Color TargetColor;

    private void Awake()
    {
        buttonImage = GetComponent<Image>();

        originalColor = buttonImage.color;

        TargetColor = originalColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        TargetColor = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TargetColor = originalColor;
    }


}
