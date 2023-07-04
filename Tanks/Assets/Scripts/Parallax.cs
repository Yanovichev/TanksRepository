using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] Transform followingTarget;
    [SerializeField, Range(0f, 1f)] float parallaxStrength = 0.1f;
    [SerializeField] bool diszbledVecticalParallax;
    Vector3 targetPreviousPosition;
    void Start()
    {
        if (!followingTarget) followingTarget = Camera.main.transform;

        targetPreviousPosition = followingTarget.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 delta = followingTarget.position - targetPreviousPosition;

        if (diszbledVecticalParallax) delta.y = 0;

        targetPreviousPosition = followingTarget.position;
        transform.position += delta * parallaxStrength;
    }
}
