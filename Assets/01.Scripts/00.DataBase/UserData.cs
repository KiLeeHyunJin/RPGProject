using System;
using System.Collections.Generic;

[Serializable]
public class UserData
{
    [Serializable]
    public class User
    {
        public Dictionary<string, Character> characters;

        public User()
        {
            characters = new Dictionary<string, Character>();
        }
    }

    [Serializable]
    public class Character
    {
        public Ability ability;
        public string job;
        public Inventory inventory;
        public int level;
        public string nickName;
        public string skill;

        public Character()
        {
            ability = new Ability();
            inventory = new Inventory();
        }
    }

    [Serializable]
    public class Ability
    {
        public float accuracy;
        public float atckPower;
        public float atckSpeed;
        public float defence;
        public float jumpPower;
        public float magicPower;
        public float moveSpeed;
        public int point;
    }

    [Serializable]
    public class Inventory
    {
        public List<Item> consume;
        public List<Item> ect;
        public List<Item> equip;
        public List<Item> money;
    }

    [Serializable]
    public class Item
    {
        public string itemName;
        public int itemCount;
    }
}

