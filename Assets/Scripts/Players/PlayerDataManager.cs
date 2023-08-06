using UnityEngine;
using UnityEngine.Events;

public class PlayerDataManager : MonoBehaviour, IHitable, IHealable
{
    [SerializeField] float nowLife, maxLife;
    [SerializeField] int money;
    public int Money { get { return money; } set { money = value; moneyEvent?.Invoke(money); } }
    UnityEvent<(float, float)> lifeEvent;
    UnityEvent<float> moneyEvent;

    public void Initialize()
    {
        lifeEvent = new UnityEvent<(float, float)>();
    }

    public void Hit(float damage)
    {
        nowLife -= damage;
        if(nowLife < 0)
        {
            nowLife = 0;
        }
        lifeEvent.Invoke((nowLife, maxLife));
    }

    public void Dead()
    {

    }

    public void Heal(float heal)
    {
        nowLife += heal;
        if (nowLife > maxLife)
        {
            nowLife = maxLife;
        }
        lifeEvent.Invoke((nowLife, maxLife));
    }

    public void AddLifeEventListener(UnityAction<(float, float)> listener)
    {
        lifeEvent.AddListener(listener);
    }

    public void RemoveLifeEventListener(UnityAction<(float, float)> listener)
    {
        lifeEvent.RemoveListener(listener);
    }

    public void AddMoneyEventListener(UnityAction<float> listener)
    {
        moneyEvent.AddListener(listener);
    }

    public void RemoveMoneyEventListener(UnityAction<float> listener)
    {
        moneyEvent.RemoveListener(listener);
    }
}
