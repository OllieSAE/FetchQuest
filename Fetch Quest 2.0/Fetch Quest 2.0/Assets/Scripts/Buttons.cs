using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Buttons : MonoBehaviour, IPointerEnterHandler
{
    public AudioClip clip;
    public AudioManager audioManager;

    public void OnPointerEnter(PointerEventData eventData)
    {
        //function is empty, but exists to allow for the OnPointerEnter to work
        //clip
    }
    
    public void PlayHighlightButton()
    {
        audioManager.GetComponent<AudioSource>().PlayOneShot(clip, 1f);
    }
}
