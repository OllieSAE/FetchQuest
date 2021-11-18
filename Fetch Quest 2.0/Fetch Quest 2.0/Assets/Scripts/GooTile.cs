using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GooTile : MonoBehaviour
{
    public Sprite GooLeft;
    public Sprite GooRight;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FadeOut());
    }

    // Update is called once per frame
    IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}
