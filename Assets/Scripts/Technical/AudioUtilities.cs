using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AudioUtilities
{
    /// <summary>
    /// Adds an AudioSource with the clip to the GameObject.
    /// </summary>
    /// <param name="gameObject"></param>
    /// <param name="clip">The clip to add.</param>
    /// <param name="loop">Loop the clip?</param>
    /// <param name="playAwake">Play clip on awake?</param>
    /// <param name="vol">Clip's volume.</param>
    /// <returns></returns>
    public static AudioSource AddAudio(this GameObject gameObject, AudioClip clip, bool loop = false, bool playAwake = false, float vol = 1f)
    {
        if (clip == null)
            return null;
        // Check if clip already exists in object.
        foreach (AudioSource audio in gameObject.GetComponents<AudioSource>())
        {
            if (audio.clip == clip)
            {
                // Update values.
                audio.loop = loop;
                audio.playOnAwake = playAwake;
                audio.volume = vol;
                return audio;
            }
        }
        var newAudio = gameObject.AddComponent<AudioSource>();
        newAudio.clip = clip;
        newAudio.loop = loop;
        newAudio.playOnAwake = playAwake;
        newAudio.volume = vol;
        return newAudio;
    }

    /// <summary>
    /// Plays a clip from an AudioSource in the GameObject. 
    /// </summary>
    /// <param name="gameObject"></param>
    /// <param name="clip">The clip to play.</param>
    /// <param name="addIfNotFound">Add and play the clip if not found?.</param>
    public static void PlayAudio(this GameObject gameObject, AudioClip clip, bool addIfNotFound = true)
    {
        if (clip == null)
            return;

        // Check if clip already exists in object.
        foreach (AudioSource audio in gameObject.GetComponents<AudioSource>())
        {
            if (audio.clip == clip)
            {
                audio.Play();
                return;
            }
        }
        if (addIfNotFound)
        {
            // Add and play if not found.
            gameObject.AddAudio(clip);
            gameObject.PlayAudio(clip);
        }
    }

    public static void Mute(this GameObject g)
    {
        foreach (AudioSource s in g.GetComponents<AudioSource>())
        {
            s.mute = true;
        }
    }

    public static bool IsMute(this GameObject g)
    {
        bool mute = true;
        foreach (AudioSource s in g?.GetComponents<AudioSource>())
        {
            if (!s.mute)
                mute = false;
        }
        return mute;
    }

    public static void ToggleMute(this GameObject g)
    {
        foreach (AudioSource s in g.GetComponents<AudioSource>())
        {
            s.mute = !s.mute;
        }
    }

    public static void Unmute(this GameObject g)
    {
        foreach (AudioSource s in g.GetComponents<AudioSource>())
        {
            s.mute = false;
        }
    }
}
