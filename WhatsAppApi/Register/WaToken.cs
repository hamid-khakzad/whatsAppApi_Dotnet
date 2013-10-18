﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace WhatsAppApi.Register
{
    class WaToken
    {
        private static string WaPrefix = "Y29tLndoYXRzYXBw";
        private static string WaSignature = "MIIDMjCCAvCgAwIBAgIETCU2pDALBgcqhkjOOAQDBQAwfDELMAkGA1UEBhMCVVMxEzARBgNVBAgTCkNhbGlmb3JuaWExFDASBgNVBAcTC1NhbnRhIENsYXJhMRYwFAYDVQQKEw1XaGF0c0FwcCBJbmMuMRQwEgYDVQQLEwtFbmdpbmVlcmluZzEUMBIGA1UEAxMLQnJpYW4gQWN0b24wHhcNMTAwNjI1MjMwNzE2WhcNNDQwMjE1MjMwNzE2WjB8MQswCQYDVQQGEwJVUzETMBEGA1UECBMKQ2FsaWZvcm5pYTEUMBIGA1UEBxMLU2FudGEgQ2xhcmExFjAUBgNVBAoTDVdoYXRzQXBwIEluYy4xFDASBgNVBAsTC0VuZ2luZWVyaW5nMRQwEgYDVQQDEwtCcmlhbiBBY3RvbjCCAbgwggEsBgcqhkjOOAQBMIIBHwKBgQD9f1OBHXUSKVLfSpwu7OTn9hG3UjzvRADDHj+AtlEmaUVdQCJR+1k9jVj6v8X1ujD2y5tVbNeBO4AdNG/yZmC3a5lQpaSfn+gEexAiwk+7qdf+t8Yb+DtX58aophUPBPuD9tPFHsMCNVQTWhaRMvZ1864rYdcq7/IiAxmd0UgBxwIVAJdgUI8VIwvMspK5gqLrhAvwWBz1AoGBAPfhoIXWmz3ey7yrXDa4V7l5lK+7+jrqgvlXTAs9B4JnUVlXjrrUWU/mcQcQgYC0SRZxI+hMKBYTt88JMozIpuE8FnqLVHyNKOCjrh4rs6Z1kW6jfwv6ITVi8ftiegEkO8yk8b6oUZCJqIPf4VrlnwaSi2ZegHtVJWQBTDv+z0kqA4GFAAKBgQDRGYtLgWh7zyRtQainJfCpiaUbzjJuhMgo4fVWZIvXHaSHBU1t5w//S0lDK2hiqkj8KpMWGywVov9eZxZy37V26dEqr/c2m5qZ0E+ynSu7sqUD7kGx/zeIcGT0H+KAVgkGNQCo5Uc0koLRWYHNtYoIvt5R3X6YZylbPftF/8ayWTALBgcqhkjOOAQDBQADLwAwLAIUAKYCp0d6z4QQdyN74JDfQ2WCyi8CFDUM4CaNB+ceVXdKtOrNTQcc0e+t";
        private static string WaClassesMd5 = "zo7YXXvrrRqpikOi/CveTw==";
        private static string WaKey = "PkTwKSZqUfAUyR0rPQ8hYJ0wNsQQ3dW1+3SCnyTXIfEAxxS75FwkDf47wNv/c8pP3p0GXKR6OOQmhyERwx74fw1RYSU10I4r1gyBVDbRJ40pidjM41G1I1oN";

        public static string GenerateToken(string number)
        {
            List<byte> key = new List<byte>(Convert.FromBase64String(WaToken.WaPrefix));
            key.AddRange(Convert.FromBase64String(WaToken.DataFile));

            Rfc2898DeriveBytes r = new Rfc2898DeriveBytes(key.ToArray(), Convert.FromBase64String(WaToken.WaKey), 128);
            key = new List<byte>(r.GetBytes(80));

            List<byte> data = new List<byte>(Convert.FromBase64String(WaToken.WaSignature));
            data.AddRange(Convert.FromBase64String(WaToken.WaClassesMd5));
            data.AddRange(System.Text.Encoding.ASCII.GetBytes(number));

            List<byte> opad = GetFilledList(0x5C, 64);
            List<byte> ipad = GetFilledList(0x36, 64);
            for (int i = 0; i < opad.Count; i++)
            {
                opad[i] = (byte)(opad[i] ^ key[i]);
                ipad[i] = (byte)(ipad[i] ^ key[i]);
            }

            SHA1 hasher = SHA1.Create();

            ipad.AddRange(data);
            data = new List<byte>(hasher.ComputeHash(ipad.ToArray()));
            opad.AddRange(data);
            data = new List<byte>(hasher.ComputeHash(opad.ToArray()));

            return Convert.ToBase64String(data.ToArray());
        }

        private static List<byte> GetFilledList(byte item, int length)
        {
            List<byte> result = new List<byte>();
            for (int i = 0; i < length; i++)
            {
                result.Add(item);
            }
            return result;
        }

        private static string DataFile = "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";
    }
}
