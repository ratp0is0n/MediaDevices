using System;
using System.Runtime.InteropServices;

namespace MediaDevices.Internal
{
    [System.Runtime.InteropServices.Guid("0000000C-0000-0000-C000-000000000046")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IStream : ISequentialStream
    {
        //void RemoteRead(out byte pv, uint cb, out uint pcbRead);
        //void RemoteWrite(ref byte pv, uint cb, out uint pcbWritten);

        void RemoteSeek(
            [In] _LARGE_INTEGER dlibMove, 
            [In] uint dwOrigin, 
            [Out] out _ULARGE_INTEGER plibNewPosition);

        void SetSize(
            [In] _ULARGE_INTEGER libNewSize);

        void RemoteCopyTo(
            [In, MarshalAs(UnmanagedType.Interface)] IStream pstm, 
            [In] _ULARGE_INTEGER cb, 
            [Out] out _ULARGE_INTEGER pcbRead, 
            [Out] out _ULARGE_INTEGER pcbWritten);

        void Commit(
            [In] uint grfCommitFlags);

        void Revert();

        void LockRegion(
            [In] _ULARGE_INTEGER libOffset,
            [In] _ULARGE_INTEGER cb,
            [In] uint dwLockType);

        void UnlockRegion(
            [In] _ULARGE_INTEGER libOffset,
            [In] _ULARGE_INTEGER cb,
            [In] uint dwLockType);

        void Stat(
            [Out] out tagSTATSTG pstatstg,
            [In] uint grfStatFlag);

        void Clone(
            [Out, MarshalAs(UnmanagedType.Interface)] out IStream ppstm);
    }

#pragma warning disable CS0649

    [StructLayout(LayoutKind.Sequential)]
    internal struct tagSTATSTG
    {
        [MarshalAs(UnmanagedType.LPWStr)]
        public string pwcsName;
        public uint type;
        public _ULARGE_INTEGER cbSize;
        public _FILETIME mtime;
        public _FILETIME ctime;
        public _FILETIME atime;
        public uint grfMode;
        public uint grfLocksSupported;
        public Guid clsid;
        public uint grfStateBits;
        public uint reserved;
    }
}
