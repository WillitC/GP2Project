using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHover : MonoBehaviour
{
    // Speed of rotation
    public float rotationSpeed = 50f;

    // Speed of floating motion
    public float floatSpeed = 0.5f;

    // How far it floats up and down
    public float floatAmplitude = 0.5f;

    // Initial position
    private Vector3 startPosition; 

    void Start()
    {
        // Store the initial position
        startPosition = transform.position;
    }

    void Update()
    {
        // Rotate the health pack
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

        // Make the health pack float up and down
        float newY = startPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
    }
}