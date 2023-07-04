using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField] public GameObject player;
    [SerializeField] private float deltaX, deltaY;

    private void Awake()
    {
    }
    // Update is called once per frame
    void LateUpdate()
    {
        if (player != null)
        {
            Vector3 forward = new Vector3(player.transform.position.x + deltaX, player.transform.position.y + deltaY, -10);
            transform.position = forward;
        }    
    }
}
