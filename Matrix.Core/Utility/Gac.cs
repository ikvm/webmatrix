namespace Microsoft.Matrix.Utility
{
    using Microsoft.Matrix;
    using System;
    using System.Collections;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using System.Runtime.InteropServices;

    public sealed class Gac
    {
        private Gac()
        {
        }

        public static bool AddAssembly(string assemblyFileName)
        {
            if ((assemblyFileName == null) || (assemblyFileName.Length == 0))
            {
                throw new ArgumentNullException("assemblyFileName");
            }
            if (!File.Exists(assemblyFileName))
            {
                throw new ArgumentException("assemblyFileName");
            }
            Interop.IAssemblyCache ppAsmCache = null;
            if (Interop.CreateAssemblyCache(out ppAsmCache, 0) != 0)
            {
                return false;
            }
            return (ppAsmCache.InstallAssembly(0, assemblyFileName, IntPtr.Zero) == 0);
        }

        public static bool ContainsAssembly(AssemblyName assemblyName)
        {
            if (assemblyName == null)
            {
                throw new ArgumentNullException("assemblyName");
            }
            return ContainsAssembly(assemblyName.FullName);
        }

        public static bool ContainsAssembly(string assemblyFullName)
        {
            if ((assemblyFullName == null) || (assemblyFullName.Length == 0))
            {
                throw new ArgumentNullException("assemblyFullName");
            }
            Interop.IAssemblyCache ppAsmCache = null;
            if (Interop.CreateAssemblyCache(out ppAsmCache, 0) != 0)
            {
                return false;
            }
            return (ppAsmCache.QueryAssemblyInfo(0, assemblyFullName, IntPtr.Zero) == 0);
        }

        public static ICollection GetAssemblies()
        {
            ArrayList list = new ArrayList();
            Interop.IAssemblyEnum ppEnum = null;
            Interop.IAssemblyName ppName = null;
            int num = Interop.CreateAssemblyEnum(out ppEnum, IntPtr.Zero, null, 2, 0);
            while (num == 0)
            {
                num = ppEnum.GetNextAssembly(IntPtr.Zero, out ppName, 0);
                if (num == 0)
                {
                    uint pccDisplayName = 0;
                    ppName.GetDisplayName(IntPtr.Zero, ref pccDisplayName, 0);
                    if (pccDisplayName > 0)
                    {
                        IntPtr zero = IntPtr.Zero;
                        string str = null;
                        try
                        {
                            zero = Marshal.AllocHGlobal((int) ((pccDisplayName + 1) * 2));
                            ppName.GetDisplayName(zero, ref pccDisplayName, 0);
                            str = Marshal.PtrToStringUni(zero);
                        }
                        finally
                        {
                            if (zero != IntPtr.Zero)
                            {
                                Marshal.FreeHGlobal(zero);
                            }
                        }
                        if (str != null)
                        {
                            AssemblyName name2 = new AssemblyName();
                            string[] strArray = str.Split(new char[] { ',' });
                            if (strArray.Length >= 4)
                            {
                                try
                                {
                                    name2.Name = strArray[0];
                                    name2.Version = new Version(strArray[1].Substring(strArray[1].IndexOf('=') + 1));
                                    string str2 = strArray[3].Substring(strArray[3].IndexOf('=') + 1);
                                    if (!str2.Equals("null"))
                                    {
                                        byte[] publicKeyToken = new byte[str2.Length / 2];
                                        for (int i = 0; i < publicKeyToken.Length; i++)
                                        {
                                            publicKeyToken[i] = byte.Parse(str2.Substring(i * 2, 2), NumberStyles.HexNumber);
                                        }
                                        name2.SetPublicKeyToken(publicKeyToken);
                                    }
                                    name2.CodeBase = GetFusionString(ppName, 13);
                                    string fusionString = GetFusionString(ppName, 8);
                                    name2.CultureInfo = new CultureInfo(fusionString);
                                    list.Add(name2);
                                    continue;
                                }
                                catch (Exception)
                                {
                                    continue;
                                }
                            }
                        }
                    }
                }
            }
            return list;
        }

        private static string GetFusionString(Interop.IAssemblyName assemblyName, uint item)
        {
            string str = string.Empty;
            uint pcbProperty = 0;
            if ((assemblyName.GetProperty(item, IntPtr.Zero, ref pcbProperty) == 0) && (pcbProperty > 0))
            {
                IntPtr zero = IntPtr.Zero;
                try
                {
                    zero = Marshal.AllocHGlobal((int) ((pcbProperty + 1) * 2));
                    if (assemblyName.GetProperty(item, zero, ref pcbProperty) == 0)
                    {
                        str = Marshal.PtrToStringUni(zero);
                    }
                }
                finally
                {
                    if (zero != IntPtr.Zero)
                    {
                        Marshal.FreeHGlobal(zero);
                    }
                }
            }
            return str;
        }

        public static bool RemoveAssembly(AssemblyName assemblyName)
        {
            if (assemblyName == null)
            {
                throw new ArgumentNullException("assemblyName");
            }
            return RemoveAssembly(assemblyName.FullName);
        }

        public static bool RemoveAssembly(string assemblyFullName)
        {
            uint num2;
            if ((assemblyFullName == null) || (assemblyFullName.Length == 0))
            {
                throw new ArgumentNullException("assemblyFullName");
            }
            Interop.IAssemblyCache ppAsmCache = null;
            if (Interop.CreateAssemblyCache(out ppAsmCache, 0) != 0)
            {
                return false;
            }
            if (ppAsmCache.UninstallAssembly(0, assemblyFullName, IntPtr.Zero, out num2) != 0)
            {
                return !ContainsAssembly(assemblyFullName);
            }
            return true;
        }
    }
}

