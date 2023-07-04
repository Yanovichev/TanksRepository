using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeTimeBangBomb : MonoBehaviour
{
    private float time, timeLife = 10f;
    void Update()
    {
        time += Time.deltaTime;
        if (time > timeLife) Destroy(gameObject);
    }
}
