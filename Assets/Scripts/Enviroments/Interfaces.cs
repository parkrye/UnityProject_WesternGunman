using UnityEngine;

public interface IInteractable
{
    public void Interact();
}

public interface ITalkable
{
    public void Talk(Transform talker);
}

public interface ILockable
{
    public void Lock();
    public void Unlock();
}

public interface IHitable
{
    public void Hit(float damage);

    void Dead();
}

public interface IHealable
{
    public void Heal(float heal, float maxLife = 0f);
}