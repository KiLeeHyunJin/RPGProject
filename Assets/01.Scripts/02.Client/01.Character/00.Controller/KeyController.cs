using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;
using static Define;

[Serializable]
public partial class KeyController
{
    const string KeyMapName = "DynamicKeySetting";
    const string KeyBindingFirstName = "<Keyboard>/";
    readonly UserCharacterController controller;
    readonly Action<InputAction.CallbackContext>[] keyActionArray;

    [SerializeField] List<int> keyDataServer;

    public List<KeyData> keyDataSet;
    List<KeyData> KeyDataList { get { return keyDataSet; } }
    bool this[Key key] { get { return keyDataSet[(int)key].state; } }

    public KeyController(InputActionAsset inputActions, UserCharacterController userController)
    {
        controller = userController;
        Key[] keysArray = Utils.GetEnumArray<Key>();
        int endNum = keysArray.Length;

        InputActionMap actionMap = inputActions.FindActionMap(KeyMapName);
        if (actionMap == null)
            actionMap = inputActions.AddActionMap(KeyMapName);

        keyDataSet = new(endNum);
        keyDataServer = new(endNum);
        keyActionArray = new Action<InputAction.CallbackContext>[endNum];
        Init(actionMap, keysArray);
    }

    void Init(InputActionMap actionMap, Key[] keysArray)
    {
        InputAction inputAction;
        actionMap.Disable();
        for (int i = 0; i < keysArray.Length; i++)
        {
            int idx = i;
            keyDataServer.Add(0);
            keyActionArray[i] = (call) =>TestKey();
            KeyDataList.Add(new() { key = keysArray[i] });
            inputAction = actionMap.FindAction(keysArray[i].ToString());
            if (inputAction != null)
            {
                inputAction.RemoveAction();
            }

            InputAction action = actionMap.AddAction(keysArray[i].ToString(), binding: $"{KeyBindingFirstName}{keysArray[i]}");

            action.performed += (call) => { keyActionArray[idx]?.Invoke(call); };
        }
        actionMap.Enable();
    }



    public void TestKey()
    {
        UnityEngine.Message.Log("sdad");
    }

    public void LoadKeySet(ServerData.ServerKeyData keyData)
    {
        int[] keyArray = keyData.keyData;
        for (int i = 0; i < keyArray.Length; i++)
        {
            KeyData keyParseData = ParseKeySlotData(keyArray[i]);
            if (keyParseData.state == false)
                continue;

            KeyDataList[(int)keyParseData.key] = keyParseData;
            keyActionArray[(int)keyParseData.key] = keyParseData.slotType switch
            {
                QuickSlotType.Default => null,
                QuickSlotType.Item => (callBackt) => controller.Inventory.UseItem(ItemType.Consume, keyParseData.idx),
                QuickSlotType.Skill => null,
                _ => null,
            };

            if (keyActionArray[(int)keyParseData.key] == null)
                return;
        }
    }




    public void ResetKey()
    {
        for (int i = 0; i < keyActionArray.Length; i++)
        {
            keyActionArray[i] = null;
        }
    }

    public void AddKey(Key keyCode, Action action, Define.QuickSlotType quickSlotType, int idx, InputActionType inputType = InputActionType.Button)
    {
        RemoveKey(keyCode);

        SetKeyDataServer(keyCode, quickSlotType, idx);
        void inputAction(InputAction.CallbackContext call) { action?.Invoke(); }
        keyActionArray[(int)keyCode] = inputAction;
    }

    public void RemoveKey(Key keyCode)
    {
        keyDataServer[(int)keyCode] = 0;
        KeyDataList[(int)keyCode].Remove();

        keyActionArray[(int)keyCode] = null;
    }

    void SetKeyDataServer(Key keyCode, Define.QuickSlotType quickSlotType, int idx, bool state = true)
    {
        int setServerData = default;
        setServerData |= ((int)keyCode).Shift(DataDefine.IntSize.One);
        setServerData |= ((int)quickSlotType).Shift(DataDefine.IntSize.One);
        setServerData |= idx.Shift(DataDefine.IntSize.Two);
        setServerData |= state ? int.MaxValue.Shift(DataDefine.IntSize.Four) : 0;
        KeyDataList[(int)keyCode].Add(keyCode, quickSlotType, idx);
        keyDataServer[(int)keyCode] = setServerData;
    }

    KeyData ParseKeySlotData(int value)
    {
        int setServerData = value;
        KeyData keyData = new()
        {
            key = (Key)setServerData.ExtractByte(DataDefine.IntSize.One),
            slotType = (QuickSlotType)setServerData.ExtractByte(DataDefine.IntSize.Two),
            idx = setServerData.ExtractByte(DataDefine.IntSize.Three),
            state = setServerData.ExtractByte(DataDefine.IntSize.Four) > 0
        };
        return keyData;
    }
    [Serializable]
    public class KeyData
    {
        public Key key;
        public QuickSlotType slotType;
        public int idx;
        public bool state;
        public void Remove()
        {
            idx = -1;
            state = false;
        }
        public void Add(Key _key, QuickSlotType _slotType, int _idx)
        {
            key = _key;
            slotType = _slotType;
            idx = _idx;
            state = true;
        }
    }

}



