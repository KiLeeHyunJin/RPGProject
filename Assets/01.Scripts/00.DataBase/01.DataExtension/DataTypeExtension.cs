using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataTypeExtension 
{
    /// <summary>
    /// Ư�� ��ġ�� ����Ʈ�� �����Ѵ�.
    /// </summary>
    /// 
    public static int ExtractByte(this long mover, DataDefine.LongSize extract)
    {
        int lastPosValue = (int)Utils.GetEnumArray<DataDefine.LongSize>()[Define.EndIndex];
        int leftShiftValue = lastPosValue - (int)extract;
        mover <<= leftShiftValue;
        return (int)mover >> lastPosValue;
    }

    public static int ExtractByte(this int mover, DataDefine.IntSize extract)
    {
        int lastPosValue = (int)Utils.GetEnumArray<DataDefine.IntSize>()[Define.EndIndex];
        int leftShiftValue = lastPosValue - (int)extract;
        mover <<= leftShiftValue;
        return mover >> lastPosValue;
    }

    public static int ExtractByte(this short mover, DataDefine.ShortSize extract)
    {
        int computeValue = mover;
        int lastPosValue = (int)Utils.GetEnumArray<DataDefine.ShortSize>()[Define.EndIndex];
        int leftShiftValue = lastPosValue - (int)extract;
        computeValue <<= leftShiftValue;
        return computeValue >> lastPosValue;
    }

    /// <summary>
    /// Ư�� ��ġ�� ��Ʈ�� �о��.
    /// </summary>
    public static short Shift(this short mover, DataDefine.ShortSize shiftValue)
    {
        return (short)(mover << (DataDefine.Byte * (int)shiftValue));
    }

    public static int Shift(this int mover, DataDefine.IntSize shiftValue)
    {
        return (int)(mover << (DataDefine.Byte * (int)shiftValue));
    }

    public static long Shift(this long mover, DataDefine.LongSize shiftValue)
    {
        return (long)(mover << (DataDefine.Byte * (int)shiftValue));
    }

}
