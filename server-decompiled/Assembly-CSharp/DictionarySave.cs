using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MemoryPack;
using MemoryPack.Formatters;
using MemoryPack.Internal;

// Token: 0x020013C9 RID: 5065
[MemoryPackable(GenerateType.Object)]
public class DictionarySave<T1, T2> : IMemoryPackable<DictionarySave<T1, T2>>, IMemoryPackFormatterRegister where T2 : class
{
	// Token: 0x06009ED1 RID: 40657 RVA: 0x003BCD80 File Offset: 0x003BAF80
	[MemoryPackConstructor]
	public DictionarySave()
	{
	}

	// Token: 0x170012A4 RID: 4772
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

	// Token: 0x170012A5 RID: 4773
	// (get) Token: 0x06009ED4 RID: 40660 RVA: 0x003BCDF0 File Offset: 0x003BAFF0
	public Dictionary<T1, T2> Dict
	{
		get
		{
			return this.dic;
		}
	}

	// Token: 0x06009ED5 RID: 40661 RVA: 0x003BCDF8 File Offset: 0x003BAFF8
	public bool ContainsKey(T1 _key)
	{
		return this.dic.ContainsKey(_key);
	}

	// Token: 0x06009ED6 RID: 40662 RVA: 0x003BCE06 File Offset: 0x003BB006
	public bool TryGetValue(T1 _key, out T2 _value)
	{
		return this.dic.TryGetValue(_key, out _value);
	}

	// Token: 0x06009ED7 RID: 40663 RVA: 0x003BCE15 File Offset: 0x003BB015
	public void Add(T1 _key, T2 _value)
	{
		this.dic.Add(_key, _value);
	}

	// Token: 0x06009ED8 RID: 40664 RVA: 0x003BCE24 File Offset: 0x003BB024
	public void Remove(T1 _key)
	{
		this.dic.Remove(_key);
	}

	// Token: 0x06009ED9 RID: 40665 RVA: 0x003BCE33 File Offset: 0x003BB033
	public void Clear()
	{
		this.dic.Clear();
	}

	// Token: 0x06009EDA RID: 40666 RVA: 0x003BCE40 File Offset: 0x003BB040
	public void MarkToRemove(T1 _v)
	{
		this.toRemove.Add(_v);
	}

	// Token: 0x06009EDB RID: 40667 RVA: 0x003BCE50 File Offset: 0x003BB050
	public void RemoveAllMarked(DictionarySave<T1, T2>.DictionaryRemoveCallback _callback)
	{
		foreach (T1 t in this.toRemove)
		{
			_callback(t);
		}
		this.toRemove.Clear();
	}

	// Token: 0x170012A6 RID: 4774
	// (get) Token: 0x06009EDC RID: 40668 RVA: 0x003BCEB0 File Offset: 0x003BB0B0
	public int Count
	{
		get
		{
			return this.dic.Count;
		}
	}

	// Token: 0x06009EDD RID: 40669 RVA: 0x003BCEBD File Offset: 0x003BB0BD
	[PublicizedFrom(EAccessModifier.Private)]
	static DictionarySave()
	{
		DictionarySave<T1, T2>.RegisterFormatter();
	}

	// Token: 0x06009EDE RID: 40670 RVA: 0x003BCED8 File Offset: 0x003BB0D8
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

	// Token: 0x06009EDF RID: 40671 RVA: 0x003BCF10 File Offset: 0x003BB110
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

	// Token: 0x06009EE0 RID: 40672 RVA: 0x003BCF5C File Offset: 0x003BB15C
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

	// Token: 0x0400781A RID: 30746
	[MemoryPackInclude]
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<T1, T2> dic = new Dictionary<T1, T2>();

	// Token: 0x0400781B RID: 30747
	[PublicizedFrom(EAccessModifier.Private)]
	public List<T1> toRemove = new List<T1>();

	// Token: 0x0400781C RID: 30748
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly bool KeyIsValuetype = typeof(T1).IsValueType;

	// Token: 0x020013CA RID: 5066
	// (Invoke) Token: 0x06009EE2 RID: 40674
	public delegate void DictionaryRemoveCallback(T1 _o);

	// Token: 0x020013CB RID: 5067
	[NullableContext(1)]
	[Nullable(new byte[] { 0, 1, 1, 1 })]
	[Preserve]
	[PublicizedFrom(EAccessModifier.Private)]
	public sealed class DictionarySaveFormatter : MemoryPackFormatter<DictionarySave<T1, T2>>
	{
		// Token: 0x06009EE5 RID: 40677 RVA: 0x003BD048 File Offset: 0x003BB248
		[Preserve]
		public override void Serialize(ref MemoryPackWriter writer, ref DictionarySave<T1, T2> value)
		{
			DictionarySave<T1, T2>.Serialize(ref writer, ref value);
		}

		// Token: 0x06009EE6 RID: 40678 RVA: 0x003BD051 File Offset: 0x003BB251
		[Preserve]
		public override void Deserialize(ref MemoryPackReader reader, ref DictionarySave<T1, T2> value)
		{
			DictionarySave<T1, T2>.Deserialize(ref reader, ref value);
		}
	}
}
