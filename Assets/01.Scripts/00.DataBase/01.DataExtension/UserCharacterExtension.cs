using static ServerData;

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
    public static (int cap, int shirt, int pant, int shoes) ParseCloth(this CharacterServerData character)
    {
        return
            (
                character.cloth.ExtractByte(DataDefine.IntSize.One),
                character.cloth.ExtractByte(DataDefine.IntSize.Two),
                character.cloth.ExtractByte(DataDefine.IntSize.Three),
                character.cloth.ExtractByte(DataDefine.IntSize.Four)
            );
    }

    public static (int weapon, int glove) ParseOtherCloth(this CharacterServerData character)
    {
        return
            (
                character.otherCloth.ExtractByte(DataDefine.IntSize.One),
                character.otherCloth.ExtractByte(DataDefine.IntSize.Two)
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
    public static int[] ParsPoint(this AbilityServerData ability)
    {
        return new int[] 
        {
                ability.point.ExtractByte(DataDefine.IntSize.One),
                ability.point.ExtractByte(DataDefine.IntSize.Two),
                ability.point.ExtractByte(DataDefine.IntSize.Three),
                ability.point.ExtractByte(DataDefine.IntSize.Four)
        };
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








    #endregion Item

}
