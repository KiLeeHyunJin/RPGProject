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
        public List<ItemEctServerData> ect;
        public List<ItemEquipServerData> equip;

        public long money;
        public int slotCount;
    }

    public class ItemEctServerData
    {
        public int code;       //종류 , 개수 , 카테고리
        public (int itemType, int count, int scriptable) ParseCode()
        {
            int _type = code.ExtractByte(DataDefine.IntSize.One);
            int _category = code.ExtractByte(DataDefine.IntSize.Two);
            int _count = code.ExtractByte(DataDefine.IntSize.Three);

            return (_type, _count, _category);
        }
        public Item ExtractItem()
        {
            Item ect = new();
            ect.Init(ParseCode());
            return ect;
        }
    }

    [Serializable]
    public class ItemEquipServerData : ItemEctServerData
    {
        public int itemData;   //장착타입, 장착 레벨 , 작수,
        public int upgradeStat;//힘, 민첩, 지력, 운,
        public int upgradeAdditional;//공격, 마법, 방어, 이속, 
        public (Define.EquipType wearType, int level, int possable) ParseData()
        {
            return
                (
                    (Define.EquipType)itemData.ExtractByte(DataDefine.IntSize.One),
                    itemData.ExtractByte(DataDefine.IntSize.Two),
                    itemData.ExtractByte(DataDefine.IntSize.Three)
                );
        }

        public (Stat stat, AdditionalStat additional) ParseUpgradeStat()
        {
            Stat stat = new(
                    upgradeStat.ExtractByte(DataDefine.IntSize.One),
                    upgradeStat.ExtractByte(DataDefine.IntSize.Two),
                    upgradeStat.ExtractByte(DataDefine.IntSize.Three),
                    upgradeStat.ExtractByte(DataDefine.IntSize.Four));

            AdditionalStat additional = new(
                    upgradeAdditional.ExtractByte(DataDefine.IntSize.One),
                    upgradeAdditional.ExtractByte(DataDefine.IntSize.Two),
                    upgradeAdditional.ExtractByte(DataDefine.IntSize.Three),
                    upgradeAdditional.ExtractByte(DataDefine.IntSize.Four));

            return (stat, additional);
        }
        new public Equip ExtractItem()
        {
            Equip equip = new();
            equip.Init(ParseCode());
            equip.EquipInit(ParseData());
            (equip.upgradeStat, equip.upgradeAdditional) = ParseUpgradeStat();
            return equip;
        }
    }

    [Serializable]
    public class StorageBox
    {
        public List<ItemEquipServerData> storage;
    }

}
