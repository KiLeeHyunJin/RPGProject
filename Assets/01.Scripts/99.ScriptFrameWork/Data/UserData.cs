using System;
using System.Collections.Generic;

[Serializable]
public class UserData
{
    [Serializable]
    public class Name
    {
        public string nickName;
        public string userId;
    }


    [Serializable]
    public class User
    {
        public Dictionary<string,Character> characters;

        public User()
        {
            characters = new();
        }
    }

    [Serializable]
    public class Character
    {
        public Ability ability;
        public Inventory inventory;

        public string job;
        public int level;
        public string nickName;
        public string skill;
        public ulong skin;
        public Stat stat;


        public Character()
        {
            ability = new Ability();
            inventory = new Inventory();
        }
    }

    [Serializable]
    public class Ability
    {
        public float moveSpeed;
        public float atckSpeed;
        public float jumpPower;

        public float atckPower;
        public float defence;
        public float magicPower;

        public float accuracy;
        public int point;
    }

    [Serializable]
    public class Stat
    {
        public int def;
        public int luk;
        public int man;
        public int str;
    }

    [Serializable]
    public class Inventory
    {
        public Inventory()
        {
            consume = new();
            ect = new();
            equip = new();
            consume = new();
        }
        public List<Item> consume;
        public List<Item> ect;
        public List<Item> equip;
        public long money;
    }

    [Serializable]
    public class Item
    {
        public ulong addAbility;
        public ulong addStat;

        public ulong itemData;
        public string itemName;
        public ulong limitStat;
        public ulong upgradeStat;
    }


}

