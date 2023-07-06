using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeComponent : MonoBehaviour
{
    public VolumeController.VolumeType type;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        VolumeController volumeController = GameObject.Find("GameController").GetComponent<VolumeController>();
        audioSource.volume = volumeController.GetVolume(type);
        volumeController.OnVolumeUpdate += OnVolumeUpdate;
    }

    private void OnVolumeUpdate(VolumeController.VolumeType type, float amount)
    {
        if(type == this.type)
            audioSource.volume = amount;
    }
}
