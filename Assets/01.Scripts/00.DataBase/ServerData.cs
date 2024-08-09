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
        public ServerKeyData keySet;
        public SkillServerData skill;
    }
    [Serializable]
    public class ServerKeyData
    {
        public int[] keyData;
    }

    [Serializable]
    public class AbilityServerData
    {
        public int hp;
        public int mp;
        public int exp;

        public int speed;       //이동속도, 점프력,  공격속도, 추가타)
        public int atck;        //물리공격, 마법공격, 치명 데미지)
        public int other;       //명중률, 치명률, 방어력, 회피력, 

        public int stat;        //스탯 (1바이트 씩)

        public string skills;

        public int point;      //능력치, 0차, 1차, 2차s
    }

    [Serializable]
    public class SkillServerData
    {
        public SkillServerData()
        {
            skillDatas = new SkillDataServerData[(int)Define.CharacterPointType.END];
        }
        public SkillDataServerData[] skillDatas;
    }

    [Serializable]
    public class SkillDataServerData
    {
        public SkillDataServerData()
        {
            skillData = new int[(int)Define.SkillDefaultSize];
        }
        public int[] skillData;
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
        public InventoryServerData(int slotSize)
        {
            ect = new(slotSize);
            equip = new(slotSize);
            consume = new(slotSize);
            for (int i = 0; i < slotSize; i++)
            {
                ect.Add(new ItemEctServerData());
                consume.Add(new ItemEctServerData());
                equip.Add(new ItemEquipServerData());
            }
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

    [Serializable]
    public class ItemEctServerData
    {
        public ItemEctServerData()
        {
            code = 0;
        }
        public int code;       //종류 , 개수 , 카테고리
        public (int itemType, int count, int category) ParseCode()
        {
            int _type = code.ExtractByte(DataDefine.IntSize.One);
            int _count = code.ExtractByte(DataDefine.IntSize.Two);
            int _category = code.ExtractByte(DataDefine.IntSize.Three);

            return (_type, _count, _category);
        }
    }

    [Serializable]
    public class ItemEquipServerData : ItemEctServerData
    {
        public ItemEquipServerData()
        {
            code = 0;
        }
        public int itemData;   // 작수,
        public int upgradeStat;//힘, 민첩, 지력, 운,
        public int upgradeAdditional;//공격, 마법, 방어, 이속, 

        public int ParseData()
        {
            return itemData.ExtractByte(DataDefine.IntSize.One);
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

        public Equip ExtractItem()
        {
            (int type, int count, int category) = ParseCode();
            (Stat upgradeStat, AdditionalStat upgradeAdditional) = ParseUpgradeStat(); //강화정보
            
            (Equip equip, Action<int, Stat, AdditionalStat> InitServerData) = Manager.Data.GameItemData.GetEquipItem((Define.ItemType)type, count, category) ;
            InitServerData(ParseData(),upgradeStat, upgradeAdditional);
            return equip;
        }
    }

    [Serializable]
    public class StorageBox
    {
        public List<ItemEquipServerData> storage;
    }

}
