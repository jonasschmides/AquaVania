using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedObstacle : MonoBehaviour, IActivatable
{
    public float[] waypointDurations;
    public Transform[] waypoints;
    public bool doesLoop;
    public bool isActive;


    private int wpIndex = 0;

    private float timeToNext = 0.0f;

    private Vector3 moveVector;

    void Awake()
    {
        if (waypointDurations.Length != waypoints.Length) Debug.LogError("The duration array must be equally long as the waypoint array");
        transform.position = waypoints[0].position;
    }

    void Update()
    {
        if (isActive)
        {
            CalcWaypointVector();

            transform.position = new Vector3(
                 transform.position.x + moveVector.x * Time.deltaTime,
                 transform.position.y + moveVector.y * Time.deltaTime
            );
        }
    }

    private void CalcWaypointVector()
    {
        if (timeToNext > 0)
        {
            timeToNext -= Time.deltaTime;
        }
        else
        {
            if (wpIndex < waypoints.Length - 1 || doesLoop)
            {
                wpIndex++;

                if (wpIndex >= waypoints.Length)
                    wpIndex = 0;

                timeToNext = waypointDurations[wpIndex];
                moveVector = (waypoints[wpIndex].position - transform.position) / timeToNext;
            }
            else
            {
                moveVector = new Vector3(0, 0);
            }

        }
    }

    public void Activate()
    {
        isActive = true;
    }

    public void Deactivate()
    {
        isActive = false;
    }

    public void Toggle()
    {
        isActive = !isActive;
    }
}
