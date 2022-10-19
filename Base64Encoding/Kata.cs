using System;
using System.Collections.Generic;

public static class Base64Utils
{
    // Not that efficient to recreate the table each time, but I don't feel like typing all values
    private static char Base64Table(int index)
    {
        List<char> table = new List<char>();
        for (char i = 'A'; i <= 'Z'; i++)
        {
            table.Add(i);
        }
        for (char i = 'a'; i <= 'z'; i++)
        {
            table.Add(i);
        }
        for (char i = '0'; i <= '9'; i++)
        {
            table.Add(i);
        }
        table.Add('+');
        table.Add('/');

        return table[index];
    }

    private static int Base64Table(char index)
    {
        List<char> table = new List<char>();
        for (char i = 'A'; i <= 'Z'; i++)
        {
            table.Add(i);
        }
        for (char i = 'a'; i <= 'z'; i++)
        {
            table.Add(i);
        }
        for (char i = '0'; i <= '9'; i++)
        {
            table.Add(i);
        }
        table.Add('+');
        table.Add('/');

        return table.IndexOf(index);
    }


    public static string ToBase64(string s)
    {
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(s);
        string base64 = "";

        for (int i = 0; i < bytes.Length; i += 3)
        {
            var val = bytes[i] << 16
                    | (i + 1 < bytes.Length ? bytes[i + 1] : 0) << 8
                    | (i + 2 < bytes.Length ? bytes[i + 2] : 0);

            base64 += Base64Table((val >> 18) & 0b00111111);
            base64 += Base64Table((val >> 12) & 0b00111111);
            base64 += i + 1 < bytes.Length ? Base64Table((val >> 6) & 0b00111111) : "=";
            base64 += i + 2 < bytes.Length ? Base64Table(val & 0b00111111) : "=";
        }

        return base64;
    }

    public static string FromBase64(string s)
    {
        List<byte> bytes = new List<byte>();

        for (int i = 0; i < s.Length; i += 4)
        {
            int val = (byte)Base64Table(s[i]) << 18
                    | (byte)(i + 1 < s.Length ? Base64Table(s[i + 1]) : 0) << 12
                    | (byte)(i + 2 < s.Length ? Base64Table(s[i + 2]) : 0) << 6
                    | (byte)(i + 3 < s.Length ? Base64Table(s[i + 3]) : 0);

            bytes.Add((byte)((val >> 16) & 0b11111111));
            if (i + 2 < s.Length && s[i + 2] != '=') bytes.Add((byte)((val >> 8) & 0b11111111));
            if (i + 3 < s.Length && s[i + 3] != '=') bytes.Add((byte)(val & 0b11111111));
        }

        return (System.Text.Encoding.UTF8.GetString(bytes.ToArray()));
    }
}