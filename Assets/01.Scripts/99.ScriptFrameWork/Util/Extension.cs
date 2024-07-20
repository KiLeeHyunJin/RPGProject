using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine.UI;
namespace UnityEngine
{
    public static class Extension
    {

        public static void FontInit(this GameObject obj, Define.Font font)
        {
            //TMP_FontAsset fontAsset = font switch
            //{
            //    Define.Font.MBold => Manager.Resource.FontBold,
            //    Define.Font.MLight => Manager.Resource.FontLight,
            //    _ => null,
            //};
            //FontRecursion(obj.transform, fontAsset);
        }

        static void FontRecursion(Transform parent, TMP_FontAsset _font)
        {
            foreach (Transform child in parent)
            {
                if (child.gameObject.TryGetComponent<TextMeshProUGUI>(out TextMeshProUGUI text))
                    text.font = _font;
                FontRecursion(child, _font);
            }
        }

        public static List<string> GetAssetBundleNames()
        {
            string path = $"{Define.dir}/Windows";

            if (!Directory.Exists(path))
            {
                Debug.LogError("Directory does not exist: " + path);
                return new();
            }

            List<string> assetBundleNames = new();

            foreach (string filePath in Directory.GetFiles(path))
            {
                if (Path.GetExtension(filePath) == string.Empty) // Check for files without an extension
                {
                    string fileName = Path.GetFileName(filePath);
                    assetBundleNames.Add(fileName);
                    //Debug.Log(fileName);
                }
            }
            return assetBundleNames;
        }

        public static T ParseEnum<T>(string value, bool ignoreCase = true)
        {
            return (T)Enum.Parse(typeof(T), value, ignoreCase);
        }


        /// <summary>
        /// 실행중인 코루틴을 중지하고 다시 실행합니다.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="gameObject"></param>
        /// <param name="routine"></param>
        /// <param name="co"></param>
        public static void ReStartCoroutine<T>(this T gameObject, IEnumerator routine, ref Coroutine co) where T : MonoBehaviour
        {
            if (co != null)
                gameObject.StopCoroutine(co);
            co = gameObject.StartCoroutine(routine);
        }


        /// <summary>
        /// 해당 레이어의 비트가 올라와있는지 확인해서 반환합니다.
        /// </summary>
        /// <param name="layerMask"></param>
        /// <param name="layer"></param>
        /// <returns></returns>
        public static bool ContainCheck(this LayerMask layerMask, int layer)
        {
            return ((1 << layer) & layerMask) != 0;
        }
        /// <summary>
        /// 해당 레이어 플래그를 올려줍니다.
        /// </summary>
        /// <param name="layerMask"></param>
        /// <param name="layer"></param>
        public static void Contain(this ref LayerMask layerMask, int layer)
        {
            layerMask |= 1 << layer;
        }
        /// <summary>
        /// 게임오브젝트 자식중 name의 이름을 갖는 객체를 반환합니다.
        /// </summary>
        /// <param name="go"></param>
        /// <param name="name"></param>
        /// <param name="recursive"></param>
        /// <returns></returns>
        public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
        {
            Transform transform = FindChild<Transform>(go, name, recursive);
            if (transform != null)
                return transform.gameObject;
            return null;
        }
        /// <summary>
        /// 게임오브젝트의 자식 중 T타입 요소의 이름이 name인 객체를 찾아 반환합니다.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="go"></param>
        /// <param name="name"></param>
        /// <param name="recursive"></param>
        /// <returns></returns>
        public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
        {
            if (go == null)
                return null;

            if (recursive == false)
            {
                Transform transform = go.transform.Find(name);
                if (transform != null)
                    return transform.GetComponent<T>();
            }
            else
            {
                foreach (T component in go.GetComponentsInChildren<T>())
                {
                    if (string.IsNullOrEmpty(name) || component.name == name)
                        return component;
                }
            }
            return null;
        }

        /// <summary>
        /// 해당 컴포넌트가 존재한다면 찾아서 반환하고 없을경우 추가합니다.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="go"></param>
        /// <returns></returns>
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

        static System.Random _rand = new System.Random();

        /// <summary>
        /// 버블소트를 이용해 모든 요소를 섞습니다.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
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
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static T GetRandom<T>(this IList<T> list)
        {
            int index = _rand.Next(list.Count);
            return list[index];
        }

        public static void ResetVertical(this ScrollRect scrollRect)
        {
            scrollRect.verticalNormalizedPosition = 1;
        }

        public static void ResetHorizontal(this ScrollRect scrollRect)
        {
            scrollRect.horizontalNormalizedPosition = 1;
        }
    }
}