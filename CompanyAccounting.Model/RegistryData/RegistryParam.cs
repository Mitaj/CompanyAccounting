using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CompanyAccounting.Model.RegistryData
{
    public enum RegistryParam
    {
        ConnectionString = 0,
    }

    public static class RegistryParamExtensions
    {
        public const string ConnectionStringRegKey = @"SOFTWARE\Mitaj\CompanyAccounting\Settings\";
        public const string ConnectionStringValueName = "ConnectionString";
        public static bool HasFullRegisterPath(this RegistryParam param, out string keyName, out string value)
        {
            switch (param)
            {
                case RegistryParam.ConnectionString:
                    keyName = ConnectionStringRegKey;
                    value = ConnectionStringValueName;
                    return true;
                default:
                    keyName = string.Empty;
                    value = string.Empty;
                    return false;
            }
        }
    }
}
