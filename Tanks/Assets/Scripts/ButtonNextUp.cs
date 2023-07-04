using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
enum NextUp
{
    Health,
    Armor,
    SpeedMove,
    Damage,
}
public class ButtonNextUp : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private LobbiManager lobbiManager;
    [SerializeField] private NextUp upType;
    public void OnPointerEnter(PointerEventData eventData)
    {
        switch (upType)
        {
            case NextUp.Armor:
                lobbiManager.ShowNextUpArmor();
                break;
            case NextUp.Health:
                lobbiManager.ShowNextUpHealth();
                break;
            case NextUp.SpeedMove:
                lobbiManager.ShowNextUpSpeedMove();
                break;
            case NextUp.Damage:
                lobbiManager.ShowNextUpDamage();
                break;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        switch (upType)
        {
            case NextUp.Armor:
                lobbiManager.CloseNextUpArmor();
                break;
            case NextUp.Health:
                lobbiManager.CloseNextUpHealth();
                break;
            case NextUp.SpeedMove:
                lobbiManager.CloseNextUpSpeedMove();
                break;
            case NextUp.Damage:
                lobbiManager.CloseNextUpDamage();
                break;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
