using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;



public class AudioManager : MonoBehaviour
{
    public List<string> AudioNames;
    public List<AudioClip> Audios;
    Dictionary<string, AudioSource> nameMap = new Dictionary<string, AudioSource>();

    void Awake()
    {
        for(int i = 0; i < AudioNames.Count; i++)
        {
            AudioSource asource = gameObject.AddComponent<AudioSource>();
            asource.clip = Audios[i];
            asource.playOnAwake = false;
            nameMap[AudioNames[i]] = asource;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Play(string name)
    {
        foreach(KeyValuePair<string, AudioSource> kp in nameMap)
        {
            if(kp.Key == name) {
                print(name);
                kp.Value.Play();
                break;
            }
        }
    }
}
