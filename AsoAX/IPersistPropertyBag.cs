using System;
using System.Runtime.InteropServices;

namespace AsoAX
{
    [ComImport]
    [Guid("37D84F60-42CB-11CE-8135-00AA004BB851")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IPersistPropertyBag
    {

        [PreserveSig]
        void InitNew();

        void Load
        (
            [In, MarshalAs(UnmanagedType.IUnknown)] object pPropBag, // IPropertyBag*
            [In, MarshalAs(UnmanagedType.IUnknown)] object pErrorLog // IErrorLog*
        );

        int Save
        (
            /* [in] */ ref object pPropBag,
            /* [in] */ bool fClearDirty,
            /* [in] */ bool fSaveAllProperties
        );

        //[PreserveSig]
        //void Load(IPropertyBag propertyBag, int errorLog);

        //[PreserveSig]
        //void Save(IPropertyBag propertyBag, [In] bool clearDirty, [In] bool saveAllProperties);

        [PreserveSig]
        void GetClassID(out Guid classID);
    }
}
