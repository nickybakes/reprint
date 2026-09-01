using UnityEngine;
using UnityEngine.InputSystem;

public class UIView : MonoBehaviour
{

    public static UIView view;
    public static RectTransform CanvasRect { get; private set; }
    public static Canvas Canvas { get; private set; }

    [field: SerializeField] public RarityDirectory RarityDirectory { get; private set; }
    [field: SerializeField] public SubTagDirectory SubTagDirectory { get; private set; }

    private Vector3 mouseScreenPosition;
    private Vector3 mouseViewPosition;


    public Vector3 MouseViewPosition { get => mouseViewPosition; }

    void Awake()
    {
        if (view != null && view != this)
        {
            Destroy(view);
        }

        view = this;
        Canvas = GetComponent<Canvas>();
        CanvasRect = GetComponent<RectTransform>();
    }

    public static Vector3 WorldToCanvasPoint(Vector3 position)
    {
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(position);
        screenPosition.x *= CanvasRect.rect.width / (float)Camera.main.pixelWidth;
        screenPosition.y *= CanvasRect.rect.height / (float)Camera.main.pixelHeight;
        screenPosition.x = screenPosition.x - CanvasRect.sizeDelta.x / 2f;
        screenPosition.y = screenPosition.y - CanvasRect.sizeDelta.y / 2f;
        return screenPosition;
    }

    void Update()
    {
        mouseScreenPosition = Mouse.current.position.value;
        mouseScreenPosition.z = 0;
        mouseScreenPosition.x = mouseScreenPosition.x / Screen.width;
        mouseScreenPosition.y = mouseScreenPosition.y / Screen.height;
        mouseScreenPosition.x -= .5f;
        mouseScreenPosition.y -= .5f;
        mouseScreenPosition.x *= CanvasRect.sizeDelta.x;
        mouseScreenPosition.y *= CanvasRect.sizeDelta.y;
        mouseViewPosition = mouseScreenPosition;
    }

}