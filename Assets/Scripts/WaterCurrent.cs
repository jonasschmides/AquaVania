using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterCurrent : Activatable
{

    public bool isEmitter;
    public bool isBarrier;

    public GameObject barrierObj;
    public GameObject bubblesObj;
    public ParticleSystem bubbles;
    public BoxCollider2D barrier;
    public AreaEffector2D effector;

    // Use this for initialization
    void Start()
    {
        if (isActive)
            Activate();
        else
            Deactivate();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void Activate()
    {
        bubbles.Play();
        barrier.enabled = isBarrier;
        effector.enabled = isEmitter;
        isActive = true;
    }

    public override void Deactivate()
    {
        bubbles.Stop();
        barrier.enabled = false;
        effector.enabled = false;
        isActive = false;
    }

    public override void Toggle()
    {
        if (isActive)
            Deactivate();
        else
            Activate();
    }
}