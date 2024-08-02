using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public partial class KeyController
{
    const string KeyMapName = "DynamicKeySetting";
    const string KeyBindingFirstName = "<Keyboard>/";

    readonly UserCharacterController controller;
    readonly KeyActionData[] keyActionArray;

    KeyActionData this[Key keyCode] { get { return keyActionArray[(int)keyCode]; } }
    [SerializeField]
    readonly List<int> keyDataServer;
    //키의 액션 다양하게 
    //키 저장을 위한 keyDataServer 구조체 또는 직렬화 클래스
    public KeyController(InputActionAsset inputActions, UserCharacterController userController)
    {
        controller = userController;
        Key[] keysArray = Utils.GetEnumArray<Key>();
        keyDataServer = new(keysArray.Length);

        inputActions.Disable();
        InputActionMap actionMap = inputActions.FindActionMap(KeyMapName);
        actionMap ??= inputActions.AddActionMap(KeyMapName);
        Init(actionMap, keysArray, out keyActionArray);
        inputActions.Enable();
    }

    void Init(InputActionMap actionMap, Key[] keysArray, out KeyActionData[] keyActionDatas)
    {
        InputAction inputAction;
        keyActionDatas = new KeyActionData[keysArray.Length];
        for (int i = 0; i < keysArray.Length; i++)
        {
            keyDataServer.Add(0);

            inputAction = actionMap.FindAction(keysArray[i].ToString());
            inputAction?.RemoveAction();
            inputAction = actionMap.AddAction(keysArray[i].ToString(), binding: $"{KeyBindingFirstName}{keysArray[i]}");
            keyActionDatas[(int)keysArray[i]] = new(inputAction, (call) =>Test());
        }
        keyActionDatas[(int)Key.V].Attach(new(null, (call) => Test2(), null));
    }

    void Test() => Debug.Log("fgsdgsasd");
    void Test2() => Debug.Log("324235352");
    public void SwapTest() => SwapKey(Key.X, Key.V);

    public void LoadKeySet(ServerData.ServerKeyData keyData)
    {
        int[] keyArray = keyData.keyData;
        for (int i = 0; i < keyArray.Length; i++)
        {
            KeyData keyParseData = ParseKeySlotData(keyArray[i]);
            if (keyParseData.state == false)
                continue;

            KeyActionCallbackBundle callbackBundle = keyParseData.slotType switch
            {
                Define.QuickSlotType.Default => null,
                Define.QuickSlotType.Item => new(null ,(callBackt) => controller.Inventory.UseItem(Define.ItemType.Consume, keyParseData.idx) ,null ),
                Define.QuickSlotType.Skill => null,
                _ => null,
            };
            this[keyParseData.key]?.Attach(callbackBundle);
        }
    }

    public void AttachKey(Key keyCode, KeyActionCallbackBundle callbackBundle, Define.QuickSlotType quickSlotType, int idx, InputActionType inputType = InputActionType.Button)
    {
        SetKeyDataServer(keyCode, quickSlotType, idx);
        this[keyCode].Attach(callbackBundle);
    }



    public void AttachCallbackMethod(Key keyCode, KeyActionCallbackBundle callbackBundle)
    {
        this[keyCode].Attach(callbackBundle);
    }


    public void RemoveKey(Key keyCode)
    {
        keyDataServer[(int)keyCode] = 0;
        this[keyCode].Remove();
    }

    public void SwapKey(Key keyCode1, Key keyCode2)
    {
        KeyActionCallbackBundle key1CallbackBundle = this[keyCode1].GetCallBackMethod();
        KeyActionCallbackBundle key2CallbackBundle = this[keyCode2].GetCallBackMethod();

        this[keyCode1].Attach(key2CallbackBundle);
        this[keyCode2].Attach(key1CallbackBundle);
    }

    void SetKeyDataServer(Key keyCode = Key.LeftMeta, Define.QuickSlotType quickSlotType = Define.QuickSlotType.Default, int idx = -1)
    {
        int setServerData = default;
        setServerData |= ((int)keyCode).Shift(DataDefine.IntSize.One);
        setServerData |= ((int)quickSlotType).Shift(DataDefine.IntSize.One);
        setServerData |= idx.Shift(DataDefine.IntSize.Two);
        setServerData |= int.MaxValue.Shift(DataDefine.IntSize.Four);

        keyDataServer[(int)keyCode] = setServerData;
    }

    KeyData ParseKeySlotData(int value)
    {
        int setServerData = value;
        KeyData keyData = new()
        {
            key = (Key)setServerData.ExtractByte(DataDefine.IntSize.One),
            slotType = (Define.QuickSlotType)setServerData.ExtractByte(DataDefine.IntSize.Two),
            idx = setServerData.ExtractByte(DataDefine.IntSize.Three),
            state = setServerData.ExtractByte(DataDefine.IntSize.Four) > 0
        };
        return keyData;
    }



    [Serializable]
    public class KeyData
    {
        public Key key;
        public Define.QuickSlotType slotType;
        public int idx;
        public bool state;
    }

    public class KeyActionCallbackBundle
    {
        public Action<InputAction.CallbackContext> started;
        public Action<InputAction.CallbackContext> performed;
        public Action<InputAction.CallbackContext> canceled;
        public KeyActionCallbackBundle(
            Action<InputAction.CallbackContext> _started = null,
            Action<InputAction.CallbackContext> _performed = null,
            Action<InputAction.CallbackContext> _cancled = null)
        {
            started = _started;
            performed = _performed;
            canceled = _cancled;
        }
    }

    class KeyActionData
    {
        readonly InputAction input;
        KeyActionCallbackBundle callbackBundle;
        bool used;
        public KeyActionData(InputAction _input, Action<InputAction.CallbackContext> _performed = null)
        {
            input = _input;
            used = _performed == null;
            if (used == false)
            {
                input.Disable();
            }
            callbackBundle = new(null, _performed,null);
            input.started += (call) => callbackBundle.started?.Invoke(call);
            input.performed += (call) => callbackBundle.performed?.Invoke(call);
            input.canceled += (call) => callbackBundle.canceled?.Invoke(call);
        }

        public void Remove()
        {
            if (used)
            {
                input.Disable();
                used = false;
            }
            callbackBundle.started = null;
            callbackBundle.performed = null;
            callbackBundle.canceled = null;
        }
        public void Attach(KeyActionCallbackBundle _callbackBundle)
        {
            if (used == false)
            {
                input.Enable();
                used = true;
            }
            callbackBundle = _callbackBundle;
        }

        public KeyActionCallbackBundle GetCallBackMethod()
        {
            return new(callbackBundle.started, callbackBundle.performed, callbackBundle.performed);
        }
    }

}



