using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Snerble.VRC.TouchControls.Shared
{
    public static class KeyframeSerializer
    {
        /// <summary>
        /// Size of the <see cref="Keyframe"/> struct in bytes.
        /// </summary>
        private const int StructSize = 25;

        [Flags]
        private enum Field : byte
        {
            None = 0,
            Time = 1 << 1,
            Value = 1 << 2,
            InTangent = 1 << 3,
            OutTangent = 1 << 4,
            InWeight = 1 << 5,
            OutWeight = 1 << 6,
            WeightedMode = 1 << 7,
            All = byte.MaxValue
        }

        public static string SerializeBase64(params Keyframe[] keyframes)
        {
            using (var mem = new MemoryStream(StructSize * keyframes.Length))
            {
                Serialize(mem, keyframes);
                return Convert.ToBase64String(mem.ToArray());
            }
        }

        public static void Serialize(Stream stream, params Keyframe[] keyframes)
        {
            using (var w = new BinaryWriter(stream, Encoding.UTF8, true))
            {
                foreach (var k in keyframes)
                {
                    var f = (k.time == default ? default : Field.Time)
                        | (k.value == default ? default : Field.Value)
                        | (k.inTangent == default ? default : Field.InTangent)
                        | (k.outTangent == default ? default : Field.OutTangent)
                        | (k.inWeight == default ? default : Field.InWeight)
                        | (k.outWeight == default ? default : Field.OutWeight)
                        | (k.weightedMode == default ? default : Field.WeightedMode);

                    w.Write((byte)f);
                    if (f.HasFlag(Field.Time))
                        w.Write(k.time);
                    if (f.HasFlag(Field.Value))
                        w.Write(k.value);
                    if (f.HasFlag(Field.InTangent))
                        w.Write(k.inTangent);
                    if (f.HasFlag(Field.OutTangent))
                        w.Write(k.outTangent);
                    if (f.HasFlag(Field.InWeight))
                        w.Write(k.inWeight);
                    if (f.HasFlag(Field.OutWeight))
                        w.Write(k.outWeight);
                    if (f.HasFlag(Field.WeightedMode))
                        w.Write((byte)k.weightedMode);
                }
            }
        }

        public static Keyframe[] DeserializeBase64(string base64)
        {
            using (var mem = new MemoryStream(Convert.FromBase64String(base64)))
                return Deserialize(mem).ToArray();
        }

        public static IEnumerable<Keyframe> Deserialize(Stream stream)
        {
            using (var r = new BinaryReader(stream, Encoding.UTF8, true))
            {
                while (r.PeekChar() != -1)
                {
                    var f = (Field)r.ReadByte();

                    yield return new Keyframe()
                    {
                        time = f.HasFlag(Field.Time) ? r.ReadSingle() : default,
                        value = f.HasFlag(Field.Value) ? r.ReadSingle() : default,
                        inTangent = f.HasFlag(Field.InTangent) ? r.ReadSingle() : default,
                        outTangent = f.HasFlag(Field.OutTangent) ? r.ReadSingle() : default,
                        inWeight = f.HasFlag(Field.InWeight) ? r.ReadSingle() : default,
                        outWeight = f.HasFlag(Field.OutWeight) ? r.ReadSingle() : default,
                        weightedMode = f.HasFlag(Field.WeightedMode) ? (WeightedMode)r.ReadByte() : default,
                    };
                }
            }
        }
    }
}
