using UnityEngine;
using UnityEngine.EventSystems;

public class UIVirtualTouchZone : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [HideInInspector]
    public Vector2 TouchDist;
    [HideInInspector]
    public Vector2 PointerOld;
    [HideInInspector]
    protected int PointerId;
    //   [HideInInspector]
    public bool Pressed;

    [Header("Settings")]
    public bool invertYAxis;
    public float sensitivity = 1f;

    private const float touchDetectionThreshold = 0.1f;
    void Update()
    {
        if (Pressed)
        {
            bool validTouch = false;


            foreach (Touch touch in Input.touches)
            {
                if (touch.fingerId == PointerId)
                {
                   // Debug.Log("TouchId Matched: " + touch.fingerId);

                    Vector2 newTouchPos = touch.position;
                    TouchDist = newTouchPos - PointerOld;
                    TouchDist = new Vector2(TouchDist.x * sensitivity,
                                            TouchDist.y * sensitivity * (invertYAxis ? -1 : 1));
                    PointerOld = newTouchPos;
                    validTouch = true;
                    break;
                }
            }

            if (!validTouch)
            {
                Debug.Log("No valid touch found for PointerId: " + PointerId);
                Pressed = false; // Dừng cập nhật khi mất touch
                TouchDist = Vector2.zero;
            }
        }
        else
        {
            TouchDist = Vector2.zero;
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        Pressed = true;

        // Find the touch that matches the ID
        foreach (Touch touch in Input.touches)
        {
            if (Vector2.Distance(touch.position, eventData.position) < touchDetectionThreshold) // Fix joystick anh huong TouchZone
            {
                PointerId = touch.fingerId; //Get the touch ID
                PointerOld = touch.position;
                return;
            }
        }

        // If find not found, use mouse position
        PointerId = -1;
        PointerOld = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        Pressed = false;
    }
}





/*using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class UIVirtualTouchZone : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [System.Serializable]
    public class Event : UnityEvent<Vector2> { }

    [Header("Rect References")]
    public RectTransform containerRect;
    //public RectTransform handleRect;

    [Header("Settings")]
    public bool clampToMagnitude;
    public float magnitudeMultiplier = 1f;
    public bool invertXOutputValue;
    public bool invertYOutputValue;

    // Stored Pointer Values
    private Vector2 pointerDownPosition;
    private Vector2 currentPointerPosition;
    private bool isDragging = false;
    public float dragThreshold = 0.01f;
    [Header("Output")]
    public Event touchZoneOutputEvent;

    public void OnPointerDown(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(containerRect, eventData.position, eventData.pressEventCamera, out pointerDownPosition);
    }

    public void OnDrag(PointerEventData eventData)
    {
        isDragging = true;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(containerRect, eventData.position, eventData.pressEventCamera, out currentPointerPosition);

        Vector2 positionDelta = GetDeltaBetweenPositions(pointerDownPosition, currentPointerPosition);
        if (positionDelta.sqrMagnitude < dragThreshold * dragThreshold)
        {
            return; // Không di chuyển nếu người chơi chỉ giữ yên
        }
        Vector2 clampedPosition = ClampValuesToMagnitude(positionDelta);
        Vector2 outputPosition = ApplyInversionFilter(clampedPosition);

        OutputPointerEventValue(outputPosition * magnitudeMultiplier);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;

        // Reset the output value to zero
        OutputPointerEventValue(Vector2.zero);

        pointerDownPosition = Vector2.zero;
        currentPointerPosition = Vector2.zero;
    }

    void OutputPointerEventValue(Vector2 pointerPosition)
    {
        // Just output the value if the user is dragging or the pointer position is zero
        if (isDragging || pointerPosition == Vector2.zero)
        {
            touchZoneOutputEvent.Invoke(pointerPosition);
        }
    }

    void SetObjectActiveState(GameObject targetObject, bool newState)
    {
        targetObject.SetActive(newState);
    }

    Vector2 GetDeltaBetweenPositions(Vector2 firstPosition, Vector2 secondPosition)
    {
        return secondPosition - firstPosition;
    }

    Vector2 ClampValuesToMagnitude(Vector2 position)
    {
        return Vector2.ClampMagnitude(position, 1);
    }

    Vector2 ApplyInversionFilter(Vector2 position)
    {
        if (invertXOutputValue)
        {
            position.x = InvertValue(position.x);
        }

        if (invertYOutputValue)
        {
            position.y = InvertValue(position.y);
        }

        return position;
    }

    float InvertValue(float value)
    {
        return -value;
    }
}*/
