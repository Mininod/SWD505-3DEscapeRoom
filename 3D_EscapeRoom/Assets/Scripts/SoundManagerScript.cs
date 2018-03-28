using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerScript : MonoBehaviour
{
    public GameObject SFXPrefab;

    public List<string> SFXNames;
    public List<AudioClip> SFXs;

    public Dictionary<string, AudioClip> SFXLibrary = new Dictionary<string, AudioClip>();

    void Start()
    {
        for (int i = 0; i < SFXs.Count; ++i)
        {
            SFXLibrary.Add(SFXNames[i], SFXs[i]);
        }
    }

    public void PlaySFX(string name)
    {
        if (SFXLibrary.ContainsKey(name))
        {
            GameObject MySFX = Instantiate(SFXPrefab);
            MySFX.GetComponent<AudioSource>().clip = SFXLibrary[name];
            MySFX.GetComponent<AudioSource>().Play();
        }
    }
}
