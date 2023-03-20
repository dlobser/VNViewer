using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Viewer
{
    public class VNMusicController : MonoBehaviour
    {
        public AudioSource BGMusicAudioSourcePrefab;
        public AudioSource OneShotMusicPrefab;
        List<AudioSource> BGAudio;
        List<AudioSource> OneShots;

        void Start()
        {
            BGAudio = new List<AudioSource>();
            OneShots = new List<AudioSource>();
        }

        public void PlayBGMusic(AudioClip clip)
        {
            if (BGAudio.Count > 0)
            {
                for (int i = BGAudio.Count - 1; i >= 0; i--)
                {
                    AudioSource a = BGAudio[i];
                    BGAudio.RemoveAt(i);
                    Destroy(a.gameObject);
                }

            }
            GameObject A = Instantiate(BGMusicAudioSourcePrefab.gameObject);
            A.transform.parent = this.transform;
            AudioSource AA = A.GetComponent<AudioSource>();
            AA.clip = clip;
            AA.Play();
            BGAudio.Add(AA);
        }

        public void PlayOneShot(AudioClip clip)
        {
            if (OneShots.Count > 0)
            {
                for (int i = OneShots.Count - 1; i >= 0; i--)
                {
                    if (!OneShots[i].isPlaying)
                    {
                        AudioSource a = OneShots[i];
                        BGAudio.RemoveAt(i);
                        Destroy(a.gameObject);
                    }
                }
            }
            GameObject A = Instantiate(BGMusicAudioSourcePrefab.gameObject);
            A.transform.parent = this.transform;
            AudioSource AA = A.GetComponent<AudioSource>();
            AA.clip = clip;
            AA.Play();
            OneShots.Add(AA);
        }
    }
}