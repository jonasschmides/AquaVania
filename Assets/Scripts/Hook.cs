using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : Activatable
{

    public bool isReelingIn = false;

    private float reelInSpeed = 2f;
    private float startY;
    private CircleCollider2D coll;
    private AudioSource aSrc;

    // Use this for initialization
    void Start()
    {
        startY = transform.position.y;
        coll = GetComponent<CircleCollider2D>();
        aSrc = GetComponent<AudioSource>();

        if (isReelingIn)
            Activate();
        else
            Deactivate();

    }

    // Update is called once per frame
    void Update()
    {
        if (isReelingIn)
        {
            this.transform.position = new Vector3(transform.position.x, transform.position.y + reelInSpeed * Time.deltaTime);
        }
        else if (transform.position.y > startY)
        {
            this.transform.position = new Vector3(transform.position.x, transform.position.y - reelInSpeed * 1.5f * Time.deltaTime);
            coll.enabled = false;
        }
        else
        {
            this.transform.position = new Vector3(transform.position.x, startY);
            coll.enabled = true;
        }
    }


    public override void Activate()
    {
        isReelingIn = true;
        aSrc.Play();
    }

    public override void Deactivate()
    {
        isReelingIn = false;
        aSrc.Stop();

    }

    public override void Toggle()
    {
        throw new System.NotImplementedException();
    }
}