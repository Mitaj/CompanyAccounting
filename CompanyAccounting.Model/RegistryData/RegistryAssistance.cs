using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace CompanyAccounting.Model.RegistryData
{
    public static class RegistryAssistance
    {
        private readonly static RegistryView RegistryView = Environment.Is64BitOperatingSystem ? RegistryView.Registry64 : RegistryView.Registry32;
        private readonly static RegistryHive RegistryHive = RegistryHive.CurrentUser;
        public static string GetRegistryParamValue(RegistryParam param)
        {
            if (!param.HasFullRegisterPath(out var keyName, out var valueName))
                return string.Empty;

            RegistryKey registryKey = null;
            try
            {
                registryKey = RegistryKey.OpenBaseKey(RegistryHive, RegistryView);
                object registryValue = registryKey?.OpenSubKey(keyName)?.GetValue(valueName, string.Empty)?? string.Empty;
                return registryValue.ToString();
            }
            catch (ArgumentException) { }
            catch (UnauthorizedAccessException) { }
            catch (SecurityException) { }
            catch (ObjectDisposedException) { }
            catch (IOException) { }
            finally
            {
                registryKey?.Close();
            }
            return string.Empty;
        }

        public static void SetRegistryParamValue(RegistryParam param, string value) 
        {
            if (!param.HasFullRegisterPath(out var keyName, out var valueName))
                return;

            RegistryKey registryKey = null;
            try
            {
                registryKey = RegistryKey.OpenBaseKey(RegistryHive, RegistryView);
                registryKey?.CreateSubKey(keyName)?.SetValue(valueName, value);
            }
            catch (ArgumentException) { }
            catch (UnauthorizedAccessException) { }
            catch (SecurityException) { }
            catch (ObjectDisposedException) { }
            catch (IOException) { }
            finally
            {
                registryKey?.Close();
            }
        }

    }
}
