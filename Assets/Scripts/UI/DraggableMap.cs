using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class DraggableMap : MonoBehaviour, IDragHandler, IScrollHandler
{
    [Header("Zoom Settings")]
    [SerializeField] private float _zoomSpeed = 0.005f;
    [SerializeField] private float _minZoom = 0.5f;
    [SerializeField] private float _maxZoom = 1.0f;

    [Header("Boundary Padding")]
    [SerializeField] private float _horizontalPadding = 100f;
    [SerializeField] private float _verticalPadding = 100f;

    [Header("Map Elements")]
    [SerializeField] private RectTransform polygonsParent; 

    private float initialScale = 0.5f;
    private RectTransform rectTransform;
    private RectTransform parentRect;
    private Vector2 minPosition;
    private Vector2 maxPosition;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        parentRect = transform.parent.GetComponent<RectTransform>();

        if(polygonsParent == null)
        {
            foreach(Transform child in parentRect)
            {
                if(child != transform && child.name.Contains("Polygon"))
                {
                    polygonsParent = child.GetComponent<RectTransform>();
                    break;
                }
            }
        }

        InitializeMap();
    }

    private void InitializeMap()
    {
        rectTransform.localScale = Vector3.one * initialScale;
        rectTransform.anchoredPosition = new Vector2(0f, -50f);
        
        if(polygonsParent != null)
        {
            polygonsParent.localScale = rectTransform.localScale;
            polygonsParent.anchoredPosition = rectTransform.anchoredPosition;
        }
        
        CalculateBounds();
    }

    private void CalculateBounds()
    {
        Vector2 visibleSize = parentRect.rect.size;
        Vector2 scaledMapSize = rectTransform.rect.size * rectTransform.localScale.x;
        Vector2 maxMovement = (scaledMapSize - visibleSize) / 2;

        maxMovement.x = Mathf.Max(0, maxMovement.x - _horizontalPadding);
        maxMovement.y = Mathf.Max(0, maxMovement.y - _verticalPadding);

        minPosition = -maxMovement;
        maxPosition = maxMovement;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(eventData.pointerEnter != gameObject) return;
        
        Vector2 delta = eventData.delta / rectTransform.localScale.x;
        
        rectTransform.localPosition += (Vector3)delta;
        if (polygonsParent != null)
        {
            polygonsParent.localPosition = rectTransform.localPosition;
        }
            
        ClampPosition();
    }

    public void OnScroll(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out Vector2 localPoint);

        float currentScale = rectTransform.localScale.x;
        float zoomDelta = eventData.scrollDelta.y * _zoomSpeed * currentScale;
        float newScale = Mathf.Clamp(currentScale + zoomDelta, _minZoom, _maxZoom);

        if(Mathf.Abs(newScale - currentScale) > 0.001f)
        {
            Vector2 pivotOffset = localPoint / rectTransform.rect.size * (1 - currentScale/newScale);
            
            rectTransform.localScale = Vector3.one * newScale;
            rectTransform.localPosition += (Vector3)(pivotOffset * rectTransform.rect.size);
            
            if(polygonsParent != null)
            {
                polygonsParent.localScale = rectTransform.localScale;
                polygonsParent.localPosition = rectTransform.localPosition;
            }

            CalculateBounds();
            ClampPosition();
        }
    }

    private void ClampPosition()
    {
        Vector3 clampedPosition = rectTransform.localPosition;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, minPosition.x, maxPosition.x);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, minPosition.y, maxPosition.y);
        
        rectTransform.localPosition = clampedPosition;
        if (polygonsParent != null)
        {
            polygonsParent.localPosition = clampedPosition;
        }
    }
}