
using System;
using System.Security.Cryptography;
using System.Text;

public class StringUtil
{

    public static int HashString(string str)
    {
        MD5 md5Hasher = MD5.Create();
        var hashed = md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(str));
        var ivalue = BitConverter.ToInt32(hashed, 0);
        return ivalue;
    }
}