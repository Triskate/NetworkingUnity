using System;
using UnityEngine;
using UnityEngine.Events;

public class HumanoidLife : MonoBehaviour
{
    [SerializeField] float startingLife = 1f;
    [SerializeField] float currentLife;

    public UnityEvent<HumanoidLife, float> onLifeChanged;
    public UnityEvent<HumanoidLife> onLifeDepleted;

    HurtCollider hurtCollider;

    private void Awake()
    {
        hurtCollider = GetComponent<HurtCollider>();
        currentLife = startingLife;
    }

    private void OnEnable()
    {
        hurtCollider.onHurt.AddListener(OnHurt);
    }

    private void OnDisable()
    {
        hurtCollider.onHurt.RemoveListener(OnHurt);
    }

    private void OnHurt(IHitter arg0, HurtCollider arg1)
    {
        if (currentLife > 0)
        {
            currentLife -= arg0.GetDamage();
            onLifeChanged.Invoke(this, currentLife);
            if (currentLife < 0) { onLifeDepleted.Invoke(this); }
        }
    }

    internal void RestoreToStartingLife()
    {
        currentLife = startingLife;
    }
}
