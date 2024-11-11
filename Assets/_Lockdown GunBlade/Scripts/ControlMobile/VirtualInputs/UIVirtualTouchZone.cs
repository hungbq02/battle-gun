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
    [HideInInspector]
    public bool Pressed;

    [Header("Settings")]
    public bool invertYAxis;
    public float sensitivity = 1f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Pressed)
        {
            Vector2 currentPosition;

            if (PointerId >= 0 && PointerId < Input.touches.Length)
            {
                currentPosition = Input.touches[PointerId].position;
            }
            else
            {
                currentPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            }

            TouchDist = currentPosition - PointerOld;

            TouchDist = new Vector2(TouchDist.x * sensitivity, TouchDist.y * sensitivity * (invertYAxis ? -1 : 1));

            PointerOld = currentPosition;
        }
        else
        {
            TouchDist = Vector2.zero;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Pressed = true;
        PointerId = eventData.pointerId;
        PointerOld = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Pressed = false;
    }
}
