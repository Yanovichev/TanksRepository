using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class MoveRewards : MonoBehaviour
{
    [SerializeField] private GameObject target;
    public Vector3 posSpawn;
    public int reward;
    void Start()
    {
        
    }
    public void AddRewards()
    {
        ParameterManager parameterManager = GameObject.FindGameObjectWithTag("ParameterManager").GetComponent<ParameterManager>();
        parameterManager.AmmoCount += reward;
        Destroy(this.gameObject);
    }
    public void Move()
    {
        target = GameObject.FindGameObjectWithTag("AmmoCount");
        transform.position = posSpawn;
        transform.DOMove(target.transform.position, 1f).onComplete = AddRewards;
    }

}
