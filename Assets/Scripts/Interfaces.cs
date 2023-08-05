public interface IInteractable
{
    public void Interact();
}

public interface IHitable
{
    public void Hit(float damage);

    void Dead();
}

public interface IHealable
{
    public void Heal(float heal);
}