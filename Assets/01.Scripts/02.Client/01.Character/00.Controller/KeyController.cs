using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.InputSystem;
using static Define;

public partial class KeyController
{
    readonly string keyMapName;
    readonly List<bool> usedKey;

    InputActionMap actionMap;
    UserCharacterController controller;
    public List<KeyData> keyDataSet;
    public List<int> keyDataServer;
    bool this[Key key]
    {
        get { return usedKey[(int)key]; }
        set { usedKey[(int)key] = value; }
    }

    public KeyController(InputActionAsset inputActions, UserCharacterController userController)
    {
        keyMapName = "DynamicKeySetting";
        controller = userController;
        Key[] keysArray = Utils.GetEnumArray<Key>();
        int endNum = (int)keysArray[Define.EndIndex];

        keyDataSet = new(endNum);
        usedKey = new(endNum);
        keyDataServer = new(endNum);

        Init(inputActions, keysArray);
    }

    void Init(InputActionAsset inputActions, Key[] keysArray)
    {
        //인풋시스템을 비활성화합니다.
        inputActions.Disable();

        //액션 맵을 생성합니다. 
        actionMap = inputActions.FindActionMap(keyMapName);
        if (actionMap == null)
            actionMap = inputActions.AddActionMap(keyMapName);

        StringBuilder stringBuilder = new();
        InputAction inputAction;
        for (int i = 0; i < keysArray.Length; i++)
        {
            stringBuilder.Append(keysArray[i].ToString());
            inputAction = actionMap.FindAction(stringBuilder.ToString());
            if (inputAction == null)
            {
                actionMap.AddAction(keysArray[i].ToString(), InputActionType.Button, binding: $"<Keyboard>/{keysArray[i]}").performed += (cal) => { TestKey(); };
            }
            else
                inputAction.Reset();

            stringBuilder.Clear();
        }
        //인풋시스템을 활성화합니다.
        inputActions.Enable();
    }
    public void TestKey()
    {
        UnityEngine.Message.Log("sdad");
    }
    public void ResetKey()
    {
        Key[] keysArray = Utils.GetEnumArray<Key>();
        StringBuilder stringBuilder = new();
        InputAction inputAction;
        actionMap.Disable();
        for (int i = 0; i < keysArray.Length; i++)
        {
            stringBuilder.Append(keysArray[i].ToString());
            inputAction = actionMap.FindAction(stringBuilder.ToString());
            if (inputAction != null)
            {
                inputAction.Reset();
                inputAction.RemoveAction();
            }

            stringBuilder.Clear();
        }
        actionMap.Enable();
    }

    public void LoadKeySet(ServerData.ServerKeyData keyData)
    {
        int[] keyArray = keyData.keyData;
        for (int i = 0; i < keyArray.Length; i++)
        {
            if (keyArray[i] == 0)
                continue;
            
            KeyData keyParseData = ParseKeySlotData(keyArray[i]);
            this[keyParseData.key] = true;
            keyDataSet[(int)keyParseData.key] = keyParseData;
            SetKeyMapping(keyParseData.slotType,keyParseData.idx, actionMap.FindAction(keyParseData.key.ToString()));
        }
    }


    void SetKeyMapping(QuickSlotType slotType, int idx, InputAction inputAction)
    {
        switch (slotType)
        {
            case QuickSlotType.Default: //기본 제공 스킬
                break;
            case QuickSlotType.Item: //아이템 사용
                inputAction.Reset();
                inputAction.performed += (callBackt)=> controller.Inventory.UseItem(ItemType.Consume, idx);
                break;
            case QuickSlotType.Skill: //스킬 사용
                break;
        }
    }


    public void RemoveKey(Key keyCode)
    {
        keyDataServer[(int)keyCode] = 0;
        this[keyCode] = false;

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

    KeyData ParseKeySlotData(int value)
    {
        int setServerData = value;
        KeyData keyData = new();
        keyData.key = (Key)setServerData.ExtractByte(DataDefine.IntSize.One);
        keyData.slotType = (QuickSlotType)setServerData.ExtractByte(DataDefine.IntSize.Two);
        keyData.idx = setServerData.ExtractByte(DataDefine.IntSize.Three);
        return keyData;
    }

    public struct KeyData
    {
        public Key key;
        public QuickSlotType slotType;
        public int idx;
    }
}



