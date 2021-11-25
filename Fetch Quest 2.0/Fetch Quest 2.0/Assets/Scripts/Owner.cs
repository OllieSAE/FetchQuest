using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Owner : MonoBehaviour
{
    SpriteRenderer [] renderers;
    
    // Start is called before the first frame update
    void Start()
    {
        renderers = GetComponentsInChildren<SpriteRenderer>();
        for (int i = 1; i < renderers.Length; i++)
        {
            renderers[i].enabled = false;
        }
    }

    public void EnableSpeechBubble()
    {
        for (int i = 1; i < renderers.Length; i++)
        {
            renderers[i].enabled = true;
        }
    }

    public void DisableSpeechBubble()
    {
        StartCoroutine("DisableSpeechBubbleCoroutine");
    }
    
    public IEnumerator DisableSpeechBubbleCoroutine()
    {
        print("waiting");
        yield return new WaitForSeconds(1.1f);
        print("waiting done");
        for (int i = 1; i < renderers.Length; i++)
        {
            renderers[i].enabled = false;
        }
    }
}
