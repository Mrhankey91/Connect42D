using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeController : MonoBehaviour
{
    public enum VolumeType {Sound, Music, UI};

    public delegate void VolumeUpdate(VolumeType type, float value);
    public VolumeUpdate OnVolumeUpdate;

    // Start is called before the first frame update
    void Start()
    {
        OnVolumeUpdate?.Invoke(VolumeType.Music, PlayerPrefs.GetFloat("MusicVolume", 0.75f));
        OnVolumeUpdate?.Invoke(VolumeType.Sound, PlayerPrefs.GetFloat("SoundVolume", 0.75f));
        OnVolumeUpdate?.Invoke(VolumeType.UI, PlayerPrefs.GetFloat("SoundVolume", 0.75f));
    }

    public void UpdateVolume(VolumeType type, float value)
    {
        switch (type)
        {
            case VolumeType.Music:
                PlayerPrefs.SetFloat("MusicVolume", value);
                OnVolumeUpdate?.Invoke(type, value);
                break;
            case VolumeType.Sound:
            case VolumeType.UI:
                PlayerPrefs.SetFloat("SoundVolume", value);
                OnVolumeUpdate?.Invoke(VolumeType.Sound, value);
                OnVolumeUpdate?.Invoke(VolumeType.UI, value);
                break;
        }
    }

    public float GetVolume(VolumeType type)
    {
        switch (type)
        {
            case VolumeType.Music:
                return PlayerPrefs.GetFloat("MusicVolume", 0.75f);
            case VolumeType.Sound:
            case VolumeType.UI:
                return PlayerPrefs.GetFloat("SoundVolume", 0.75f);
        }

        return 0f;
    }
}
