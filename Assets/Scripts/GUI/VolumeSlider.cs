using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour, IPointerEnterHandler
{
    private SettingsMenu settingsMenu;
    private VolumeController volumeController;
    private Slider slider;
    public VolumeController.VolumeType volumeType;

    void Start()
    {
        volumeController = GameObject.Find("GameController").GetComponent<VolumeController>();
        settingsMenu = transform.parent.GetComponent<SettingsMenu>();
        slider = GetComponent<Slider>();

        slider.value = volumeController.GetVolume(volumeType);

        slider.onValueChanged.AddListener(OnValueChanged);
    }

    private void OnValueChanged(float value)
    {
        volumeController.UpdateVolume(volumeType, value);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        settingsMenu.OnPointerEnter();
    }
}
