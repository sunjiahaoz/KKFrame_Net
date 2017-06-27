using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace KK.Frame.Net
{
    public class KKNetUtil
    {
        public static string Get4scalPasswordCN(string str, int len)
        {
            string s = str;
            int i = System.Text.Encoding.UTF8.GetBytes(s).Length;   // 与Server通讯使用的是UTF8编码
            if (i >= len)
                return s;

            byte[] a = new byte[len - i];
            s += System.Text.Encoding.ASCII.GetString(a);
            return s;
        }
    }

    /// <summary>
    /// 工具，内部使用的
    /// </summary>
    class ToolsNet
    {
        /// <summary>
        /// 合并两个此点
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dictDest">合并后的目标词典</param>
        /// <param name="dictSrc">要合并的此点</param>
        /// <param name="bOverrideKeySame">如果目标词典中已经存在KEY，是否覆盖掉，否则就过掉</param>
        public static void CombineDict<TKey, TValue>(Dictionary<TKey, TValue> dictDest, Dictionary<TKey, TValue> dictSrc, bool bOverrideKeySame = true)
        {
            foreach (var item in dictSrc)
            {
                if (dictDest.ContainsKey(item.Key))
                {
                    if (bOverrideKeySame)
                    {
                        dictDest[item.Key] = item.Value;
                    }
                    else
                    {
                        continue;
                    }
                }
                else
                {
                    dictDest.Add(item.Key, item.Value);
                }
            }
        }
    }
}
