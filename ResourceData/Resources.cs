﻿using System;
using Courby.Data;

namespace Courby.Resource.Data
{
    public static class Resources
    {
        public static string GetResourceValue(string culture, string ResourceKey)
        {
            return "";
        }
        public static void GetResourceValues(string culture, params string[] ResourceKeys)
        {
            Connection.ExecuteProcedure("resources_GetResourceValues",
                new Connection.ParamData() { name = "", value = "" },
                new Connection.ParamData() { name= "", value=""}
                );
        }
    }
}