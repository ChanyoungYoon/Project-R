using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float health = 10;

    [SerializeField] private float max_Health = 10;
    // 오브젝트 사망시 사망했다는 이벤트 받아오기
    //https://truecode.tistory.com/86
    //public delegate void CheckDie();
    //public static event CheckDie isDie;

    public float CurrentHealth
    {
        get { return health; }
        set { health = value;  }
    }

    public float MaxHealth
    {
        get { return max_Health; }
        set { max_Health = value; }
    }

    private bool check = false;
    public void Damage(float amount)
    {
        if(amount < 0)
        {
            throw new System.ArgumentOutOfRangeException("Can't have negative Damage");
        }

        this.health -= amount;

        if (health <= 0)
        {
            check = true;
        }

    }

    public void Heal(int amount)
    {
        if (amount < 0)
        {
            throw new System.ArgumentOutOfRangeException("Can't have negative Healing");
        }

        bool checkMaxHealth = health + amount > max_Health;
        if (checkMaxHealth)
        {
            this.health = max_Health;
        }
        else
        {
            this.health += amount;
        }
    }

    public bool isDie()
    {
        if (check)
        {
            return true;
        }
        return false;
    }



}
