using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private ParticleSystem ps;
    private bool play;
    public GameObject player;

    void Awake()
    {
        play = false;
        ps = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        //if player has died, hasBall becomes false. This check essentially resets the ball whenever the player dies
        if (player.GetComponent<ImprovedPlayerMovement>().hasBall == false)
        {
            GetComponent<Renderer>().enabled = true;
            play = false;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            if (!play)
            {
                print("You found the ball! Good slime dog!");
                play = true;
                FindObjectOfType<AudioManager>().Play("PickUp");
                ps.Play();
                GetComponent<Renderer>().enabled = false;
            }
        }
    }
}
