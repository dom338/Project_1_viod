using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;


public class HoverButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI textMeshPro;

    [SerializeField] private Color StarterColor;
    [SerializeField] private Color huverColor;
    [SerializeField] private AudioClip huverSound;
    [SerializeField] private AudioSource audioSource;

    private void Awake()
    {
        if (textMeshPro == null)
        {
            textMeshPro = GetComponent<TextMeshProUGUI>();
        }
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    private void Start()
    {
        if (textMeshPro != null)
        {
            textMeshPro.color = StarterColor;
        }
        else
        {
            Debug.Log("no text found");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        textMeshPro.color = huverColor;

        if (huverSound != null)
        {
            if (audioSource != null)
            {
                audioSource.PlayOneShot(huverSound);
            }
            else
            {
                Debug.Log("audiosource not found");
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        textMeshPro.color = StarterColor;
    }


}
