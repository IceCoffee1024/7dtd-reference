using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MemoryPack;
using MemoryPack.Formatters;
using MemoryPack.Internal;

// Token: 0x02001404 RID: 5124
[MemoryPackable(GenerateType.Object)]
public class DictionarySave<T1, T2> : IMemoryPackable<DictionarySave<T1, T2>>, IMemoryPackFormatterRegister where T2 : class
{
	// Token: 0x0600A079 RID: 41081 RVA: 0x003C6C60 File Offset: 0x003C4E60
	[MemoryPackConstructor]
	public DictionarySave()
	{
	}

	// Token: 0x17001322 RID: 4898
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

	// Token: 0x17001323 RID: 4899
	// (get) Token: 0x0600A07C RID: 41084 RVA: 0x003C6CD0 File Offset: 0x003C4ED0
	public Dictionary<T1, T2> Dict
	{
		get
		{
			return this.dic;
		}
	}

	// Token: 0x0600A07D RID: 41085 RVA: 0x003C6CD8 File Offset: 0x003C4ED8
	public bool ContainsKey(T1 _key)
	{
		return this.dic.ContainsKey(_key);
	}

	// Token: 0x0600A07E RID: 41086 RVA: 0x003C6CE6 File Offset: 0x003C4EE6
	public bool TryGetValue(T1 _key, out T2 _value)
	{
		return this.dic.TryGetValue(_key, out _value);
	}

	// Token: 0x0600A07F RID: 41087 RVA: 0x003C6CF5 File Offset: 0x003C4EF5
	public void Add(T1 _key, T2 _value)
	{
		this.dic.Add(_key, _value);
	}

	// Token: 0x0600A080 RID: 41088 RVA: 0x003C6D04 File Offset: 0x003C4F04
	public void Remove(T1 _key)
	{
		this.dic.Remove(_key);
	}

	// Token: 0x0600A081 RID: 41089 RVA: 0x003C6D13 File Offset: 0x003C4F13
	public void Clear()
	{
		this.dic.Clear();
	}

	// Token: 0x0600A082 RID: 41090 RVA: 0x003C6D20 File Offset: 0x003C4F20
	public void MarkToRemove(T1 _v)
	{
		this.toRemove.Add(_v);
	}

	// Token: 0x0600A083 RID: 41091 RVA: 0x003C6D30 File Offset: 0x003C4F30
	public void RemoveAllMarked(DictionarySave<T1, T2>.DictionaryRemoveCallback _callback)
	{
		foreach (T1 t in this.toRemove)
		{
			_callback(t);
		}
		this.toRemove.Clear();
	}

	// Token: 0x17001324 RID: 4900
	// (get) Token: 0x0600A084 RID: 41092 RVA: 0x003C6D90 File Offset: 0x003C4F90
	public int Count
	{
		get
		{
			return this.dic.Count;
		}
	}

	// Token: 0x0600A085 RID: 41093 RVA: 0x003C6D9D File Offset: 0x003C4F9D
	[PublicizedFrom(EAccessModifier.Private)]
	static DictionarySave()
	{
		DictionarySave<T1, T2>.RegisterFormatter();
	}

	// Token: 0x0600A086 RID: 41094 RVA: 0x003C6DB8 File Offset: 0x003C4FB8
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

	// Token: 0x0600A087 RID: 41095 RVA: 0x003C6DF0 File Offset: 0x003C4FF0
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

	// Token: 0x0600A088 RID: 41096 RVA: 0x003C6E3C File Offset: 0x003C503C
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

	// Token: 0x0400796E RID: 31086
	[MemoryPackInclude]
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<T1, T2> dic = new Dictionary<T1, T2>();

	// Token: 0x0400796F RID: 31087
	[PublicizedFrom(EAccessModifier.Private)]
	public List<T1> toRemove = new List<T1>();

	// Token: 0x04007970 RID: 31088
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly bool KeyIsValuetype = typeof(T1).IsValueType;

	// Token: 0x02001405 RID: 5125
	// (Invoke) Token: 0x0600A08A RID: 41098
	public delegate void DictionaryRemoveCallback(T1 _o);

	// Token: 0x02001406 RID: 5126
	[NullableContext(1)]
	[Nullable(new byte[] { 0, 1, 1, 1 })]
	[Preserve]
	[PublicizedFrom(EAccessModifier.Private)]
	public sealed class DictionarySaveFormatter : MemoryPackFormatter<DictionarySave<T1, T2>>
	{
		// Token: 0x0600A08D RID: 41101 RVA: 0x003C6F28 File Offset: 0x003C5128
		[Preserve]
		public override void Serialize(ref MemoryPackWriter writer, ref DictionarySave<T1, T2> value)
		{
			DictionarySave<T1, T2>.Serialize(ref writer, ref value);
		}

		// Token: 0x0600A08E RID: 41102 RVA: 0x003C6F31 File Offset: 0x003C5131
		[Preserve]
		public override void Deserialize(ref MemoryPackReader reader, ref DictionarySave<T1, T2> value)
		{
			DictionarySave<T1, T2>.Deserialize(ref reader, ref value);
		}
	}
}
