using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureResizerOnSucked : MonoBehaviour
{
    [SerializeField] private CreatureCore creatureCore;
    [SerializeField] private float scaleSpeed = 5f; // Speed at which the scale changes
    private Vector3 targetScale;

    void Start()
    {
        InitializeTargetScale();
    }

    void Update()
    {
        if (IsBeingSucked())
        {
            SetTargetScale();
        }

        SmoothlyTransitionToTargetScale();
    }

    // Method to initialize the target scale to the current scale at the start
    private void InitializeTargetScale()
    {
        targetScale = transform.localScale;
    }

    // Method to check if the creature is currently being sucked
    private bool IsBeingSucked()
    {
        return creatureCore.GetIsBeingSucked();
    }

    // Method to calculate and set the target scale based on distance
    private void SetTargetScale()
    {
        float distance = GetDistanceToSuckZone();
        float scaleFactor = CalculateScaleFactor(distance);
        targetScale = Vector3.one * scaleFactor;
    }

    // Method to get the distance to the suction zone from CreatureCore
    private float GetDistanceToSuckZone()
    {
        return creatureCore.GetDistanceFromTheZone();
    }

    // Method to calculate the scale factor based on distance
    private float CalculateScaleFactor(float distance)
    {
        return Mathf.Clamp(distance / 5f, 0.2f, 1f); // Adjust 5f, 0.2f, and 1f as needed
    }

    // Method to smoothly transition the scale towards the target scale
    private void SmoothlyTransitionToTargetScale()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, scaleSpeed * Time.deltaTime);
    }
}
