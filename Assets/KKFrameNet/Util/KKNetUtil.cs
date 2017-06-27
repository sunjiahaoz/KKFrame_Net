using UnityEngine;
using System.Collections;
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
}
