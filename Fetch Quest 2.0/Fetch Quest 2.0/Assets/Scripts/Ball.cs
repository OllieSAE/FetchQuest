using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    //private ParticleSystem ps;
    private bool play;
    public GameObject player;

    public Transform target;

    public float smoothSpeed = 0.075f;
    public Vector3 offset;
    private bool chase = false;
    
    private Vector2 respawnPoint;

    void Awake()
    {
        play = false;
        respawnPoint = transform.position;
        //ps = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        //if player has died, hasBall becomes false. This check essentially resets the ball whenever the player dies
        if (player.GetComponent<ImprovedPlayerMovement>().hasBall == false)
        {
            Respawn();
            play = false;
            chase = false;
        }
    }

    private void FixedUpdate()
    {
        if (chase)
        {
            Vector3 desiredPosition = target.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
    }
    private void Respawn()
    {
        transform.position = respawnPoint;
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
                //ps.Play();
                //GetComponent<Renderer>().enabled = false;
                chase = true;
            }
        }
    }
}
