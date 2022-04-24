using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 스탯 매니저. 값이 null이면 연산하지 않음, StatManager.Null을 부르면 전부 null인 스트럭트 반환, StatManager.Zero 부르면 전부 값이 0인 스트럭트 반환
// name이 중복인 값이 있으면 덮어씌움
public struct Stat{

    //Constructor
    public Stat(string _name,float? _moveSpeed,float? _health,float? _damage,float? _damageMultiplier){
        name = _name;
        moveSpeed = _moveSpeed;
        health = _health;
        damage = _damage;
        damageMultiplier = _damageMultiplier;
    }

    public string name;
    public float? moveSpeed;
    public float? health; 
    public float? damage; 
    public float? damageMultiplier;
}

public static class StatManager{

    public static float moveSpeed { get; private set; }
    public static float health { get; private set; }
    public static float damage { get; private set; }
    public static float damageMultiplier { get; private set; }


    public static Stat Null => new Stat("",null,null,null,null);
    public static Stat Zero => new Stat("",0,0,0,0);
    private static Stat baseStat = Zero;
    private static List<Stat> addStatList = new List<Stat>();

    private static void UpdateStat(){
        //set base stat
        moveSpeed = baseStat.moveSpeed==null?0:(float)baseStat.moveSpeed;
        health = baseStat.health==null?0:(float)baseStat.health;
        damage = baseStat.damage==null?0:(float)baseStat.damage;
        damageMultiplier = baseStat.damage==null?0:(float)baseStat.damageMultiplier;
        
        //add additive stat
        for (int i = 0; i < addStatList.Count; i++)
        {
            moveSpeed += addStatList[i].moveSpeed==null?0:(float)addStatList[i].moveSpeed;
            health += addStatList[i].health==null?0:(float)addStatList[i].health;
            damage += addStatList[i].damage==null?0:(float)addStatList[i].damage;
            damageMultiplier = addStatList[i].damage==null?0:(float)baseStat.damageMultiplier;
        }
    }

    public static void SetStat(Stat stat){
        baseStat = stat;
        UpdateStat();
    }

    public static void AddStat(Stat stat){
        for (int i = 0; i < addStatList.Count; i++)
        {
            if(stat.name.Equals(addStatList[i].name)){
                addStatList[i] = stat;
                return;
            }
        }
        UpdateStat();
    }
}
