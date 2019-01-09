using System;
using System.Diagnostics;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Security;

namespace MediaDevices.Internal
{
    // https://docs.microsoft.com/de-de/windows/desktop/api/propidl/ns-propidl-tagpropvariant

    internal sealed class PropVariant : IDisposable
    {
        // cannot be a property because it will be filled by reference
        public PROPVARIANT Value;

        public PropVariant()
        {
            this.Value = new PROPVARIANT();
        }

        public void Dispose()
        {
            // clear only if filled
            if (this.Value.vt != 0)
            {
                try
                {
                    NativeMethods.PropVariantClear(ref this.Value);
                }
                catch (Exception ex)
                {
                    Trace.TraceError(ex.ToString());
                }
            }
        }

        public VarType VariantType
        {
            get { return (VarType)this.Value.vt; }
        }

        public override string ToString()
        {
            switch ((VarType)this.Value.vt)
            {
                case VarType.VT_LPSTR:
                    // Hack: pszVal seems to be missing, but pcVal has equal semantics
                    //return Marshal.PtrToStringAnsi(this.Value.inner.pszVal);
                    return Marshal.PtrToStringAnsi(this.Value.inner.pcVal);

                case VarType.VT_LPWSTR:
                    // Hack: pwszVal seems to be missing, but pcVal has the correct type
                    //return Marshal.PtrToStringUni(this.Value.inner.pwszVal);
                    return Marshal.PtrToStringUni(this.Value.inner.pcVal);

                case VarType.VT_BSTR:
                    // Hack: bstrVal seems to be missing, but bstrblobVal has the same type
                    Trace.WriteLine("GetString: BSTR is untested. If you see this message and everything works fine, you can remove this message.");
                    //return Marshal.PtrToStringBSTR(this.Value.inner.bstrVal);
                    return Marshal.PtrToStringBSTR(this.Value.inner.bstrblobVal.pData);

                case VarType.VT_CLSID:
                    return ToGuid().ToString();

                case VarType.VT_DATE:
                    return ToDate().ToString();

                case VarType.VT_BOOL:
                    return ToBool().ToString();

                case VarType.VT_UI4:
                    return ToUInt().ToString();

                case VarType.VT_UI8:
                    return ToUlong().ToString();

                case VarType.VT_ERROR:
                    int error = ToError();
                    string name = Enum.GetName(typeof(HResult), error) ?? error.ToString("X");
                    return $"Error: {name}";
            }

            return $"Unknown type {this.Value.vt}"; 
        }

        public int ToInt()
        {
            if ((VarType)this.Value.vt == VarType.VT_ERROR)
            {
                return 0;
            }

            if ((VarType)this.Value.vt != VarType.VT_INT && (VarType)this.Value.vt != VarType.VT_I4)
            {
                throw new InvalidOperationException($"ToInt does not work for value type {(VarType)this.Value.vt}");
            }

            return this.Value.inner.intVal;
        }

        public uint ToUInt()
        {
            if ((VarType)this.Value.vt == VarType.VT_ERROR)
            {
                return 0;
            }

            if ((VarType)this.Value.vt != VarType.VT_UINT && (VarType)this.Value.vt != VarType.VT_UI4)
            {
                throw new InvalidOperationException($"ToUInt does not work for value type {(VarType)this.Value.vt}");
            }

            return this.Value.inner.uintVal;
        }

        public ulong ToUlong()
        {
            if ((VarType)this.Value.vt == VarType.VT_ERROR)
            {
                return 0;
            }

            if ((VarType)this.Value.vt != VarType.VT_UI8)
            {
                throw new InvalidOperationException($"ToUlong does not work for value type {(VarType)this.Value.vt}");
            }
            
            return this.Value.inner.uhVal.QuadPart;
        }


        public DateTime ToDate()
        {
            if ((VarType)this.Value.vt == VarType.VT_ERROR)
            {
                return new DateTime();
            }

            if ((VarType)this.Value.vt != VarType.VT_DATE)
            {
                throw new InvalidOperationException($"ToDate does not work for value type {(VarType)this.Value.vt}");
            }
            
            return DateTime.FromOADate(this.Value.inner.date);
        }

        public bool ToBool()
        {
            if ((VarType)this.Value.vt == VarType.VT_ERROR)
            {
                return false;
            }

            if ((VarType)this.Value.vt != VarType.VT_BOOL)
            {
                throw new InvalidOperationException($"ToBool does not work for value type {(VarType)this.Value.vt}");
            }

            return this.Value.inner.boolVal != 0;
        }

        public Guid ToGuid()
        {
            if ((VarType)this.Value.vt == VarType.VT_ERROR)
            {
                return new Guid();
            }

            if ((VarType)this.Value.vt != VarType.VT_CLSID)
            {
                throw new InvalidOperationException($"ToGuid does not work for value type {(VarType)this.Value.vt}");
            }

            return (Guid)Marshal.PtrToStructure(this.Value.inner.pcVal, typeof(Guid));
        }

        [HandleProcessCorruptedStateExceptions]
        [SecurityCritical]
        public byte[] ToByteArray()
        {
            if ((VarType)this.Value.vt != (VarType.VT_VECTOR | VarType.VT_UI1))
            {
                throw new InvalidOperationException($"ToByteArray does not work for value type {(VarType)this.Value.vt}");
            }

            int size = (int)this.Value.inner.caub.cElems;
            byte[] managedArray = new byte[size];
            try
            {
                // !!! Does work for x86 and x64 but not for Any CPU !!!
                Marshal.Copy(this.Value.inner.caub.pElems, managedArray, 0, size);
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
                return null;
            }
            return managedArray;
        }

        public int ToError()
        {
            if ((VarType)this.Value.vt != VarType.VT_ERROR)
            {
                return 0;
            }

            return this.Value.inner.scode;
        }
            

        public static PropVariant StringToPropVariant(string value)
        {
            PropVariant pv = new PropVariant();
            pv.Value.vt = (ushort)VarType.VT_LPWSTR;
            // Hack, see GetString
            pv.Value.inner.pcVal = Marshal.StringToCoTaskMemUni(value);
            return pv;
        }

        public static PropVariant UIntToPropVariant(uint value)
        {
            PropVariant pv = new PropVariant();
            pv.Value.vt = (ushort)VarType.VT_UI4;
            pv.Value.inner.ulVal = value;
            return pv;
        }

        public static PropVariant IntToPropVariant(int value)
        {
            PropVariant pv = new PropVariant();
            pv.Value.vt = (ushort)VarType.VT_INT;
            pv.Value.inner.intVal = value;
            return pv;
        }

        public static implicit operator string(PropVariant val)
        {
            return val.ToString();
        }

        public static implicit operator bool(PropVariant val)
        {
            return val.ToBool();
        }

        public static implicit operator DateTime(PropVariant val)
        {
            return val.ToDate();
        }

        public static implicit operator Guid(PropVariant val)
        {
            return val.ToGuid();
        }

        public static implicit operator int(PropVariant val)
        {
            return val.ToInt();
        }

        public static implicit operator ulong(PropVariant val)
        {
            return val.ToUlong();
        }

        public static implicit operator Byte[] (PropVariant val)
        {
            return val.ToByteArray();
        }

        private static class NativeMethods
        {
            [DllImport("ole32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
            static extern public int PropVariantClear(ref PROPVARIANT val);
        }
    }

    /*
    [StructLayout(LayoutKind.Explicit, Size = 32)]
    internal class PropVariant
    { 
        [FieldOffset(0)]
        public VarType variantType;
        [FieldOffset(8)]
        public IntPtr pointerValue;
        [FieldOffset(8)]
        public byte byteValue;
        [FieldOffset(8)]
        public long intValue;
        [FieldOffset(8)]
        public long longValue;
        [FieldOffset(8)]
        public double dateValue;
        [FieldOffset(8)]
        public short boolValue;
        
        public static PropVariant FromValue(PROPVARIANT value)
        {
            
            IntPtr ptrValue = Marshal.AllocHGlobal(Marshal.SizeOf(value));
            Marshal.StructureToPtr(value, ptrValue, false);

            //
            // Marshal the pointer into our C# object
            //
            return (PropVariant)Marshal.PtrToStructure(ptrValue, typeof(PropVariant));
        }

        public override string ToString()
        {
            switch ((VarType)variantType)
            {
            case VarType.VT_LPWSTR:
                return Marshal.PtrToStringUni(pointerValue);

            case VarType.VT_CLSID:
                return ToGuid().ToString();

            case VarType.VT_DATE:
                return ToDate().ToString();

            case VarType.VT_BOOL:
                return ToBool().ToString();

            case VarType.VT_UI4:
                return intValue.ToString();

            case VarType.VT_UI8:
                return longValue.ToString();

            case VarType.VT_ERROR:
                string name = Enum.GetName(typeof(HResult), longValue) ?? longValue.ToString("X");
                return $"Error: {name}";
            }

            return $"Unknown type {variantType}";
        }

        public Guid ToGuid()
        {
            return (Guid)Marshal.PtrToStructure(pointerValue, typeof(Guid));
        }

        public DateTime ToDate()
        {
            return DateTime.FromOADate(dateValue);
        }

        public bool ToBool()
        {
            return Convert.ToBoolean(boolValue);
        }

        public int ToInt()
        {
            return (int)this.intValue;
        }

        public ulong ToUlong()
        {
            return (ulong)this.longValue;
        }

        public Byte[] ToByteArray()
        {
            // TODO

            //byte[] arr = new byte[16];
            ////Marshal.Copy(pointerValue, arr, 0, 199);
            //Marshal.Copy((IntPtr)longValue, arr, 0, 16);
            //return arr;
            
            return null;
        }

        public static PROPVARIANT StringToPropVariant(string value)
        {
            // Tried using the method suggested here:
            // http://blogs.msdn.com/b/dimeby8/archive/2007/01/08/creating-wpd-propvariants-in-c-without-using-interop.aspx
            // However, the GetValue fails (Element Not Found) even though we've just added it.
            // So, I use the alternative (and I think more "correct") approach below.

            var pvSet = new PropVariant
            {
                variantType = VarType.VT_LPWSTR,
                pointerValue = Marshal.StringToCoTaskMemUni(value)
            };

            // Marshal our definition into a pointer
            var ptrValue = Marshal.AllocHGlobal(Marshal.SizeOf(pvSet));
            Marshal.StructureToPtr(pvSet, ptrValue, false);

            // Marshal pointer into the interop PROPVARIANT 
            return (PROPVARIANT)Marshal.PtrToStructure(ptrValue, typeof(PROPVARIANT));
        }

        public static PROPVARIANT IntToPropVariant(int value)
        {
            // Tried using the method suggested here:
            // http://blogs.msdn.com/b/dimeby8/archive/2007/01/08/creating-wpd-propvariants-in-c-without-using-interop.aspx
            // However, the GetValue fails (Element Not Found) even though we've just added it.
            // So, I use the alternative (and I think more "correct") approach below.

            var pvSet = new PropVariant
            {
                variantType = VarType.VT_UI4,
                intValue = value
            };

            // Marshal our definition into a pointer
            var ptrValue = Marshal.AllocHGlobal(Marshal.SizeOf(pvSet));
            Marshal.StructureToPtr(pvSet, ptrValue, false);

            // Marshal pointer into the interop PROPVARIANT 
            return (PROPVARIANT)Marshal.PtrToStructure(ptrValue, typeof(PROPVARIANT));
        }

        public void Dispose()
        {
            this.pointerValue = IntPtr.Zero;
        }

        public static implicit operator PropVariant(PROPVARIANT val)
        {
            return PropVariant.FromValue(val);
        }

        public static implicit operator string(PropVariant val)
        {
            return val.ToString();
        }

        public static implicit operator bool(PropVariant val)
        {
            return val.ToBool();
        }

        public static implicit operator DateTime(PropVariant val)
        {
            return val.ToDate();
        }

        public static implicit operator Guid(PropVariant val)
        {
            return val.ToGuid();
        }

        public static implicit operator int(PropVariant val)
        {
            return val.ToInt();
        }

        public static implicit operator ulong(PropVariant val)
        {
            return val.ToUlong();
        }

        public static implicit operator Byte[](PropVariant val)
        {
            return val.ToByteArray();
        }
    }
    */
}
