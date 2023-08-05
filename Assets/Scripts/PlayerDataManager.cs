using UnityEngine;
using UnityEngine.Events;

public class PlayerDataManager : MonoBehaviour, IHitable, IHealable
{
    [SerializeField] float nowLife, maxLife;
    UnityEvent<(float, float)> lifeEvent;

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
}
