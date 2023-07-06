using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    private RectTransform rectTransform;
    private bool show = false;

    private Vector2 hidePosition;
    private Vector2 showPosition = new Vector2(-2000f, -100f);

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        hidePosition = rectTransform.anchoredPosition;
    }

    private void Update()
    {
        rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, show ? showPosition : hidePosition, Time.deltaTime * 10f);
    }

    public void OnPointerEnter()
    {
        show = true;
    }

    public void OnPointerExit()
    {
        show = false;
    }
}
