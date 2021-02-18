﻿using System;

namespace ItemPriceCharts.UI.WPF.Helpers
{
    public static class ObjectExtension
    {
        public static bool IsAny<T>(this T obj, params T[] args)
        {
            return Array.IndexOf(args, obj) >= 0;
        }
    }
}
