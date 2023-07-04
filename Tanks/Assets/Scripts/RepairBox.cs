using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairBox : MonoBehaviour
{
    [SerializeField] private int procentHill;
    [SerializeField] private AudioClip audioRepair;
    private void RepairAudio()
    {
        GameObject nativeAudio = new GameObject();
        nativeAudio.AddComponent<LifeTimeBangBomb>();
        AudioSource audioSours = nativeAudio.AddComponent<AudioSource>();
        audioSours.clip = audioRepair;
        audioSours.Play();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if (player != null)
        {
            int maxHealth = player.MaxHealth;
            int hill = (maxHealth * procentHill) / 100;
            player.Health += hill;
            RepairAudio();
            Destroy(this.gameObject);
        }
    }
}
