using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerData
{
    public class USerServerData
    {
        public Dictionary<string, CharacterServerData> characters;

        public USerServerData()
        {
            characters = new();
        }
    }

    public class CharacterServerData
    {
        public string nickName; //닉네임
        public short jobLv;     //직업, 레벨(1바이트 씩)

        public long cloth;      //모자, 상의, 하의, 신발, 장갑, 무기, 
        public int skin;        //외형    (1바이트 씩)

        public AbilityServerData ability;      //능력치
        public InventoryServerData inventory;  //인벤
    }

    public class AbilityServerData
    {
        public int speed;       //이동속도, 점프력,  공격속도, 추가타)
        public int atck;        //물리공격, 마법공격, 치명 데미지)
        public int other;       //명중률, 치명률, 방어력, 회피력, 

        public int stat;        //스탯 (1바이트 씩)

        public string skills;

        public long point;      //능력치, 0차, 1차, 2차, 3차, 4차   
    }


    public class InventoryServerData
    {
        public void SetInvenSize(int equipCount, int consumeCount, int ectCount)
        {
            equip.Capacity = equipCount;
            consume.Capacity = consumeCount;
            ect.Capacity = ectCount;
        }

        public InventoryServerData()
        {
            ect = new();
            equip = new();
            consume = new();
        }

        public List<ItemServerData> consume;
        public List<ItemServerData> ect;
        public List<ItemServerData> equip;
        public long money;
    }

    public class ItemServerData
    {
        public string itemName; //이름

        public long code;       //종류, 카테고리, 이미지, 스크립터블
        public long itemData;   //타입, 레벨, 개수, 작수,

        public int limitStat;   //힘, 민첩, 지력, 운,

        public long addAbility; //힘, 민첩, 지력, 운, 공격, 마법, 방어, 이속, 
        public long addStat;    //힘, 민첩, 지력, 운, 공격, 마법, 방어, 이속, 
        public long upgradeStat;//힘, 민첩, 지력, 운, 공격, 마법, 방어, 이속, 
    }

    public class StorageBox
    {
        public List<ItemServerData> storage;
    }

}
