using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Primary namespace for the Flag library
/// </summary>
namespace FlagUtil {

    /// <summary>
    /// Generic instance of a flag
    /// </summary>
    public struct Flag {
        /// <summary>
        /// Value of this flag instance
        /// </summary>
        public ulong Value { get; set; }

        /// <summary>
        /// Gets a specific bit
        /// </summary>
        /// <param name="i">Index of bit (from right most first)</param>
        /// <returns>Bit at index</returns>
        public short this[int i] {
            get { return ((Value & (uint)(1 << i - 1)) != 0) ? (short)0b1 : (short)0b1; }
        }

        /// <summary>
        /// Creates a new instace of a flag(s)
        /// </summary>
        /// <param name="flag">Enums to create flag from</param>
        public Flag(params Enum[] flag) {
            Value = EnumToLong(flag[0]);
            Merge(flag);
        }

        /// <summary>
        /// Creates a new instace of a flag
        /// </summary>
        /// <param name="flag">Enum to create flag from</param>
        public Flag(Enum flag) {
            Value = EnumToLong(flag);
        }

        /// <summary>
        /// Creates a new instance of a flag(s)
        /// </summary>
        /// <param name="flag">Flag to set</param>
        public Flag(params ulong[] flag) {
            Value = flag[0];
            Merge(flag);
        }

        /// <summary>
        /// Creates a new instance of a flag
        /// </summary>
        /// <param name="flag">Flag to set</param>
        public Flag(ulong flag) {
            Value = flag;
        }

        /// <summary>
        /// Copies an instance of a flag(s)
        /// </summary>
        /// <param name="flag">Flag to copy</param>
        public Flag(params Flag[] flag) {
            Value = flag[0].Value;
            Merge(flag);
        }

        /// <summary>
        /// Copies an instance of a flag
        /// </summary>
        /// <param name="flag">Flag to copy</param>
        public Flag(Flag flag) {
            Value = flag.Value;
        }

        /// <summary>
        /// Converts this flag to an enum
        /// </summary>
        /// <typeparam name="T">Type of enum</typeparam>
        /// <returns>Flag represented as an enum</returns>
        public T ToEnum<T>() where T : struct, IConvertible {
            Type t = typeof(T);

            if (!t.IsEnum)
                throw new ArgumentException("<T> must be of type Enum.");

            return (T)Enum.Parse(t, Value.ToString());
        }

        /// <summary>
        /// Inverts this Flag
        /// </summary>
        /// <example>
        /// Example of an invert operation:
        /// <code>
        /// Flag a = new Flag(0b0000_1111);
        /// a.Invert();
        /// 
        /// // a == 0b1111_0000
        /// </code>
        /// </example>
        public void Invert() {
            Value = ~Value;
        }

        /// <summary>
        /// Sets this flag to another flag(s). Will merge multiple.
        /// </summary>
        /// <param name="flag">Flag(s) to set</param>
        /// <example>
        /// The following is an example of a set operation:
        /// <code>
        ///     Flag a = new Flag(0b0000_1111);
        ///     a.Set(0b1100_0000, 0b0011_0000);
        ///     
        ///     a == 0b1111_0000
        /// </code>
        /// </example>
        public void Set(params Enum[] flag) {
            Value = 0;
            for (int i = 0; i < flag.Length; i++)
                Value |= EnumToLong(flag[i]);
        }

        /// <summary>
        /// Sets this flag to another flag.
        /// </summary>
        /// <param name="flag">Flag to set</param>
        /// <example>
        /// The following is an example of a set operation:
        /// <code>
        ///     Flag a = new Flag(0b0000_1111);
        ///     a.Set(0b1100_0000);
        ///     
        ///     a == 0b1100_0000
        /// </code>
        /// </example>
        public void Set(Enum flag) {
            Value = EnumToLong(flag);
        }

        /// <summary>
        /// Sets this flag to another flag(s). Will merge multiple.
        /// </summary>
        /// <param name="flag">Flag(s) to set</param>
        /// <example>
        /// The following is an example of a set operation:
        /// <code>
        ///     Flag a = new Flag(0b0000_1111);
        ///     a.Set(0b1100_0000, 0b0011_0000);
        ///     
        ///     a == 0b1111_0000
        /// </code>
        /// </example>
        public void Set(params ulong[] flag) {
            Value = 0;
            for (int i = 0; i < flag.Length; i++)
                Value |= flag[i];
        }

        /// <summary>
        /// Sets this flag to another flag.
        /// </summary>
        /// <param name="flag">Flag to set</param>
        /// <example>
        /// The following is an example of a set operation:
        /// <code>
        ///     Flag a = new Flag(0b0000_1111);
        ///     a.Set(0b1100_0000);
        ///     
        ///     a == 0b1100_0000
        /// </code>
        /// </example>
        public void Set(ulong flag) {
            Value = flag;
        }

        /// <summary>
        /// Sets this flag to another flag(s). Will merge multiple.
        /// </summary>
        /// <param name="flag">Flag(s) to set</param>
        /// <example>
        /// The following is an example of a set operation:
        /// <code>
        ///     Flag a = new Flag(0b0000_1111);
        ///     a.Set(0b1100_0000, 0b0011_0000);
        ///     
        ///     a == 0b1111_0000
        /// </code>
        /// </example>
        public void Set(params Flag[] flag) {
            Value = 0;
            for (int i = 0; i < flag.Length; i++)
                Value |= flag[i].Value;
        }

        /// <summary>
        /// Sets this flag to another flag.
        /// </summary>
        /// <param name="flag">Flag to set</param>
        /// <example>
        /// The following is an example of a set operation:
        /// <code>
        ///     Flag a = new Flag(0b0000_1111);
        ///     a.Set(0b1100_0000);
        ///     
        ///     a == 0b1100_0000
        /// </code>
        /// </example>
        public void Set(Flag flag) {
            Value = flag.Value;
        }

        /// <summary>
        /// Joins flag(s) into this instance
        /// </summary>
        /// <param name="flag">Flag(s) to combine</param>
        /// <example>
        /// The following is an example of a merge operation:
        /// <code>
        ///     Flag a = new Flag(0b0000_1111);
        ///     a.Join(0b1111_0000);
        ///     
        ///     a == 0b1111_1111
        /// </code>
        /// </example>
        public void Merge(params Enum[] flag) {
            for (int i = 0; i < flag.Length; i++)
                Value |= EnumToLong(flag[i]);
        }

        /// <summary>
        /// Joins a flag into this instance
        /// </summary>
        /// <param name="flag">Flag to combine</param>
        /// <example>
        /// The following is an example of a merge operation:
        /// <code>
        ///     Flag a = new Flag(0b0000_1111);
        ///     a.Join(0b1111_0000);
        ///     
        ///     a == 0b1111_1111
        /// </code>
        /// </example>
        public void Merge(Enum flag) {
            Value |= EnumToLong(flag);
        }

        /// <summary>
        /// Joins flag(s) into this instance
        /// </summary>
        /// <param name="flag">Flag(s) to combine</param>
        /// <example>
        /// The following is an example of a merge operation:
        /// <code>
        ///     Flag a = new Flag(0b0000_1111);
        ///     a.Join(0b1111_0000);
        ///     
        ///     a == 0b1111_1111
        /// </code>
        /// </example>
        public void Merge(params ulong[] flag) {
            for (int i = 0; i < flag.Length; i++)
                Value |= flag[i];
        }

        /// <summary>
        /// Joins a flag into this instance
        /// </summary>
        /// <param name="flag">Flag to combine</param>
        /// <example>
        /// The following is an example of a merge operation:
        /// <code>
        ///     Flag a = new Flag(0b0000_1111);
        ///     a.Join(0b1111_0000);
        ///     
        ///     a == 0b1111_1111
        /// </code>
        /// </example>
        public void Merge(ulong flag) {
            Value |= flag;
        }

        /// <summary>
        /// Joins flag(s) into this instance
        /// </summary>
        /// <param name="flags">Flag(s) to combine</param>
        /// <example>
        /// The following is an example of a merge operation:
        /// <code>
        ///     Flag a = new Flag(0b0000_1111);
        ///     a.Join(0b1111_0000);
        ///     
        ///     a == 0b1111_1111
        /// </code>
        /// </example>
        public void Merge(params Flag[] flag) {
            for (int i = 0; i < flag.Length; i++)
                Value |= flag[i].Value;
        }

        /// <summary>
        /// Joins a flag into this instance
        /// </summary>
        /// <param name="flags">Flag to combine</param>
        /// <example>
        /// The following is an example of a merge operation:
        /// <code>
        ///     Flag a = new Flag(0b0000_1111);
        ///     a.Join(0b1111_0000);
        ///     
        ///     a == 0b1111_1111
        /// </code>
        /// </example>
        public void Merge(Flag flag) {
            Value |= flag.Value;
        }

        /// <summary>
        /// Removes flag(s) markers from this instance
        /// </summary>
        /// <param name="flag">Flag(s) to remove</param>
        /// <example>
        /// The following is an example of a remove operation:
        /// <code>
        ///     Flag a = new Flag(0b0000_0110);
        ///     a.Remove(0b0000_0100);
        ///     
        ///     a == 0b0000_0010
        /// </code>
        /// </example>
        public void Remove(params Enum[] flag) {
            ulong f = MergeNew(flag).Value;
            Value &= ~f;
        }

        /// <summary>
        /// Removes a flag marker from this instance
        /// </summary>
        /// <param name="flag">Flag to remove</param>
        /// <example>
        /// The following is an example of a remove operation:
        /// <code>
        ///     Flag a = new Flag(0b0000_0110);
        ///     a.Remove(0b0000_0100);
        ///     
        ///     a == 0b0000_0010
        /// </code>
        /// </example>
        public void Remove(Enum flag) {
            Value &= ~(EnumToLong(flag));
        }

        /// <summary>
        /// Removes flag(s) markers from this instance
        /// </summary>
        /// <param name="flag">Flag(s) to remove</param>
        /// <example>
        /// The following is an example of a remove operation:
        /// <code>
        ///     Flag a = new Flag(0b0000_0110);
        ///     a.Remove(0b0000_0100);
        ///     
        ///     a == 0b0000_0010
        /// </code>
        /// </example>
        public void Remove(params ulong[] flag) {
            ulong f = MergeNew(flag);
            Value &= ~f;
        }

        /// <summary>
        /// Removes a flag markers from this instance
        /// </summary>
        /// <param name="flag">Flag to remove</param>
        /// <example>
        /// The following is an example of a remove operation:
        /// <code>
        ///     Flag a = new Flag(0b0000_0110);
        ///     a.Remove(0b0000_0100);
        ///     
        ///     a == 0b0000_0010
        /// </code>
        /// </example>
        public void Remove(ulong flag) {
            Value &= ~flag;
        }

        /// <summary>
        /// Removes flag(s) markers from this instance
        /// </summary>
        /// <param name="flag">Flag(s) to remove</param>
        /// <example>
        /// The following is an example of a remove operation:
        /// <code>
        ///     Flag a = new Flag(0b0000_0110);
        ///     a.Remove(0b0000_0100);
        ///     
        ///     a == 0b0000_0010
        /// </code>
        /// </example>
        public void Remove(params Flag[] flag) {
            Remove(MergeNew(flag).Value);
        }

        /// <summary>
        /// Removes a flag markers from this instance
        /// </summary>
        /// <param name="flag">Flag to remove</param>
        /// <example>
        /// The following is an example of a remove operation:
        /// <code>
        ///     Flag a = new Flag(0b0000_0110);
        ///     a.Remove(0b0000_0100);
        ///     
        ///     a == 0b0000_0010
        /// </code>
        /// </example>
        public void Remove(Flag flag) {
            Remove(flag.Value);
        }

        /// <summary>
        /// Checks if this instance matches other flag(s). Multiple flags are combined.
        /// </summary>
        /// <param name="flag">Flag to check</param>
        /// <returns>If the flag matches or not</returns>
        /// <example>
        /// The following is an example of a match using binary notation:
        /// <code>
        ///     (param) 0000_0100 == (instance) 0110_0100
        /// </code>
        /// This is an example of a non-match:
        /// <code>
        ///     (param) 0000_1000 == (instance) 0110_0100
        /// </code>
        /// </example>
        public bool Match(params Enum[] flag) {
            ulong f = (flag.Length > 0) ? MergeNew(flag).Value : EnumToLong(flag[0]);
            return (Value & f) == f;
        }

        /// <summary>
        /// Checks if this instance matches another flag.
        /// </summary>
        /// <param name="flag">Flag to check</param>
        /// <returns>If the flag matches or not</returns>
        /// <example>
        /// The following is an example of a match using binary notation:
        /// <code>
        ///     (param) 0000_0100 == (instance) 0110_0100
        /// </code>
        /// This is an example of a non-match:
        /// <code>
        ///     (param) 0000_1000 == (instance) 0110_0100
        /// </code>
        /// </example>
        public bool Match(Enum flag) {
            ulong l = EnumToLong(flag);
            return (Value & l) == l;
        }

        /// <summary>
        /// Checks if this instance matches other flag(s). Multiple flags are combined.
        /// </summary>
        /// <param name="flag">Flag to check</param>
        /// <returns>If the flag matches or not</returns>
        /// <example>
        /// The following is an example of a match using binary notation:
        /// <code>
        ///     (param) 0000_0100 == (instance) 0110_0100
        /// </code>
        /// This is an example of a non-match:
        /// <code>
        ///     (param) 0000_1000 == (instance) 0110_0100
        /// </code>
        /// </example>
        public bool Match(params ulong[] flag) {
            ulong f = (flag.Length > 0) ? MergeNew(flag) : flag[0];
            return (Value & f) == f;
        }

        /// <summary>
        /// Checks if this instance matches other flag.
        /// </summary>
        /// <param name="flag">Flag to check</param>
        /// <returns>If the flag matches or not</returns>
        /// <example>
        /// The following is an example of a match using binary notation:
        /// <code>
        ///     (param) 0000_0100 == (instance) 0110_0100
        /// </code>
        /// This is an example of a non-match:
        /// <code>
        ///     (param) 0000_1000 == (instance) 0110_0100
        /// </code>
        /// </example>
        public bool Match(ulong flag) {
            return (Value & flag) == flag;
        }

        /// <summary>
        /// Checks if this instance matches other flag(s). Multiple flags are combined.
        /// </summary>
        /// <param name="flag">Flag to check</param>
        /// <returns>If the flag matches or not</returns>
        /// <example>
        /// The following is an example of a match using binary notation:
        /// <code>
        ///     (param) 0000_0100 == (instance) 0110_0100
        /// </code>
        /// This is an example of a non-match:
        /// <code>
        ///     (param) 0000_1000 == (instance) 0110_0100
        /// </code>
        /// </example>
        public bool Match(params Flag[] flag) {
            return Match(MergeNew(flag).Value);
        }

        /// <summary>
        /// Checks if this instance matches another flag.
        /// </summary>
        /// <param name="flag">Flag to check</param>
        /// <returns>If the flag matches or not</returns>
        /// <example>
        /// The following is an example of a match using binary notation:
        /// <code>
        ///     (param) 0000_0100 == (instance) 0110_0100
        /// </code>
        /// This is an example of a non-match:
        /// <code>
        ///     (param) 0000_1000 == (instance) 0110_0100
        /// </code>
        /// </example>
        public bool Match(Flag flag) {
            return Match(flag.Value);
        }

        /// <summary>
        /// Checks if this instance matches another flag(s) exactly. Multiple flags are combined.
        /// </summary>
        /// <param name="flag">Flag(s) to check</param>
        /// <returns>If the flag exactly matches or not</returns>
        /// <example>
        /// The following is an example of a match using binary notation:
        /// <code>
        ///     (param) 0110_0100 == (instance) 0110_0100
        /// </code>
        /// This is an example of a non-match:
        /// <code>
        ///     (param) 0000_0100 == (instance) 0110_0100
        /// </code>
        /// </example>
        public bool MatchExact(params Enum[] flag) {
            return Value == MergeNew(flag).Value;
        }

        /// <summary>
        /// Checks if this instance matches another flag exactly.
        /// </summary>
        /// <param name="flag">Flag to check</param>
        /// <returns>If the flag exactly matches or not</returns>
        /// <example>
        /// The following is an example of a match using binary notation:
        /// <code>
        ///     (param) 0110_0100 == (instance) 0110_0100
        /// </code>
        /// This is an example of a non-match:
        /// <code>
        ///     (param) 0000_0100 == (instance) 0110_0100
        /// </code>
        /// </example>
        public bool MatchExact(Enum flag) {
            return Value == EnumToLong(flag);
        }

        /// <summary>
        /// Checks if this instance matches another flag(s) exactly. Multiple flags are combined.
        /// </summary>
        /// <param name="flag">Flag(s) to check</param>
        /// <returns>If the flag exactly matches or not</returns>
        /// <example>
        /// The following is an example of a match using binary notation:
        /// <code>
        ///     (param) 0110_0100 == (instance) 0110_0100
        /// </code>
        /// This is an example of a non-match:
        /// <code>
        ///     (param) 0000_0100 == (instance) 0110_0100
        /// </code>
        /// </example>
        public bool MatchExact(params ulong[] flag) {
            return Value == MergeNew(flag);
        }

        /// <summary>
        /// Checks if this instance matches another flag exactly.
        /// </summary>
        /// <param name="flag">Flag to check</param>
        /// <returns>If the flag exactly matches or not</returns>
        /// <example>
        /// The following is an example of a match using binary notation:
        /// <code>
        ///     (param) 0110_0100 == (instance) 0110_0100
        /// </code>
        /// This is an example of a non-match:
        /// <code>
        ///     (param) 0000_0100 == (instance) 0110_0100
        /// </code>
        /// </example>
        public bool MatchExact(ulong flag) {
            return Value == flag;
        }

        /// <summary>
        /// Checks if this instance matches another flag(s) exactly. Multiple flags are combined.
        /// </summary>
        /// <param name="flag">Flag(s) to check</param>
        /// <returns>If the flag exactly matches or not</returns>
        /// <example>
        /// The following is an example of a match using binary notation:
        /// <code>
        ///     (param) 0110_0100 == (instance) 0110_0100
        /// </code>
        /// This is an example of a non-match:
        /// <code>
        ///     (param) 0000_0100 == (instance) 0110_0100
        /// </code>
        /// </example>
        public bool MatchExact(params Flag[] flag) {
            return MatchExact(MergeNew(flag).Value);
        }

        /// <summary>
        /// Checks if this instance matches another flag exactly.
        /// </summary>
        /// <param name="flag">Flag to check</param>
        /// <returns>If the flag exactly matches or not</returns>
        /// <example>
        /// The following is an example of a match using binary notation:
        /// <code>
        ///     (param) 0110_0100 == (instance) 0110_0100
        /// </code>
        /// This is an example of a non-match:
        /// <code>
        ///     (param) 0000_0100 == (instance) 0110_0100
        /// </code>
        /// </example>
        public bool MatchExact(Flag flag) {
            return MatchExact(flag.Value);
        }

        /// <summary>
        /// Tries to match any flag(s)
        /// </summary>
        /// <param name="flags">Flags to match</param>
        /// <returns>True if any matched</returns>
        /// <example>
        /// The following is an example of a any match operation:
        /// <code>
        ///     Flag<EnumType> a = new Flag(0b0000_0110);
        ///     a.MatchAny(0b0000_0001, 0b0000_0010) // true
        /// </code>
        /// </example>
        public bool MatchAny(params Enum[] flags) {
            foreach (Enum f in flags)
                if (Match(f)) return true;

            return false;
        }

        /// <summary>
        /// Tries to match any flag(s), assigns the matched flag to the out parameter
        /// </summary>
        /// <param name="match">Matched flag. Will set match to 0 if no match found.</param>    
        /// <param name="flags">Flags to match</param>
        /// <returns>True if any matched</returns>
        /// <example>
        /// The following is an example of a any match operation:
        /// <code>
        ///     Flag<EnumType> a = new Flag(0b0000_0110);
        ///     ulong m;    
        /// 
        ///     a.MatchAny(out m, 0b0000_0001, 0b0000_0010) // true
        ///     // m = 0b0000_0010
        /// </code>
        /// </example>
        public bool MatchAny(out Enum match, params Enum[] flags) {
            foreach (Enum f in flags) {
                if (Match(f)) {
                    match = f;
                    return true;
                }
            }

            match = null;
            return false;
        }

        /// <summary>
        /// Tries to match any flag(s)
        /// </summary>
        /// <param name="flags">Flags to match</param>
        /// <returns>True if any matched</returns>
        /// <example>
        /// The following is an example of a any match operation:
        /// <code>
        ///     Flag<EnumType> a = new Flag(0b0000_0110);
        ///     a.MatchAny(0b0000_0001, 0b0000_0010) // true
        /// </code>
        /// </example>
        public bool MatchAny(params ulong[] flags) {
            foreach (ulong f in flags)
                if (Match(f)) return true;

            return false;
        }

        /// <summary>
        /// Tries to match any flag(s), assigns the matched flag to the out parameter
        /// </summary>
        /// <param name="match">Matched flag. Will set match to null if no match found.</param>    
        /// <param name="flags">Flags to match</param>
        /// <returns>True if any matched</returns>
        /// <example>
        /// The following is an example of a any match operation:
        /// <code>
        ///     Flag<EnumType> a = new Flag(0b0000_0110);
        ///     ulong m;    
        /// 
        ///     a.MatchAny(out m, 0b0000_0001, 0b0000_0010) // true
        ///     // m = 0b0000_0010
        /// </code>
        /// </example>
        public bool MatchAny(out ulong match, params ulong[] flags) {
            foreach (ulong f in flags) {
                if (Match(f)) {
                    match = f;
                    return true;
                }
            }

            match = 0;
            return false;
        }

        /// <summary>
        /// Tries to match any flag(s), assigns the matched flag to the out parameter
        /// </summary>
        /// <param name="flags">Flags to match</param>
        /// <returns>True if any matched</returns>
        /// <example>
        /// The following is an example of a any match operation:
        /// <code>
        ///     Flag<EnumType> a = new Flag(0b0000_0110);
        ///     ulong m;    
        /// 
        ///     a.MatchAny(out m, 0b0000_0001, 0b0000_0010) // true
        ///     // m = 0b0000_0010
        /// </code>
        /// </example>
        public bool MatchAny(params Flag[] flags) {
            foreach (Flag f in flags)
                if (Match(f)) return true;

            return false;
        }

        /// <summary>
        /// Tries to match any flag(s), assigns the matched flag to the out parameter
        /// </summary>
        /// <param name="match">Matched flag. Will set match to 0 if no match found.</param>    
        /// <param name="flags">Flags to match</param>
        /// <returns>True if any matched</returns>
        /// <example>
        /// The following is an example of a any match operation:
        /// <code>
        ///     Flag<EnumType> a = new Flag(0b0000_0110);
        ///     ulong m;    
        /// 
        ///     a.MatchAny(out m, 0b0000_0001, 0b0000_0010) // true
        ///     // m = 0b0000_0010
        /// </code>
        /// </example>
        public bool MatchAny(out Flag match, params Flag[] flags) {
            foreach (Flag f in flags) {
                if (Match(f)) {
                    match = f;
                    return true;
                }
            }

            match = new Flag();
            return false;
        }

        /// <summary>
        /// Creates a new flag by joining this instance with another flag(s).
        /// </summary>
        /// <param name="flag">Flag(s) to join</param>
        /// <returns>The joined flags</returns>
        public Flag MergeNew(params Enum[] flag) {
            ulong v = Value;

            for (int i = 0; i < flag.Length; i++)
                v |= EnumToLong(flag[i]);

            return new Flag(v);
        }

        /// <summary>
        /// Creates a new flag by joining this instance with another flag.
        /// </summary>
        /// <param name="flag">Flag to join</param>
        /// <returns>The joined flags</returns>
        public Flag MergeNew(Enum flag) {
            return new Flag(Value | EnumToLong(flag));
        }

        /// <summary>
        /// Creates a new flag by joining this instance with another flag(s).
        /// </summary>
        /// <param name="flag">Flag(s) to join</param>
        /// <returns>The joined flags</returns>
        public ulong MergeNew(params ulong[] flag) {
            ulong v = Value;

            for (int i = 0; i < flag.Length; i++)
                v |= flag[i];

            return v;
        }

        /// <summary>
        /// Creates a new flag by joining this instance with another flag.
        /// </summary>
        /// <param name="flag">Flag to join</param>
        /// <returns>The joined flags</returns>
        public ulong MergeNew(ulong flag) {
            return Value | flag;
        }

        /// <summary>
        /// Creates a new flag by joining this instance with another flag(s).
        /// </summary>
        /// <param name="flag">Flag(s) to join</param>
        /// <returns>The joined flags</returns>
        public Flag MergeNew(params Flag[] flag) {
            Flag f = new Flag(Value);
            f.Merge(flag);

            return f;
        }

        /// <summary>
        /// Creates a new flag by joining this instance with another flag.
        /// </summary>
        /// <param name="flag">Flag to join</param>
        /// <returns>The joined flags</returns>
        public Flag MergeNew(Flag flag) {
            return new Flag(this, flag);
        }

        /// <summary>
        /// Converts an enum into a long
        /// </summary>
        /// <param name="e">Enum to convet</param>
        /// <returns>Long representation</returns>
        public static ulong EnumToLong(Enum e) {
            return Convert.ToUInt64(e);
        }

        /// <summary>
        /// Inverts a flag
        /// </summary>
        /// <param name="subject">Subject to invert</param>
        /// <returns>Inverted flag</returns>
        public static Flag operator ~(Flag subject) {
            subject.Invert();
            return subject;
        }

        /// <summary>
        /// Inverts a flag
        /// </summary>
        /// <param name="subject">Subject to invert</param>
        /// <returns>Inverted flag</returns>
        public static Flag operator !(Flag subject) {
            subject.Invert();
            return subject;
        }

        /// <summary>
        /// Returns true if two flags do not Match
        /// </summary>
        /// <param name="a">Flag a</param>
        /// <param name="b">Flag b</param>
        /// <returns></returns>
        public static bool operator !=(Flag a, object b) {
            return !a.Equals(b);
        }

        /// <summary>
        /// Returns true if two flags Match
        /// </summary>
        /// <param name="a">Flag a</param>
        /// <param name="b">Flag b</param>
        /// <returns></returns>
        public static bool operator ==(Flag a, object b) {
            return a.Equals(b);
        }

        /// <summary>
        /// Removes a pattern flag from a subject flag
        /// </summary>
        /// <param name="a">Subject flag to remove the pattern from</param>
        /// <param name="b">Pattern to remove</param>
        /// <returns>Subject flag withou the pattern</returns>
        public static Flag operator -(Flag subject, Flag pattern) {
            subject.Remove(pattern);
            return subject;
        }
        
        /// <summary>
        /// Join two flags
        /// </summary>
        /// <param name="a">Flag a</param>
        /// <param name="b">Flag b</param>
        /// <returns>Joined Flag a and b</returns>
        public static Flag operator +(Flag a, Flag b) {
            a.Merge(b);
            return a;
        }

        /// <summary>
        /// Gets the hash code for this flag
        /// </summary>
        /// <returns>Hash</returns>
        public override int GetHashCode() {
            return Value.GetHashCode();
        }

        /// <summary>
        /// Checks if this instance matches another flag.
        /// </summary>
        /// <param name="flag">Flag to check. Can be <code>Flag</code> type or any numerical type that supports conversion to <code>long</code> type</param>
        /// <returns>If the flag matches or not</returns>
        /// <example>
        /// The following is an example of a match using binary notation:
        /// <code>
        ///     (param) 0000_0100 == (instance) 0110_0100
        /// </code>
        /// This is an example of a non-match:
        /// <code>
        ///     (param) 0000_1000 == (instance) 0110_0100
        /// </code>
        /// </example>
        public override bool Equals(object obj) {
            Flag f;

            if (obj.GetType() == typeof(Flag)) {
                f = (Flag)obj;
            } else {
                if (obj is Enum) {
                    f = new Flag(EnumToLong((Enum) obj));
                } else {
                    try {
                        f = new Flag(Convert.ToUInt64(obj));
                    } catch (FormatException e) {
                        FormatException n = new FormatException($"Unable to compare {obj.GetType()} {obj} to Flag type", e);
                        throw n;
                    }
                }
            }

            return Match(f);
        }

        /// <summary>
        /// String representation of the flag
        /// </summary>
        /// <returns>String representation</returns>
        public override string ToString() {
            return Convert.ToString((long)Value, 2);
        }
    }
}
