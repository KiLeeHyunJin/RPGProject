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

    






    #endregion Item

}
