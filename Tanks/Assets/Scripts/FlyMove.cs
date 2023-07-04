using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyMove : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Vector3 deltaPos;
    void Update()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            /*if (player == null)
            {
                Debug.Log("Player is'n find");
            }
            else Debug.Log("Player is find");*/
        }
        if (player != null)
        {
            Vector3 position = player.transform.position;
            transform.position = new Vector3(position.x + deltaPos.x, position.y + deltaPos.y, position.z + deltaPos.z);
            //Debug.Log("translate");
        }
    }
}
