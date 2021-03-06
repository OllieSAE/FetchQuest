using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggeredPlatform : MonoBehaviour
{
    public Transform pos1, pos2;
    public float platformSpeed;
    public Transform startPos;
    public bool startMoving = false;

    private Vector3 nextPos;
    
    // Start is called before the first frame update
    void Start()
    {
        nextPos = startPos.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (transform.position == pos1.position)
        {
            nextPos = pos2.position;
        }

        if (transform.position == pos2.position)
        {
            nextPos = pos1.position;
        }

        if (startMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, nextPos, platformSpeed * Time.deltaTime);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(pos1.position, pos2.position);
    }

    public void StartPlatform()
    {
        print("start moving true");
        startMoving = true;
    }
}