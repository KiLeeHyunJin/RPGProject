using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ServerData;

public class Ability 
{
    public Ability(AbilityServerData abilityServerData)
    {

    }
    public int jumpPower; //점프파워
    public int moveSpeed; //이동속도
    public int atckSpeed; //공격속도

    public int atckAddition; //추가타

    public int atckPower; //공격력
    public int magicPower; //마력

    public int defence; //방어력

    public int accuracy; //명중률
    public int ciritical; //치명타
    public int avoidance; //회피율

    public int[] point;

    public int hp;
    public int mp;
    public int exp;
}
