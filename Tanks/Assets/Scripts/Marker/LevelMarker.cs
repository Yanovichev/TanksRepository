using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMarker : MonoBehaviour
{
    public GameObject block, button;
    public int numberLevel;
    [SerializeField] private LobbiManager lobbiManager;

    public void PlayNemberLevel()
    {
        lobbiManager = GameObject.FindGameObjectWithTag("LobbiManager").GetComponent<LobbiManager>();
        lobbiManager.PlayNumberLevel(numberLevel);
    }
}
