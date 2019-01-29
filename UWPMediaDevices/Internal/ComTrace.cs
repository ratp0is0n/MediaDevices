using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace MediaDevices.Internal
{
    // to enable COM traces add "COMTRACE" to the Build Conditional compilation symbols of the MediaDevice project.

    internal static class ComTrace
    {
        private static List<FieldInfo> pkeyFields;
        private static List<FieldInfo> guidFields;

        static ComTrace()
        {
            pkeyFields = typeof(WPD).GetFields().Where(f => f.FieldType == typeof(PropertyKey)).ToList();
            guidFields = typeof(WPD).GetFields().Where(f => f.FieldType == typeof(Guid)).ToList();
        }

        private static FieldInfo FindPropertyKeyField(PropertyKey key)
        {
            return pkeyFields.SingleOrDefault(i => ((PropertyKey)i.GetValue(null)).pid == key.pid && ((PropertyKey)i.GetValue(null)).fmtid == key.fmtid);
        }
        
        private static FieldInfo FindGuidField(Guid guid)
        {
            return guidFields.SingleOrDefault(i => ((Guid)i.GetValue(null)) == guid);
        }

        [Conditional("COMTRACE")]
        public static void WriteObject(IPortableDeviceValues values)
        {
            InternalWriteObject(values);
        }

        [Conditional("COMTRACE")]
        public static void WriteObject(IPortableDeviceProperties deviceProperties, string objectId)
        {
            IPortableDeviceKeyCollection keys;
            deviceProperties.GetSupportedProperties(objectId, out keys);

            IPortableDeviceValues values;
            deviceProperties.GetValues(objectId, keys, out values);

            InternalWriteObject(values);
        }

        [Conditional("COMTRACE")]
        private static void InternalWriteObject(IPortableDeviceValues values)
        {
#if WINDOWS_UWP
#else
            string func = new StackTrace().GetFrame(2).GetMethod().Name;
            Debug.WriteLine($"############################### {func}");   
#endif
            uint num = 0;

            values.GetCount(ref num);
            for (uint i = 0; i < num; i++)
            {
                PropertyKey key = new PropertyKey();
                PropVariantFacade val = new PropVariantFacade();
                values.GetAt(i, ref key, ref val.Value);

                FieldInfo field = FindPropertyKeyField(key);
                

                string fieldName = field?.Name ?? $"{key.fmtid}, {key.pid}";

                switch (val.VariantType)
                {
                case PropVariantType.VT_CLSID:
                    Debug.WriteLine($"##### {fieldName} = {FindGuidField(val.ToGuid())?.Name ?? val.ToString()}");
                    break;
                default:
                    Debug.WriteLine($"##### {fieldName} = {val.ToString()}");
                    break;
                }
            }
        }

        [Conditional("COMTRACE")]
        public static void WriteObject(IPortableDevicePropVariantCollection collection)
        {
            Debug.WriteLine("###############################");
            uint num = 0;
            collection.GetCount(ref num);
            for (uint index = 0; index < num; index++)
            {
                using (PropVariantFacade val = new PropVariantFacade())
                {
                    collection.GetAt(index, ref val.Value);

                    Debug.WriteLine($"##### {val.ToString()}");
                }
            }
        }

        [Conditional("COMTRACE")]
        public static void WriteObject(IPortableDeviceKeyCollection collection)
        {
            Debug.WriteLine("###############################");
            uint num = 0;
            collection.GetCount(ref num);
            for (uint index = 0; index < num; index++)
            {
                PropertyKey key = new PropertyKey();
                collection.GetAt(index, ref key);

                PropertyKeys propertyKey = key.GetEnumFromAttrKey<PropertyKeys>();
                Debug.WriteLine($"##### {propertyKey}");
            }
        }
    }
}
