using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    [SerializeField]
    protected int MaxHP = 100;

    [SerializeField]
    protected int currentHP;

    [SerializeField]
    protected int Damage = 1;

    [SerializeField]
    protected int crasDamage = 100;

    [SerializeField]
    bool isDead = false;

    public bool IsDead
    {
        get
        {
            return isDead;
        }
    }

    protected int CrashDamage
    {
        get
        {
            return CrashDamage;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    protected virtual void Initialize()
    {
        currentHP = MaxHP;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateActor();
    }

    protected virtual void UpdateActor()
    {

    }

    public virtual void OnBulletHited(int damage)
    {
        Debug.Log("OnBulletHited damage = " + damage);
        DecreaseHP(damage);
    }

    public virtual void OnCrash(int damage)
    {
        Debug.Log("OnCrash damage = " + damage);
    }

    void DecreaseHP(int value)
    {
        if (isDead)
            return;
        
        currentHP -= value;

        if (currentHP < 0)
            currentHP = 0;

        if (currentHP == 0)
        {
            OnDead();
        }

    }

    protected virtual void OnDead()
    {
        Debug.Log(name + " OnDead");
        isDead = true;
    }
}
