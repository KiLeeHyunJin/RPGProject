using System;
using System.Collections.Generic;


public class ServerData
{
    [Serializable]
    public class Name
    {
        public string nickName;
        public string userId;
    }
    [Serializable]
    public class UserServerData
    {
        public UserServerData()
        {
            characters = new();
        }
        public Dictionary<string, CharacterServerData> characters;
    }
    [Serializable]
    //앞이 낮은 위치 뒤가 높은 위치 
    public class CharacterServerData
    {
        public string nickName; //닉네임
        public short jobLv;     //직업, 레벨(1바이트 씩)

        public long cloth;      //모자, 상의, 하의, 신발, 장갑, 무기, 
        public int skin;        //머리, 얼굴, 피부    (1바이트 씩)

        public AbilityServerData ability;      //능력치
        public InventoryServerData inventory;  //인벤
    }
    [Serializable]
    public class AbilityServerData
    {
        public int speed;       //이동속도, 점프력,  공격속도, 추가타)
        public int atck;        //물리공격, 마법공격, 치명 데미지)
        public int other;       //명중률, 치명률, 방어력, 회피력, 

        public int stat;        //스탯 (1바이트 씩)

        public string skills;

        public long point;      //능력치, 0차, 1차, 2차, 3차, 4차   
    }

    [Serializable]
    public class InventoryServerData
    {
        public InventoryServerData()
        {
            ect = new();
            equip = new();
            consume = new();
        }
        public void SetInvenSize(int equipCount, int consumeCount, int ectCount)
        {
            equip.Capacity = equipCount;
            consume.Capacity = consumeCount;
            ect.Capacity = ectCount;
        }

        public List<ItemEctServerData> consume;
        public List<ItemConsumeServerData> ect;
        public List<ItemEquipServerData> equip;

        public long money;
        public int slotCount;
    }

    public class ItemEctServerData
    {
        public string itemName; //이름
        public long code;       //종류, 카테고리, 이미지, 스크립터블
        public long itemData;   //타입, 레벨, 개수, 작수,

    }
    public class ItemConsumeServerData : ItemEctServerData
    {
        public long addAbility; //강화 타입, 효과 타입, 값, 지속 시간
    }

    [Serializable]
    public class ItemEquipServerData : ItemConsumeServerData
    {
        public int limitStat;   //힘, 민첩, 지력, 운,
        //addAbility; //힘, 민첩, 지력, 운, 공격, 마법, 방어, 이속, 
        public long addStat;    //힘, 민첩, 지력, 운, 공격, 마법, 방어, 이속, 
        public long upgradeStat;//힘, 민첩, 지력, 운, 공격, 마법, 방어, 이속, 
    }

    [Serializable]
    public class StorageBox
    {
        public List<ItemEquipServerData> storage;
    }

}
