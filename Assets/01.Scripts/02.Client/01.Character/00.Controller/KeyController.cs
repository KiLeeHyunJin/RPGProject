using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.InputSystem;
using static Define;

/// <summary>
/// 입력 키 컨트롤러 
/// </summary>
[Serializable]
public class KeyController
{
    const string KeyMapName = "DynamicKeySetting";
    const string KeyBindingFirstName = "<Keyboard>/";

    readonly UserCharacterController characterController;
    readonly KeyActionData[] keyActionArray;
    readonly InputActionMap actionMap;
    KeyActionData this[Key keyCode] 
    { 
        get { return keyActionArray[(int)keyCode]; } 
        set { keyActionArray[(int)keyCode] = value; } 
    }
    [SerializeField]
    readonly List<int> keyDataServer;



    void Test() => Debug.Log("fgsdgsasd");
    void Test2() => Debug.Log("324235352");


    int temp = 0;
    public void TestCode()
    {
        if ((temp % 2) == 0)
        {
            this[Key.G].ChangeInteractionPress(PressBehaviorType.PressAndRelease, 0.2f);
        }
        else
        {
            this[Key.G].ChangeInteractionOther(InteractionType.hold, 0.2f, 0.8f);
        }
        temp++;
    }




    //키 저장을 위한 keyDataServer 구조체 또는 직렬화 클래스
    public KeyController(UserCharacterController owner, PlayerInput input)
    {
        characterController = owner;
        Key[] keysArray = Utils.GetEnumArray<Key>();
        keyDataServer = new(keysArray.Length);

        if(input.actions == null)
            input.actions = Manager.Resource.Load<InputActionAsset>("Manager/InputAction");

        InputActionAsset inputActions = input.actions;

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
            keyActionDatas[(int)keyCode] = new(characterController ,keyCode, inputAction);
        }
        InitMove(actionMap);


        this[Key.V].Attach(new(_performed : (call)=>Test2()));
        this[Key.G].Attach(new(_performed : (call)=> Test()));
    }


    void InitMove(InputActionMap actionMap)
    {
        InputAction moveAction = actionMap.AddAction("Move", type: InputActionType.Button, interactions: "hold(duration=4)");
        moveAction.AddCompositeBinding("2DVector")
                .With("Up",     $"<Keyboard>/{Key.UpArrow}")
                .With("Left",   $"<Keyboard>/{Key.LeftArrow}")
                .With("Down",   $"<Keyboard>/{Key.DownArrow}")
                .With("Right",  $"<Keyboard>/{Key.RightArrow}");
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
                Define.QuickSlotType.Item => new(null, (callBackt) => characterController.InventoryController.UseItem(Define.ItemType.Consume, keyParseData.idx), null),
                Define.QuickSlotType.Skill => null,
                _ => null,
            };

            switch (keyParseData.slotType)
            {
                case QuickSlotType.Default:
                    break;
                case QuickSlotType.Item:
                    callbackBundle = new(null, (callBackt) => characterController.InventoryController.UseItem(Define.ItemType.Consume, keyParseData.idx), null);

                    break;
                case QuickSlotType.Skill:
                    break;
            }

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

    /// <summary>
    /// 해당 키의 액션 타입을 변경
    /// </summary>
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
    public void ChangeInteractionRemove(Key keyCode)
    {
        this[keyCode].ChangeInteractionRemove();
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
        Action<InputAction.CallbackContext> started;
        Action<InputAction.CallbackContext> performed;
        Action<InputAction.CallbackContext> canceled;
        public Action<InputAction.CallbackContext> Started { get { return started; } }
        public Action<InputAction.CallbackContext> Performed { get { return performed; } }
        public Action<InputAction.CallbackContext> Canceled { get { return canceled; } }

        public KeyActionCallbackBundle(
            Action<InputAction.CallbackContext> _started = null,
            Action<InputAction.CallbackContext> _performed = null,
            Action<InputAction.CallbackContext> _cancled = null)
        {
            started = _started;
            performed = _performed;
            canceled = _cancled;
        }
        public void Remove()
        {
            started = null;
            performed = null;
            canceled = null;
        }
    }

    class KeyActionData
    {
        Key keyCode;
        bool used;
        InputAction input;
        readonly UserCharacterController characterController;
        KeyActionCallbackBundle callbackBundle;
        KeyActionCallbackBundle CallbackBundle 
        { 
            set 
            { 
                if (value != null) 
                    callbackBundle = value;
            }
        }

        public KeyActionData(UserCharacterController _characterController ,Key _keyCode, InputAction _input, Action<InputAction.CallbackContext> _performed = null)
        {
            input = _input;
            keyCode = _keyCode;
            used = _performed == null;
            characterController = _characterController;
            if (used == false)
            {
                input.Disable();
            }
            callbackBundle = new(null, _performed, null);
            input.started += (call) => callbackBundle.Started?.Invoke(call);
            input.performed += (call) => callbackBundle.Performed?.Invoke(call);
            input.canceled += (call) => callbackBundle.Canceled?.Invoke(call);
        }

        public void Remove()
        {
            if (used)
            {
                input.Disable();
                used = false;
            }
            callbackBundle.Remove();
        }

        public void Attach(KeyActionCallbackBundle _callbackBundle)
        {
            if (used == false)
            {
                input.Enable();
                used = true;
            }
            CallbackBundle = _callbackBundle;
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

        public void ChangeInteractionRemove()
        {
            int checkCount = default;
            for (int i = 0; i < input.bindings.Count; i++)
            {
                if (string.IsNullOrEmpty(input.bindings[i].interactions))
                    continue;
                else
                    checkCount++;
            }
            if (int.Equals(checkCount, default))
                return;

            input.Disable();
            RemoveInteraction();
            input.Enable();
        }

        void RemoveInteraction()
        {
            for (int i = 0; i < input.bindings.Count; i++)
            {
                if (string.IsNullOrEmpty(input.bindings[i].interactions))
                    continue;

                var binding = input.bindings[i];

                InputBinding newBinding = new()
                {
                    path = binding.path,
                    processors = binding.processors,
                    groups = binding.groups,
                    action = binding.action,
                    isComposite = binding.isComposite,
                    isPartOfComposite = binding.isPartOfComposite,
                    interactions = null,
                    name = binding.name
                };

                input.ChangeBinding(i).Erase();
                input.AddBinding(newBinding);
            }
        }

        public void ChangeInteractionPress(PressBehaviorType behaviorType, float pressPointValue)
        {
            SetInteraction("press", "behavior", (int)behaviorType, pressPointValue);
        }

        public void ChangeInteractionOther(InteractionType interactionType, float durationValue, float pressPointValue)
        {
            SetInteraction($"{interactionType}", "duration", durationValue, pressPointValue);
        }

        void SetInteraction(string type, string value1Name, float value1, float value2)
        {
            var sb = new StringBuilder($"{type}");

            bool hasParameter = false;

            if (value1 >= 0)
            {
                sb.Append($"({value1Name}={value1}");
                hasParameter = true;
            }

            if (value2 >= 0)
            {
                if (hasParameter)
                {
                    sb.Append(",");
                }
                else
                {
                    sb.Append("(");
                    hasParameter = true;
                }
                sb.Append($"pressPoint={value2}");
            }

            if (hasParameter)
                sb.Append(")");
            else
                return;

            input.Disable();
            if (string.IsNullOrEmpty(input.bindings[0].interactions) == false)
                RemoveInteraction();
            input.ChangeBinding(0).WithInteraction(sb.ToString());
            input.Enable();
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
            input.started += (call) => callbackBundle.Started?.Invoke(call);
            input.performed += (call) => callbackBundle.Performed?.Invoke(call);
            input.canceled += (call) => callbackBundle.Canceled?.Invoke(call);

            actionMap.Enable();
        }
        public KeyActionCallbackBundle GetCallBackMethod()
        {
            return new(callbackBundle.Started, callbackBundle.Performed, callbackBundle.Performed);
        }

        void CallKey(InputAction.CallbackContext context)
        {
            characterController.ComboController.AddCommandToComboSequence(keyCode);
        }
    }

}



