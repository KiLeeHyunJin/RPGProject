using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using UnityEngine.InputSystem;
using static Define;
using static System.Runtime.CompilerServices.RuntimeHelpers;

[Serializable]
public partial class KeyController
{
    const string KeyMapName = "DynamicKeySetting";
    const string KeyBindingFirstName = "<Keyboard>/";
    readonly UserCharacterController controller;
    readonly InputActionMap actionMap;
    readonly Action<InputAction.CallbackContext>[] keyActionArray;

    [SerializeField] List<int> keyDataServer;

    public List<KeyData> keyDataSet;
    List<KeyData> KeyDataList { get { return keyDataSet; } }
    bool this[Key key]  {   get { return keyDataSet[(int)key].state; }  }

    public KeyController(InputActionAsset inputActions, UserCharacterController userController)
    {
        controller = userController;
        Key[] keysArray = Utils.GetEnumArray<Key>();
        int endNum = keysArray.Length;

        actionMap = inputActions.FindActionMap(KeyMapName);
        actionMap ??= inputActions.AddActionMap(KeyMapName);

        keyDataSet = new(endNum);
        keyDataServer = new(endNum);
        keyActionArray = new Action<InputAction.CallbackContext>[endNum];
        Init(keysArray);
    }

    void Init( Key[] keysArray)
    {
        InputAction inputAction;
        for (int i = 0; i < keysArray.Length; i++)
        {
            keyDataServer.Add(0);
            keyActionArray[i] = null;
            KeyDataList.Add(new() { key = keysArray[i] });
            inputAction = actionMap.FindAction(keysArray[i].ToString());
            if (inputAction != null)
            {
                inputAction.RemoveAction();
            }
            actionMap.AddAction(keysArray[i].ToString(), binding: $"{KeyBindingFirstName}{keysArray[i]}");
        }
        //인풋시스템을 활성화합니다.
        //inputActions.Enable();
    }
    public void TestKey()
    {
        UnityEngine.Message.Log("sdad");
    }

    public void ResetKey()
    {
        Key[] keysArray = Utils.GetEnumArray<Key>();
        InputAction inputAction;
        actionMap.Disable();
        for (int i = 0; i < keysArray.Length; i++)
        {
            inputAction = actionMap.FindAction(keysArray[i].ToString());
            if (inputAction != null)
            {
                RemoveKey(keysArray[i]);
            }
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
            if(keyParseData.state)
                SetKeyMapping(keyParseData, actionMap.FindAction(keyParseData.key.ToString()));
        }
    }


    void SetKeyMapping(KeyData keyData, InputAction inputAction)
    {
        KeyDataList[(int)keyData.key] = keyData;
        Action<InputAction.CallbackContext> action = keyData.slotType switch
        {
            QuickSlotType.Default => null,
            QuickSlotType.Item => (callBackt) => controller.Inventory.UseItem(ItemType.Consume, keyData.idx),
            QuickSlotType.Skill => null,
            _ => null,
        };

        if (action == null)
            return;

        inputAction.performed += action;
        keyActionArray[(int)keyData.key] = action;
    }


    public void AddKey(Key keyCode, Action action,Define.QuickSlotType quickSlotType, int idx , InputActionType inputType = InputActionType.Button)
    {
        if (this[keyCode] == true)
            RemoveKey(keyCode);


        actionMap.Disable();
        InputAction keyAction = actionMap.FindAction(keyCode.ToString());
        if(keyAction != null)
        {
            keyAction.performed -= keyActionArray[(int)keyCode];
        }
        else
        {
            keyAction = actionMap.AddAction(keyCode.ToString(), binding: $"{KeyBindingFirstName}{keyCode}");
        }
        SetKeyDataServer(keyCode, quickSlotType, idx);
        void inputAction(InputAction.CallbackContext call) { action?.Invoke(); }
        keyActionArray[(int)keyCode] = inputAction;
        keyAction.performed += inputAction;

        actionMap.Enable();
    }

    public void RemoveKey(Key keyCode)
    {
        keyDataServer[(int)keyCode] = 0;
        KeyDataList[(int)keyCode].Remove();
        InputAction keyAction = actionMap.FindAction(keyCode.ToString());
        if (keyAction != null)
        {
            actionMap.Disable();
            Action<InputAction.CallbackContext> action = keyActionArray[(int)keyCode];
            if (action != null)
            {
                keyAction.performed -= action;
            }
            else
            {
                keyAction.RemoveAction();
                actionMap.AddAction(keyCode.ToString(), binding: $"{KeyBindingFirstName}{keyCode}");
            }
            actionMap.Enable();
        }
    }



    InputAction GetKeyAction(Key keyCode)
    {
        InputAction action = actionMap.FindAction(keyCode.ToString());
        action ??= actionMap.AddAction(keyCode.ToString(), InputActionType.Button, binding: $"{KeyBindingFirstName}{keyCode}");
        return action;
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



