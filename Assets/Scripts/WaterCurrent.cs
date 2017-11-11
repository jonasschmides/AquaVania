using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterCurrent : MonoBehaviour, IActivatable
{

    public bool isEmitter;
    public bool isBarrier;

    public GameObject barrierObj;
    public GameObject bubblesObj;
    private ParticleSystem bubbles;
    private BoxCollider2D barrier;
    private AreaEffector2D effector;

    // Use this for initialization
    void Start()
    {
        bubbles = bubblesObj.GetComponent<ParticleSystem>();
        barrier = barrierObj.GetComponent<BoxCollider2D>();
        effector = GetComponent<AreaEffector2D>();

        if (!isBarrier)
            barrier.enabled = false;

        if (!isEmitter)
        {
            bubbles.Stop();
            effector.enabled = false;
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Activate()
    {
        bubbles.Play();
        barrier.enabled = true;
        effector.enabled = true;

    }

    public void Deactivate()
    {
        bubbles.Stop();
        barrier.enabled = false;
        effector.enabled = false;
    }

    public void Toggle()
    {
        if (bubbles.isPlaying)
            bubbles.Stop();
        else
            bubbles.Play();

        barrier.enabled = !barrier.enabled;
        effector.enabled = !effector.enabled;
    }
}
