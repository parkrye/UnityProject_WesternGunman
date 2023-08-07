using UnityEngine;
using UnityEngine.Events;

public class PlayerDataManager : MonoBehaviour, IHitable, IHealable
{
    [SerializeField] PlayerData playerData;
    public int Money { get { return playerData.Money; } set { playerData.Money = value; moneyEvent?.Invoke(playerData.Money); } }
    UnityEvent<(float, float)> lifeEvent;
    UnityEvent<int> moneyEvent;

    public void Initialize()
    {
        lifeEvent = new UnityEvent<(float, float)>();
        moneyEvent = new UnityEvent<int>();
    }

    public void Hit(float damage)
    {
        playerData.NowLife -= damage;
        if(playerData.NowLife < 0)
        {
            playerData.NowLife = 0;
        }
        lifeEvent.Invoke((playerData.NowLife, playerData.MaxLife));
    }

    public void Dead()
    {

    }

    public void Heal(float heal)
    {
        playerData.NowLife += heal;
        if (playerData.NowLife > playerData.MaxLife)
        {
            playerData.NowLife = playerData.MaxLife;
        }
        lifeEvent.Invoke((playerData.NowLife, playerData.MaxLife));
    }

    public void AddLifeEventListener(UnityAction<(float, float)> listener)
    {
        lifeEvent.AddListener(listener);
    }

    public void RemoveLifeEventListener(UnityAction<(float, float)> listener)
    {
        lifeEvent.RemoveListener(listener);
    }

    public void AddMoneyEventListener(UnityAction<int> listener)
    {
        moneyEvent.AddListener(listener);
    }

    public void RemoveMoneyEventListener(UnityAction<int> listener)
    {
        moneyEvent.RemoveListener(listener);
    }
}
