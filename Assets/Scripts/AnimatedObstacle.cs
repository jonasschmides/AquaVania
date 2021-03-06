﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedObstacle : Activatable
{
    public float[] waypointDurations;
    public Transform[] waypoints;
    public bool doesLoop;

    private int wpIndex = 0;

    public float timeToNext = 0.0f;

    private Vector3 moveVector;

    void Awake()
    {
        if (waypoints.Length == 0) return;

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
}