﻿using System.Text;
using Microsoft.AspNetCore.WebUtilities;

namespace ETicaretAPI.Application.Helpers;

public static class CustomEncoders
{
    public static string UrlEncode(this string value)
    {
        var bytes = Encoding.UTF8.GetBytes(value);
        return WebEncoders.Base64UrlEncode(bytes);
    }

    public static string UrlDecode(this string value)
    {
        var bytes = WebEncoders.Base64UrlDecode(value);
        return Encoding.UTF8.GetString(bytes);
    }
}