using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataTypeExtension 
{

    public static System.Threading.Tasks.Task SetData(this Firebase.Database.FirebaseDatabase DB, string path,string json)
    {
        return DB.GetReference(path).SetRawJsonValueAsync(json);
    }

    public static bool IsNullOrEmpty(this string str)
    {
        return string.IsNullOrEmpty(str);
    }
    /// <summary>
    /// 특정 위치의 바이트를 추출한다.
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
    /// 특정 위치로 비트를 밀어낸다.
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
