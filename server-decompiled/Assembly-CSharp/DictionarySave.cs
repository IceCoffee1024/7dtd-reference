using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MemoryPack;
using MemoryPack.Formatters;
using MemoryPack.Internal;

// Token: 0x02001416 RID: 5142
[MemoryPackable(GenerateType.Object)]
public class DictionarySave<T1, T2> : IMemoryPackable<DictionarySave<T1, T2>>, IMemoryPackFormatterRegister where T2 : class
{
	// Token: 0x0600A0D1 RID: 41169 RVA: 0x003C9064 File Offset: 0x003C7264
	[MemoryPackConstructor]
	public DictionarySave()
	{
	}

	// Token: 0x1700132E RID: 4910
	public virtual T2 this[T1 _v]
	{
		get
		{
			if (!DictionarySave<T1, T2>.KeyIsValuetype && _v == null)
			{
				return default(T2);
			}
			T2 t;
			if (this.dic.TryGetValue(_v, out t))
			{
				return t;
			}
			return default(T2);
		}
		set
		{
			this.dic[_v] = value;
		}
	}

	// Token: 0x1700132F RID: 4911
	// (get) Token: 0x0600A0D4 RID: 41172 RVA: 0x003C90D4 File Offset: 0x003C72D4
	public Dictionary<T1, T2> Dict
	{
		get
		{
			return this.dic;
		}
	}

	// Token: 0x0600A0D5 RID: 41173 RVA: 0x003C90DC File Offset: 0x003C72DC
	public bool ContainsKey(T1 _key)
	{
		return this.dic.ContainsKey(_key);
	}

	// Token: 0x0600A0D6 RID: 41174 RVA: 0x003C90EA File Offset: 0x003C72EA
	public bool TryGetValue(T1 _key, out T2 _value)
	{
		return this.dic.TryGetValue(_key, out _value);
	}

	// Token: 0x0600A0D7 RID: 41175 RVA: 0x003C90F9 File Offset: 0x003C72F9
	public void Add(T1 _key, T2 _value)
	{
		this.dic.Add(_key, _value);
	}

	// Token: 0x0600A0D8 RID: 41176 RVA: 0x003C9108 File Offset: 0x003C7308
	public void Remove(T1 _key)
	{
		this.dic.Remove(_key);
	}

	// Token: 0x0600A0D9 RID: 41177 RVA: 0x003C9117 File Offset: 0x003C7317
	public void Clear()
	{
		this.dic.Clear();
	}

	// Token: 0x0600A0DA RID: 41178 RVA: 0x003C9124 File Offset: 0x003C7324
	public void MarkToRemove(T1 _v)
	{
		this.toRemove.Add(_v);
	}

	// Token: 0x0600A0DB RID: 41179 RVA: 0x003C9134 File Offset: 0x003C7334
	public void RemoveAllMarked(DictionarySave<T1, T2>.DictionaryRemoveCallback _callback)
	{
		foreach (T1 t in this.toRemove)
		{
			_callback(t);
		}
		this.toRemove.Clear();
	}

	// Token: 0x17001330 RID: 4912
	// (get) Token: 0x0600A0DC RID: 41180 RVA: 0x003C9194 File Offset: 0x003C7394
	public int Count
	{
		get
		{
			return this.dic.Count;
		}
	}

	// Token: 0x0600A0DD RID: 41181 RVA: 0x003C91A1 File Offset: 0x003C73A1
	[PublicizedFrom(EAccessModifier.Private)]
	static DictionarySave()
	{
		DictionarySave<T1, T2>.RegisterFormatter();
	}

	// Token: 0x0600A0DE RID: 41182 RVA: 0x003C91BC File Offset: 0x003C73BC
	[Preserve]
	public static void RegisterFormatter()
	{
		if (!MemoryPackFormatterProvider.IsRegistered<DictionarySave<T1, T2>>())
		{
			MemoryPackFormatterProvider.Register<DictionarySave<T1, T2>>(new DictionarySave<T1, T2>.DictionarySaveFormatter());
		}
		if (!MemoryPackFormatterProvider.IsRegistered<DictionarySave<T1, T2>[]>())
		{
			MemoryPackFormatterProvider.Register<DictionarySave<T1, T2>[]>(new ArrayFormatter<DictionarySave<T1, T2>>());
		}
		if (!MemoryPackFormatterProvider.IsRegistered<Dictionary<T1, T2>>())
		{
			MemoryPackFormatterProvider.Register<Dictionary<T1, T2>>(new DictionaryFormatter<T1, T2>());
		}
	}

	// Token: 0x0600A0DF RID: 41183 RVA: 0x003C91F4 File Offset: 0x003C73F4
	[Preserve]
	public static void Serialize(ref MemoryPackWriter writer, [Nullable(new byte[] { 2, 1, 1 })] ref DictionarySave<T1, T2> value)
	{
		if (value == null)
		{
			writer.WriteNullObjectHeader();
			return;
		}
		writer.WriteObjectHeader(3);
		writer.WriteValue<Dictionary<T1, T2>>(in value.dic);
		Dictionary<T1, T2> dict = value.Dict;
		writer.WriteValue<Dictionary<T1, T2>>(in dict);
		int count = value.Count;
		writer.WriteUnmanaged<int>(in count);
	}

	// Token: 0x0600A0E0 RID: 41184 RVA: 0x003C9240 File Offset: 0x003C7440
	[Preserve]
	public static void Deserialize(ref MemoryPackReader reader, [Nullable(new byte[] { 2, 1, 1 })] ref DictionarySave<T1, T2> value)
	{
		byte b;
		if (!reader.TryReadObjectHeader(out b))
		{
			value = null;
			return;
		}
		Dictionary<T1, T2> dictionary;
		if (b == 3)
		{
			Dictionary<T1, T2> dictionary2;
			int num;
			if (value == null)
			{
				dictionary = reader.ReadValue<Dictionary<T1, T2>>();
				dictionary2 = reader.ReadValue<Dictionary<T1, T2>>();
				reader.ReadUnmanaged<int>(out num);
				goto IL_00D1;
			}
			dictionary = value.dic;
			dictionary2 = value.Dict;
			num = value.Count;
			reader.ReadValue<Dictionary<T1, T2>>(ref dictionary);
			reader.ReadValue<Dictionary<T1, T2>>(ref dictionary2);
			reader.ReadUnmanaged<int>(out num);
		}
		else
		{
			if (b > 3)
			{
				MemoryPackSerializationException.ThrowInvalidPropertyCount(typeof(DictionarySave<T1, T2>), 3, b);
				return;
			}
			Dictionary<T1, T2> dictionary2;
			if (value == null)
			{
				dictionary = null;
				dictionary2 = null;
				int num = 0;
			}
			else
			{
				dictionary = value.dic;
				dictionary2 = value.Dict;
				int num = value.Count;
			}
			if (b != 0)
			{
				reader.ReadValue<Dictionary<T1, T2>>(ref dictionary);
				if (b != 1)
				{
					reader.ReadValue<Dictionary<T1, T2>>(ref dictionary2);
					if (b != 2)
					{
						int num;
						reader.ReadUnmanaged<int>(out num);
					}
				}
			}
			if (value == null)
			{
				goto IL_00D1;
			}
		}
		value.dic = dictionary;
		return;
		IL_00D1:
		value = new DictionarySave<T1, T2>
		{
			dic = dictionary
		};
	}

	// Token: 0x040079AF RID: 31151
	[MemoryPackInclude]
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<T1, T2> dic = new Dictionary<T1, T2>();

	// Token: 0x040079B0 RID: 31152
	[PublicizedFrom(EAccessModifier.Private)]
	public List<T1> toRemove = new List<T1>();

	// Token: 0x040079B1 RID: 31153
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly bool KeyIsValuetype = typeof(T1).IsValueType;

	// Token: 0x02001417 RID: 5143
	// (Invoke) Token: 0x0600A0E2 RID: 41186
	public delegate void DictionaryRemoveCallback(T1 _o);

	// Token: 0x02001418 RID: 5144
	[NullableContext(1)]
	[Nullable(new byte[] { 0, 1, 1, 1 })]
	[Preserve]
	[PublicizedFrom(EAccessModifier.Private)]
	public sealed class DictionarySaveFormatter : MemoryPackFormatter<DictionarySave<T1, T2>>
	{
		// Token: 0x0600A0E5 RID: 41189 RVA: 0x003C932C File Offset: 0x003C752C
		[Preserve]
		public override void Serialize(ref MemoryPackWriter writer, ref DictionarySave<T1, T2> value)
		{
			DictionarySave<T1, T2>.Serialize(ref writer, ref value);
		}

		// Token: 0x0600A0E6 RID: 41190 RVA: 0x003C9335 File Offset: 0x003C7535
		[Preserve]
		public override void Deserialize(ref MemoryPackReader reader, ref DictionarySave<T1, T2> value)
		{
			DictionarySave<T1, T2>.Deserialize(ref reader, ref value);
		}
	}
}
