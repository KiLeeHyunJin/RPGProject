using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.InputSystem;
using static Define;

[Serializable]
public partial class KeyController
{
    const string KeyMapName = "DynamicKeySetting";
    const string KeyBindingFirstName = "<Keyboard>/";

    readonly UserCharacterController controller;
    readonly KeyActionData[] keyActionArray;
    readonly InputActionMap actionMap;
    KeyActionData this[Key keyCode] { get { return keyActionArray[(int)keyCode]; } set { keyActionArray[(int)keyCode] = value; } }
    [SerializeField]
    readonly List<int> keyDataServer;

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
            inputAction.Disable();
            keyActionDatas[(int)keyCode] = new(keyCode, inputAction);
        }
        keyActionArray[(int)Key.V].Attach(new(_performed : (call)=>Test2()));
        keyActionArray[(int)Key.G].Attach(new(_performed : (call)=> Test()));
    }
    void Test() => Debug.Log("fgsdgsasd");
    void Test2() => Debug.Log("324235352");
    public void TestCode()
    {
        SwapKey(Key.G, Key.V);
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
                Define.QuickSlotType.Item => new(null, (callBackt) => controller.Inventory.UseItem(Define.ItemType.Consume, keyParseData.idx), null),
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
        this[keyCode1].ChangeKeyName(keyCode2, out Action key1Action);
        this[keyCode2].ChangeKeyName(keyCode1, out Action key2Action);

        key1Action?.Invoke();
        key2Action?.Invoke();

        KeyActionData keyData = this[keyCode1];
        this[keyCode1] = this[keyCode2];
        this[keyCode2] = keyData;
    }

    public void ChangeActionType(Key keyCode, InputActionType _actionType)
    {
        this[keyCode].ChangeActionType(in actionMap, _actionType);
    }
    public void ChangeInteractionPress(Key keyCode, PressBehaviorType pressType = PressBehaviorType.None, float pressPointValue = -1)
    {
        this[keyCode].ChangeInteractionPress(pressType, pressPointValue);
    }
    public void ChangeInteractionOther(Key keyCode, InteractionType interactionType, float durationValue = -1, float pressPointValue = -1)
    {
        this[keyCode].ChangeInteractionOther(interactionType, durationValue, pressPointValue);
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
        Key keyCode;
        InputAction input;
        KeyActionCallbackBundle callbackBundle;
        bool used;
        public KeyActionData(Key _keyCode, InputAction _input, Action<InputAction.CallbackContext> _performed = null)
        {
            input = _input;
            keyCode = _keyCode;
            used = _performed == null;
            if (used == false)
            {
                input.Disable();
            }
            callbackBundle = new(null, _performed, null);
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

        public void ChangeKeyName(Key _keyCode, out Action renamed)
        {
            input.Disable();

            keyCode = _keyCode;
            input.Rename($"{(int)_keyCode}");
            input.ChangeBinding(0)
                .WithName(keyCode.ToString())
                .WithPath($"{KeyBindingFirstName}{keyCode}");

            renamed = ()=> 
            {
                input.Rename(keyCode.ToString());
                input.Enable();
            };
        }

        public void ChangeInteractionPress(PressBehaviorType behaviorType, float pressPointValue)
        {
            StringBuilder sb = null;
            bool changedState = false;

            if (behaviorType != PressBehaviorType.None)
            {
                changedState = true;
                sb = new($"(behavior={(int)behaviorType}");
            }

            if (pressPointValue >= 0)
            {
                if (changedState == false)
                {
                    changedState = true;
                    sb = new("(");
                }
                else
                {
                    sb.Append(",");
                }
                sb.Append($"pressPoint={pressPointValue}");
            }

            sb?.Append(")");

            if (changedState)
            {
                input.Disable();
                input.ChangeBinding(0).WithInteraction($"press{sb}");
                input.Enable();
            }
        }
        public void ChangeInteractionOther(InteractionType interactionType, float durationValue, float pressPointValue)
        {
            StringBuilder sb = null;
            bool changedState = default;
            if (durationValue >= 0)
            {
                changedState = true;
                sb = new($"(duration={durationValue}");
            }

            if (pressPointValue >= 0)
            {
                if (changedState == false)
                {
                    changedState = true;
                    sb = new("(");
                }
                else
                {
                    sb.Append(",");
                }
                sb.Append($"pressPoint={pressPointValue}");
            }

            sb?.Append(")");

            if (changedState)
            {
                input.Disable();
                input.ChangeBinding(0).WithInteraction($"{interactionType}{sb}");
                input.Enable();
            }
        }

        public void ChangeActionType(in InputActionMap actionMap, InputActionType _actionType)
        {
            if (input.type == _actionType)
                return;

            actionMap.Disable();
            string _interaction = input.interactions;
            input.RemoveAction();
            input = actionMap.AddAction(
                keyCode.ToString(),
                type: _actionType,
                binding: $"{KeyBindingFirstName}{keyCode}",
                interactions : _interaction);
            input.started += (call) => callbackBundle.started?.Invoke(call);
            input.performed += (call) => callbackBundle.performed?.Invoke(call);
            input.canceled += (call) => callbackBundle.canceled?.Invoke(call);

            actionMap.Enable();
        }
        public KeyActionCallbackBundle GetCallBackMethod()
        {
            return new(callbackBundle.started, callbackBundle.performed, callbackBundle.performed);
        }
    }

}



