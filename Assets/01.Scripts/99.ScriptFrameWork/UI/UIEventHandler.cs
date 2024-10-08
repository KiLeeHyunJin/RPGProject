using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIEventHandler : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    public event Action OnClickHandler = null;
    public event Action OnPressedHandler = null;
    public event Action OnPointerDownHandler = null;
    public event Action OnPointerUpHandler = null;
    bool _pressed;

    private void Start()
    {
        _pressed = default;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClickHandler?.Invoke();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _pressed = true;
        this.ReStartCoroutine(OnPressedHandlerRoutine(), ref pressCo);
        OnPointerDownHandler?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _pressed = false;
        this.StopCoroutine(ref pressCo);
        OnPointerUpHandler?.Invoke();
    }

    Coroutine pressCo;
    IEnumerator OnPressedHandlerRoutine()
    {
        while (_pressed)
        {
            OnPressedHandler?.Invoke();
            yield return null;
        }
    }
}
