using System;
using System.Runtime.InteropServices;
using STATSTG = System.Runtime.InteropServices.ComTypes.STATSTG;

namespace MediaDevices.Internal
{
    [System.Runtime.InteropServices.Guid("0000000C-0000-0000-C000-000000000046")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IStream : ISequentialStream
    {
         void RemoteSeek(
            [In] long dlibMove, 
            [In] uint dwOrigin, 
            [Out] out ulong plibNewPosition);

        void SetSize(
            [In] ulong libNewSize);

        void RemoteCopyTo(
            [In, MarshalAs(UnmanagedType.Interface)] IStream pstm, 
            [In] ulong cb, 
            [Out] out ulong pcbRead, 
            [Out] out ulong pcbWritten);

        void Commit(
            [In] uint grfCommitFlags);

        void Revert();

        void LockRegion(
            [In] ulong libOffset,
            [In] ulong cb,
            [In] uint dwLockType);

        void UnlockRegion(
            [In] ulong libOffset,
            [In] ulong cb,
            [In] uint dwLockType);

        void Stat(
            [Out] out STATSTG pstatstg,
            [In] uint grfStatFlag);

        void Clone(
            [Out, MarshalAs(UnmanagedType.Interface)] out IStream ppstm);
    }
}
