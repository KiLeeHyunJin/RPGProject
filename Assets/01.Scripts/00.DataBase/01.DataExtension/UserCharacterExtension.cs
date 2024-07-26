using static DataDefine;
using static Define;
using static ServerData;
using static UnityEditor.Progress;

public static class UserCharacterExtension
{
    #region Character
    public static int ParseJob(this CharacterServerData character)
    {
        return character.jobLv.ExtractByte(DataDefine.ShortSize.One);
    }
    public static int ParseLevel(this CharacterServerData character)
    {
        return character.jobLv.ExtractByte(DataDefine.ShortSize.Two);
    }
    public static (int cap, int shirt, int pant, int shoes, int glove, int weapon) ParseCloth(this CharacterServerData character)
    {
        return
            (
                character.cloth.ExtractByte(DataDefine.LongSize.One),
                character.cloth.ExtractByte(DataDefine.LongSize.Two),
                character.cloth.ExtractByte(DataDefine.LongSize.Three),
                character.cloth.ExtractByte(DataDefine.LongSize.Four),
                character.cloth.ExtractByte(DataDefine.LongSize.Five),
                character.cloth.ExtractByte(DataDefine.LongSize.Six)
            );
    }
    public static (int hair, int face, int skin) ParseSkin(this CharacterServerData character)
    {
        return
            (
                character.skin.ExtractByte(DataDefine.IntSize.One),
                character.skin.ExtractByte(DataDefine.IntSize.One),
                character.skin.ExtractByte(DataDefine.IntSize.One)
            );
    }
    #endregion Character

    #region Stat

    public static (int moveSpeed, int jumpPower, int atckSpeed, int addAtck) ParseSpeed(this AbilityServerData ability)
    {
        return
            (
                ability.speed.ExtractByte(DataDefine.IntSize.One),
                ability.speed.ExtractByte(DataDefine.IntSize.Two),
                ability.speed.ExtractByte(DataDefine.IntSize.Three),
                ability.speed.ExtractByte(DataDefine.IntSize.Four)
            );
    }
    public static (int baseAtck, int magicAtck, int crtAtck) ParseAtck(this AbilityServerData ability)
    {
        return
            (
                ability.atck.ExtractByte(DataDefine.IntSize.One),
                ability.atck.ExtractByte(DataDefine.IntSize.Two),
                ability.atck.ExtractByte(DataDefine.IntSize.Three)
            );
    }
    public static (int accuracy, int ciritical, int defence, int avoidance) ParseOther(this AbilityServerData ability)
    {
        return
            (
                ability.other.ExtractByte(DataDefine.IntSize.One),
                ability.other.ExtractByte(DataDefine.IntSize.Two),
                ability.other.ExtractByte(DataDefine.IntSize.Three),
                ability.other.ExtractByte(DataDefine.IntSize.Four)
            );
    }
    public static (int str, int def, int man, int luk) ParseStat(this AbilityServerData ability)
    {
        return
            (
                ability.stat.ExtractByte(DataDefine.IntSize.One),
                ability.stat.ExtractByte(DataDefine.IntSize.Two),
                ability.stat.ExtractByte(DataDefine.IntSize.Three),
                ability.stat.ExtractByte(DataDefine.IntSize.Four)
            );
    }
    public static (int ability, int zero, int one, int two, int three, int four) ParsPoint(this AbilityServerData ability)
    {
        return
            (
                ability.point.ExtractByte(DataDefine.LongSize.One),
                ability.point.ExtractByte(DataDefine.LongSize.Two),
                ability.point.ExtractByte(DataDefine.LongSize.Three),
                ability.point.ExtractByte(DataDefine.LongSize.Four),
                ability.point.ExtractByte(DataDefine.LongSize.Five),
                ability.point.ExtractByte(DataDefine.LongSize.Six)
            );
    }
    #endregion Stat

    #region Inventory
    public static (int equip, int consume, int ect) ParseSlot(this InventoryServerData inventory)
    {
        return
            (
                inventory.slotCount.ExtractByte(DataDefine.IntSize.One),
                inventory.slotCount.ExtractByte(DataDefine.IntSize.Two),
                inventory.slotCount.ExtractByte(DataDefine.IntSize.Three)
            );
    }
    #endregion Inventory

    #region Item

    public static (int itemType, int count, int img, int scriptable) ParseCode(this ItemEctServerData item)
    {

        int _type = item.code.ExtractByte(DataDefine.IntSize.One);
        int _count = item.code.ExtractByte(DataDefine.IntSize.Two);
        int _img = item.code.ExtractByte(DataDefine.IntSize.Three);
        int _scrip = item.code.ExtractByte(DataDefine.IntSize.Four);

        return (_type, _count, _img, _scrip);
    }
    public static (Define.HealType addType, Define.ConsumeType efxType, int value, int stayTIme) ParseAddAbilityConsume(this ItemConsumeServerData item)
    {
        return
            (
                (Define.HealType)item.addAbility.ExtractByte(DataDefine.IntSize.One),
                (Define.ConsumeType)item.addAbility.ExtractByte(DataDefine.IntSize.Two),
                item.addAbility.ExtractByte(DataDefine.IntSize.Three),
                item.addAbility.ExtractByte(DataDefine.IntSize.Four)
            );
    }

    public static (Stat stat, AdditionalStat additional) ParseAddAbilityEquip(this ItemEquipServerData item)
    {
        
        Stat stat = new
            (
            item.addAbility.ExtractByte(DataDefine.IntSize.One),
            item.addAbility.ExtractByte(DataDefine.IntSize.Two),
            item.addAbility.ExtractByte(DataDefine.IntSize.Three),
            item.addAbility.ExtractByte(DataDefine.IntSize.Four)
            );
        AdditionalStat additional = new(
            item.addAbility.ExtractByte(DataDefine.IntSize.One),
            item.addAbility.ExtractByte(DataDefine.IntSize.Two),
            item.addAbility.ExtractByte(DataDefine.IntSize.Three),
            item.addAbility.ExtractByte(DataDefine.IntSize.Four));

        return (stat, additional);
    }

    public static Stat ParseLimitStat(this ItemEquipServerData item)
    {
        Stat stat = new(
                item.limitStat.ExtractByte(DataDefine.IntSize.One),
                item.limitStat.ExtractByte(DataDefine.IntSize.Two),
                item.limitStat.ExtractByte(DataDefine.IntSize.Three),
                item.limitStat.ExtractByte(DataDefine.IntSize.Four));

        return stat;
    }

    public static (Stat stat, AdditionalStat additional) ParseAddStat(this ItemEquipServerData item)
    {
        Stat stat = new(
                item.baseStat.ExtractByte(DataDefine.IntSize.One),
                item.baseStat.ExtractByte(DataDefine.IntSize.Two),
                item.baseStat.ExtractByte(DataDefine.IntSize.Three),
                item.baseStat.ExtractByte(DataDefine.IntSize.Four));

        AdditionalStat additional = new(
                item.baseAdditional.ExtractByte(DataDefine.IntSize.One),
                item.baseAdditional.ExtractByte(DataDefine.IntSize.Two),
                item.baseAdditional.ExtractByte(DataDefine.IntSize.Three),
                item.baseAdditional.ExtractByte(DataDefine.IntSize.Four));

        return (stat, additional);

    }
    public static (Stat stat, AdditionalStat additional) ParseUpgradeStat(this ItemEquipServerData item)
    {
        Stat stat = new(
                item.upgradeStat.ExtractByte(DataDefine.IntSize.One),
                item.upgradeStat.ExtractByte(DataDefine.IntSize.Two),
                item.upgradeStat.ExtractByte(DataDefine.IntSize.Three),
                item.upgradeStat.ExtractByte(DataDefine.IntSize.Four));

        AdditionalStat additional = new(
                item.upgradeAdditional.ExtractByte(DataDefine.IntSize.One),
                item.upgradeAdditional.ExtractByte(DataDefine.IntSize.Two),
                item.upgradeAdditional.ExtractByte(DataDefine.IntSize.Three),
                item.upgradeAdditional.ExtractByte(DataDefine.IntSize.Four));

        return (stat, additional);
    }
    public static (Define.EquipType wearType, int level, int category, int possable) ParseData(this ItemEquipServerData item)
    {
        return
            (
                (Define.EquipType)item.itemData.ExtractByte(DataDefine.IntSize.One),
                item.itemData.ExtractByte(DataDefine.IntSize.Two),
                item.itemData.ExtractByte(DataDefine.IntSize.Three),
                item.itemData.ExtractByte(DataDefine.IntSize.Four)
            );
    }
    public static Item ExtractItem(this ItemEctServerData item)
    {
        Item ect = new(item.ParseCode());
        return ect;
    }
    public static Consume ExtractItem(this ItemConsumeServerData item)
    {
        Consume consume = new(item.ParseCode(), item.ParseAddAbilityConsume());
        return consume;
    }
    public static Equip ExtractItem(this ItemEquipServerData item)
    {
        (int itemType, int count, int img, int scriptable) value = item.ParseCode();
        (EquipType wearType, int level, int category, int possable) _value = item.ParseData();
        Equip equip = new(value, _value)
        {
            limitStat = item.ParseLimitStat()
        };
        AdditionalStat temp;
        (equip.addAbility, temp) = item.ParseAddAbilityEquip();

        (equip.baseStat, equip.baseAdditional) = item.ParseAddStat();
        (equip.upgradeStat, equip.upgradeAdditional) = item.ParseUpgradeStat();

        return equip;
    }



    #endregion Item

}
