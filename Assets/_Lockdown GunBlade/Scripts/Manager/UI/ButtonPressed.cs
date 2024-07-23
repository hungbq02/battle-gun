using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonPressed : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        transform.localScale = Vector3.one * 0.8f;
        if (gameObject.TryGetComponent(out Animator anim))
        {
            anim.enabled = false;
        }
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        transform.localScale = Vector3.one * 1f;
        if (gameObject.TryGetComponent(out Animator anim))
        {
            anim.enabled = true;
        }
    }
}
