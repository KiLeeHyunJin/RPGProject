using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.InputSystem;
using static Define;

public class KeyController
{
    readonly string keyMapName;
    readonly List<bool> usedKey;

    InputActionMap actionMap;
    public List<KeyData> keyDataSet;
    public List<int> keyDataServer;
    bool this[Key key]
    {
        get { return usedKey[(int)key]; }
        set { usedKey[(int)key] = value; }
    }

    public KeyController(InputActionAsset inputActions)
    {
        keyMapName = "Player";
        Key[] keysArray = Utils.GetEnumArray<Key>();
        usedKey = new((int)keysArray[Define.EndIndex]);
        keyDataServer = new((int)keysArray[Define.EndIndex]);

        Init(inputActions, keysArray);
    }

    void Init(InputActionAsset inputActions, Key[] keysArray)
    {
        //인풋시스템을 비활성화합니다.
        inputActions.Disable();

        //액션 맵을 생성합니다. 
        actionMap = inputActions.FindActionMap(keyMapName);
        if (actionMap == null)
            inputActions.AddActionMap(keyMapName);

        StringBuilder stringBuilder = new();

        for (int i = 0; i < keysArray.Length; i++)
        {
            stringBuilder.Append(keysArray[i].ToString());
            if (actionMap.FindAction(stringBuilder.ToString()) == null)
                actionMap.AddAction(keysArray[i].ToString(), InputActionType.Button, binding: $"<Keyboard>/{keysArray[i]}");
            stringBuilder.Clear();
        }
        //인풋시스템을 활성화합니다.
        inputActions.Enable();
    }

    public void LoadKeySet()
    {

    }

    public void RemoveKey(Key keyCode)
    {
        keyDataServer[(int)keyCode] = 0;
    }

    public void AddKey(Key keyCode, Action action,Define.QuickSlotType quickSlotType, int idx , InputActionType inputType = InputActionType.Button)
    {
        SetKeyDataServer(keyCode, quickSlotType, idx);

        if (this[keyCode] == true)
        {
            GetKeyAction(keyCode).Reset();
        }
        else
        {

        }
    }


    InputAction GetKeyAction(Key keyCode)
    {
        InputAction action = actionMap.FindAction(keyCode.ToString());
        if (action == null)
            action = actionMap.AddAction(keyCode.ToString(), InputActionType.Button);
        return action;
    }



    void SetKeyDataServer(Key keyCode, Define.QuickSlotType quickSlotType, int idx)
    {
        int setServerData = default;
        setServerData |= ((int)keyCode).Shift(DataDefine.IntSize.One);
        setServerData |= ((int)quickSlotType).Shift(DataDefine.IntSize.One);
        setServerData |= idx.Shift(DataDefine.IntSize.Two);
        keyDataServer[(int)keyCode] = setServerData;
    }

    (Key key, QuickSlotType quickSlot, int idx) ParseKeySlotData(int value)
    {
        int setServerData = value;
        Key key = (Key)setServerData.ExtractByte(DataDefine.IntSize.One);
        QuickSlotType quickSlot = (QuickSlotType)setServerData.ExtractByte(DataDefine.IntSize.Two);
        int idx = setServerData.ExtractByte(DataDefine.IntSize.Three);
        return (key, quickSlot, idx);
    }

    public struct KeyData
    {
        public Key key;
        public QuickSlotType slotType;
        public int idx;
    }

   
}

