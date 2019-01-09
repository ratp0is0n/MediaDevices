using System;
using System.Runtime.InteropServices;

namespace MediaDevices.Internal
{
    [ComConversionLoss]
    [StructLayout(LayoutKind.Explicit, Size = 16)]
    internal struct PROPVARIANT_INNER
    {
        [ComConversionLoss]
        [FieldOffset(0)]
        public sbyte cVal;
        [FieldOffset(0)]
        public tagCAUI caui;
        [FieldOffset(0)]
        public tagCAL cal;
        [FieldOffset(0)]
        public tagCAUL caul;
        [FieldOffset(0)]
        public tagCAFLT caflt;
        [FieldOffset(0)]
        public tagCADBL cadbl;
        [FieldOffset(0)]
        public tagCABOOL cabool;
        [FieldOffset(0)]
        public tagCASCODE cascode;
        [ComConversionLoss]
        [FieldOffset(0)]
        public IntPtr pcVal;
        [ComConversionLoss]
        [FieldOffset(0)]
        public IntPtr pbVal;
        [ComConversionLoss]
        [FieldOffset(0)]
        public IntPtr piVal;
        [ComConversionLoss]
        [FieldOffset(0)]
        public IntPtr puiVal;
        [ComConversionLoss]
        [FieldOffset(0)]
        public IntPtr plVal;
        [ComConversionLoss]
        [FieldOffset(0)]
        public IntPtr pulVal;
        [ComConversionLoss]
        [FieldOffset(0)]
        public IntPtr pintVal;
        [ComConversionLoss]
        [FieldOffset(0)]
        public IntPtr puintVal;
        [ComConversionLoss]
        [FieldOffset(0)]
        public IntPtr pfltVal;
        [ComConversionLoss]
        [FieldOffset(0)]
        public IntPtr pdblVal;
        [FieldOffset(0)]
        public tagCAI cai;
        [ComConversionLoss]
        [FieldOffset(0)]
        public IntPtr pboolVal;
        [FieldOffset(0)]
        public tagCAUB caub;
        [FieldOffset(0)]
        public tagBLOB blob;
        [FieldOffset(0)]
        public byte bVal;
        [FieldOffset(0)]
        public short iVal;
        [FieldOffset(0)]
        public ushort uiVal;
        [FieldOffset(0)]
        public int lVal;
        [FieldOffset(0)]
        public uint ulVal;
        [FieldOffset(0)]
        public int intVal;
        [FieldOffset(0)]
        public uint uintVal;
        [FieldOffset(0)]
        public _LARGE_INTEGER hVal;
        [FieldOffset(0)]
        public _ULARGE_INTEGER uhVal;
        [FieldOffset(0)]
        public float fltVal;
        [FieldOffset(0)]
        public double dblVal;
        [FieldOffset(0)]
        public short boolVal;
        [FieldOffset(0)]
        public short @bool;
        [FieldOffset(0)]
        public int scode;
        [FieldOffset(0)]
        public double date;
        [FieldOffset(0)]
        public _FILETIME filetime;
        [FieldOffset(0)]
        public tagBSTRBLOB bstrblobVal;
        [FieldOffset(0)]
        public tagCAC cac;
        [ComConversionLoss]
        [FieldOffset(0)]
        public IntPtr pscode;
    }

    [ComConversionLoss]
    internal struct tagCAUI
    {
        public uint cElems;
        [ComConversionLoss]
        public IntPtr pElems;
    }


    [ComConversionLoss]
    internal struct tagCAL
    {
        public uint cElems;
        [ComConversionLoss]
        public IntPtr pElems;
    }


    [ComConversionLoss]
    internal struct tagCAUL
    {
        public uint cElems;
        [ComConversionLoss]
        public IntPtr pElems;
    }


    [ComConversionLoss]
    internal struct tagCAFLT
    {
        public uint cElems;
        [ComConversionLoss]
        public IntPtr pElems;
    }


    [ComConversionLoss]
    internal struct tagCADBL
    {
        public uint cElems;
        [ComConversionLoss]
        public IntPtr pElems;
    }


    [ComConversionLoss]
    internal struct tagCABOOL
    {
        public uint cElems;
        [ComConversionLoss]
        public IntPtr pElems;
    }


    [ComConversionLoss]
    internal struct tagCASCODE
    {
        public uint cElems;
        [ComConversionLoss]
        public IntPtr pElems;
    }


    [ComConversionLoss]
    internal struct tagCAI
    {
        public uint cElems;
        [ComConversionLoss]
        public IntPtr pElems;
    }

    [ComConversionLoss]
    internal struct tagCAUB
    {
        public uint cElems;
        [ComConversionLoss]
        public IntPtr pElems;
    }


    [ComConversionLoss]
    internal struct tagBLOB
    {
        public uint cbSize;
        [ComConversionLoss]
        public IntPtr pBlobData;
    }

    internal struct _LARGE_INTEGER
    {
        public long QuadPart;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct _ULARGE_INTEGER
    {
        public ulong QuadPart;
    }

    internal struct _FILETIME
    {
        public uint dwLowDateTime;
        public uint dwHighDateTime;
    }


    [ComConversionLoss]
    internal struct tagBSTRBLOB
    {
        public uint cbSize;
        [ComConversionLoss]
        public IntPtr pData;
    }


    [ComConversionLoss]
    internal struct tagCAC
    {
        public uint cElems;
        [ComConversionLoss]
        public IntPtr pElems;
    }
}
