using UnityEngine;

namespace AMSS
{
    public static class JsonParser<T>
    {
        /// <summary>
        /// Josn解析
        /// </summary>
        /// <param name="jsonFilePath">json文本路径</param>
        /// <returns>json数据解析出来存储在泛型类中，调用这个方法会返回泛型类的对象</returns>
        public static T Parse(string jsonFilePath)
        {
            TextAsset ta = Resources.Load<TextAsset>(jsonFilePath);
            if (ta == null)
            {
                Debug.LogWarning("请检查路径是否正确！");
                return default(T);
            }
            else
            {
                T jsonObject = JsonUtility.FromJson<T>(ta.text);
                return jsonObject;
            }
        }
    }
}

