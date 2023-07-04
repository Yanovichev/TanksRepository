using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveKirka : MonoBehaviour
{
    [SerializeField] private float rotateSpeed;
    void Update()
    {
        transform.Rotate(0f, 0f, 1f * rotateSpeed*Time.deltaTime);
    }
}
