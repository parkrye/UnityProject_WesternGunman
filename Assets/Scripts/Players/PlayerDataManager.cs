using UnityEngine;
using UnityEngine.Events;

public class PlayerDataManager : MonoBehaviour, IHitable, IHealable
{
    [SerializeField] Player player;
    [SerializeField] PlayerData playerData;
    public int Money { get { return playerData.Money; } set { playerData.Money = value; moneyEvent?.Invoke(playerData.Money); } }

    UnityEvent<(float, float)> lifeEvent, armorEvent;
    UnityEvent<int> moneyEvent;

    public void Initialize()
    {
        lifeEvent = new UnityEvent<(float, float)>();
        moneyEvent = new UnityEvent<int>();
        armorEvent = new UnityEvent<(float, float)>();
    }

    public void Hit(float damage)
    {
        playerData.NowArmor -= damage;
        if (playerData.NowArmor < 0f)
        {
            playerData.NowLife += playerData.NowArmor;
            playerData.NowArmor = 0f;
        }

        if(playerData.NowLife < 0)
        {
            playerData.NowLife = 0;
        }

        lifeEvent.Invoke((playerData.NowLife, playerData.MaxLife));
        armorEvent?.Invoke((playerData.NowArmor, playerData.MaxArmor));
    }

    public void Dead()
    {

    }

    public void Heal(float heal, float maxLife = 0f)
    {
        playerData.MaxLife += maxLife;
        playerData.NowLife += heal;
        if (playerData.NowLife > playerData.MaxLife)
        {
            playerData.NowLife = playerData.MaxLife;
        }
        lifeEvent.Invoke((playerData.NowLife, playerData.MaxLife));
    }

    public void AddArmor(float nowArmorModifier, float maxArmorModifier = 0f)
    {
        playerData.MaxArmor += maxArmorModifier;
        playerData.NowArmor += nowArmorModifier;
        if(playerData.NowArmor > playerData.MaxArmor)
            playerData.NowArmor = playerData.MaxArmor;
        armorEvent?.Invoke((playerData.NowArmor, playerData.MaxArmor));
    }

    public (float, float) GetArmor()
    {
        return (playerData.NowArmor, playerData.MaxArmor);
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

    public void AddArmorEventListener(UnityAction<(float, float)> listener)
    {
        armorEvent.AddListener(listener);
    }

    public void RemoveArmorEventListener(UnityAction<(float, float)> listener)
    {
        armorEvent.RemoveListener(listener);
    }
}
