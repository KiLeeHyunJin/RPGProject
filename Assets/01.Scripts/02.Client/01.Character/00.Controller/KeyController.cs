using System;
using System.Collections.Generic;
using System.Text;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using static KeyController.InteractionKeyEnumGroup;
using static System.Runtime.CompilerServices.RuntimeHelpers;
using static UnityEditor.Progress;

[Serializable]
public partial class KeyController
{
    const string KeyMapName = "DynamicKeySetting";
    const string KeyBindingFirstName = "<Keyboard>/";

    readonly UserCharacterController controller;
    readonly KeyActionData[] keyActionArray;
    readonly InputActionMap actionMap;
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
        actionMap = inputActions.FindActionMap(KeyMapName);

        if (actionMap != null)
            inputActions.RemoveActionMap(actionMap);

        actionMap = inputActions.AddActionMap(KeyMapName);
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
            Key keyCode = keysArray[i];
            inputAction = actionMap.FindAction(keyCode.ToString());
            inputAction?.RemoveAction();
            inputAction = actionMap.AddAction(keyCode.ToString(), binding: $"{KeyBindingFirstName}{keyCode}");
            //inputAction.ChangeBinding(0).WithInteraction($"{InteractionType.press}({PressType.behavior}={(int)PressBehaviorType.PressOnly},{PressType.pressPoint}={1})");
            //inputAction.Disable();
            keyActionDatas[(int)keyCode] = new(keyCode, inputAction, (call) =>Test());
        }
        //keyActionDatas[(int)Key.V].Attach(new(_performed : (call) => { Test2();}));
    }

    void Test() => Debug.Log("fgsdgsasd");
    void Test2() => Debug.Log("324235352");
    public void SwapTest()
    {
        ChangeKeyType(Key.G, InputActionType.Value, InteractionType.press, $"{PressType.behavior}={(int)PressBehaviorType.PressOnly}", $"{PressType.pressPoint}={1}");
    }

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

    public void AttachKey(Key keyCode, KeyActionCallbackBundle callbackBundle, Define.QuickSlotType quickSlotType, int idx)
    {
        SetKeyDataServer(keyCode, quickSlotType, idx);
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

    public void ChangeKeyType(Key keyCode, InputActionType _actionType, InteractionType interactionType,params string[] parameters)
    {
        this[keyCode].ChangeActionType(in actionMap, _actionType, interactionType, parameters);
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
        readonly Key keyCode;
        InputAction input;
        KeyActionCallbackBundle callbackBundle;
        bool used;
        public KeyActionData(Key _keyCode,InputAction _input, Action<InputAction.CallbackContext> _performed = null)
        {
            input = _input;
            keyCode = _keyCode;
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

        public void ChangeActionType(in InputActionMap actionMap, InputActionType _actionType, InteractionType interactionType, string[] parameters)
        {
            bool state = input.type != _actionType;

            if(string.IsNullOrEmpty(input.bindings[0].interactions) == false)
            {
                string checkInteraction = input.bindings[0].interactions.Split("(")[0];
                if (state == false &&
                    string.Equals(checkInteraction, interactionType.ToString()))
                {
                    return;
                }
            }

            Debug.Log("Change Oper");
            StringBuilder sb = new();
            if (parameters != null)
            {
                sb.Append("(");
                for (int i = 0; i < parameters.Length - 1; i++)
                    sb.Append($"{parameters[i]},");
                sb.Append($"{parameters[^1]})");
            }

            actionMap.Disable();
            if (state)
            {
                input.RemoveAction();
                input = actionMap.AddAction(
                    keyCode.ToString(), 
                    type: _actionType, 
                    binding: $"{KeyBindingFirstName}{keyCode}");
                input.started += (call) => callbackBundle.started?.Invoke(call);
                input.performed += (call) => callbackBundle.performed?.Invoke(call);
                input.canceled += (call) => callbackBundle.canceled?.Invoke(call);
            }
            input.ChangeBinding(0).WithInteraction($"{interactionType}{sb}");
            actionMap.Enable();
        }
        public KeyActionCallbackBundle GetCallBackMethod()
        {
            return new(callbackBundle.started, callbackBundle.performed, callbackBundle.performed);
        }
    }


    public class InteractionKeyEnumGroup
    { 
        public enum InteractionType
        {
            press,
            hold,
            tab
        }

        public enum PressType
        {
            behavior,
            pressPoint,
        }
        public enum PressBehaviorType
        {
            PressOnly = 0,
            ReleaseOnly = 1,
            PressAndRelease = 2,
        }

        public enum Otherype
        { 
            duration,
            pressPoint,
        }

    }

}



