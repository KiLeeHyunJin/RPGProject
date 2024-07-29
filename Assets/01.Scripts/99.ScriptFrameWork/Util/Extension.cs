using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace UnityEngine
{
    public static class Extension
    {
        /// <summary>
        /// 버튼의 Null상태를 확인한 후 액션을 제거 후 추가
        /// </summary>
        public static bool NullCheckAndAddListener(this UI.Button btn, Events.UnityAction action, bool allRemoveListner = false)
        {
            if (btn != null)
            {
                if (allRemoveListner)
                    btn.onClick.RemoveAllListeners();
                else
                    btn.onClick.RemoveListener(action);
                btn.onClick.AddListener(action);
                return true;
            }
            return false;
        }

        public static void StopCoroutine(this MonoBehaviour mono, ref Coroutine co)
        {
            if (co != null)
                mono.StopCoroutine(co);
        }
       
        /// <summary>
        /// 실행중인 코루틴을 중지하고 다시 실행합니다.
        /// </summary>
        public static void ReStartCoroutine(this MonoBehaviour mono, IEnumerator routine, ref Coroutine co)
        {
            if (co != null)
                mono.StopCoroutine(co);
            co = mono.StartCoroutine(routine);
        }


        /// <summary>
        /// 해당 레이어의 비트가 올라와있는지 확인해서 반환합니다.
        /// </summary>
        public static bool ContainCheck(this LayerMask layerMask, int layer)
        {
            return ((1 << layer) & layerMask) != 0;
        }
        /// <summary>
        /// 해당 레이어 플래그를 올려줍니다.
        /// </summary>
        public static void Contain(this ref LayerMask layerMask, int layer)
        {
            layerMask |= 1 << layer;
        }


        /// <summary>
        /// 해당 컴포넌트가 존재한다면 찾아서 반환하고 없을경우 추가합니다.
        /// </summary>
        public static T GetOrAddComponent<T>(this GameObject go) where T : UnityEngine.Component
        {
            T component = go.GetComponent<T>();
            if (component == null)
                component = go.AddComponent<T>();
            return component;
        }

        public static void BindEvent(this GameObject go, Action action, Define.UIEvent type = Define.UIEvent.Click)
        {
            BaseUI.BindEvent(go, action, type);
        }

        static System.Random _rand = new();

        /// <summary>
        /// 버블소트를 이용해 모든 요소를 섞습니다.
        /// </summary>
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = _rand.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
        /// <summary>
        /// 리스트의 크기 범위 내 랜덤한 인덱스를 반환합니다.
        /// </summary>
        public static T GetRandom<T>(this IList<T> list)
        {
            int index = _rand.Next(list.Count);
            return list[index];
        }
        /// <summary>
        /// 스크롤바 위치를 초기화합니다.
        /// </summary>
        public static void ResetVertical(this UI.ScrollRect scrollRect)
        {
            scrollRect.verticalNormalizedPosition = 1;
        }

        /// <summary>
        /// 스크롤바 위치를 초기화합니다.
        /// </summary>
        public static void ResetHorizontal(this UI.ScrollRect scrollRect)
        {
            scrollRect.horizontalNormalizedPosition = 1;
        }
    }
}