using System;
using System.Runtime.InteropServices;

namespace AsoAX
{
    [ComVisible(true)]
    [ComImport]
    [Guid("55272A00-42CB-11CE-8135-00AA004BB851")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IPropertyBag
    {
        void Read
        (
            [In, MarshalAs(UnmanagedType.BStr)] String bstrPropName,
            [In, Out] ref object pVar, // VARIANT __RPC_FAR *pVar
            [In] object pErrorLog // IErrorLog* pErrorLog
        );

        void Write
        (
            /* [in] */ string pszPropName,
            /* [in] */ ref object pVar // VARIANT* pVar
        );

        //[PreserveSig]
        //int Read(
        //  [In, MarshalAs(UnmanagedType.LPWStr)] string pszPropName,
        //  [Out, MarshalAs(UnmanagedType.Struct)] out object pVar,
        //  [In] IntPtr pErrorLog
        //);

        //[PreserveSig]
        //int Write(
        //  [In, MarshalAs(UnmanagedType.LPWStr)] string pszPropName,
        //  [In, MarshalAs(UnmanagedType.Struct)] ref object pVar
        //);
    }
}
