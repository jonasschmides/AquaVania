﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatCurrent : Activatable
{

    public bool isEmitter;

    public GameObject barrierObj;
    public GameObject bubblesObj;
    public ParticleSystem bubbles;
    public BoxCollider2D barrier;
    public GameObject hideMe;

    public float timeOff;
    public float timeOn;
    public float timeDelayShift;

    private bool timerState;
    private float timer;

    // Use this for initialization
    void Start()
    {
        if (isActive)
            Activate();
        else
            Deactivate();

        timer = timeDelayShift;
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                if (timerState)
                {
                    barrier.enabled = false;

                    bubbles.Stop();
                    timer = timeOff;
                }
                else if (!timerState)
                {
                    barrier.enabled = true;

                    bubbles.Play();
                    timer = timeOn;
                }
                timerState = !timerState;
            }
        }
    }

    public override void Activate()
    {
        if (hideMe != null)
            hideMe.SetActive(true);
        bubbles.Play();
        barrier.enabled = true;
        isActive = true;
    }

    public override void Deactivate()
    {
        if (hideMe != null)
            hideMe.SetActive(false);
        bubbles.Stop();
        barrier.enabled = false;
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