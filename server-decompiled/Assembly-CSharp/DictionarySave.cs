using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MemoryPack;
using MemoryPack.Formatters;
using MemoryPack.Internal;

// Token: 0x020013E4 RID: 5092
[MemoryPackable(GenerateType.Object)]
public class DictionarySave<T1, T2> : IMemoryPackable<DictionarySave<T1, T2>>, IMemoryPackFormatterRegister where T2 : class
{
	// Token: 0x06009FCE RID: 40910 RVA: 0x003C20C4 File Offset: 0x003C02C4
	[MemoryPackConstructor]
	public DictionarySave()
	{
	}

	// Token: 0x170012DD RID: 4829
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

	// Token: 0x170012DE RID: 4830
	// (get) Token: 0x06009FD1 RID: 40913 RVA: 0x003C2134 File Offset: 0x003C0334
	public Dictionary<T1, T2> Dict
	{
		get
		{
			return this.dic;
		}
	}

	// Token: 0x06009FD2 RID: 40914 RVA: 0x003C213C File Offset: 0x003C033C
	public bool ContainsKey(T1 _key)
	{
		return this.dic.ContainsKey(_key);
	}

	// Token: 0x06009FD3 RID: 40915 RVA: 0x003C214A File Offset: 0x003C034A
	public bool TryGetValue(T1 _key, out T2 _value)
	{
		return this.dic.TryGetValue(_key, out _value);
	}

	// Token: 0x06009FD4 RID: 40916 RVA: 0x003C2159 File Offset: 0x003C0359
	public void Add(T1 _key, T2 _value)
	{
		this.dic.Add(_key, _value);
	}

	// Token: 0x06009FD5 RID: 40917 RVA: 0x003C2168 File Offset: 0x003C0368
	public void Remove(T1 _key)
	{
		this.dic.Remove(_key);
	}

	// Token: 0x06009FD6 RID: 40918 RVA: 0x003C2177 File Offset: 0x003C0377
	public void Clear()
	{
		this.dic.Clear();
	}

	// Token: 0x06009FD7 RID: 40919 RVA: 0x003C2184 File Offset: 0x003C0384
	public void MarkToRemove(T1 _v)
	{
		this.toRemove.Add(_v);
	}

	// Token: 0x06009FD8 RID: 40920 RVA: 0x003C2194 File Offset: 0x003C0394
	public void RemoveAllMarked(DictionarySave<T1, T2>.DictionaryRemoveCallback _callback)
	{
		foreach (T1 t in this.toRemove)
		{
			_callback(t);
		}
		this.toRemove.Clear();
	}

	// Token: 0x170012DF RID: 4831
	// (get) Token: 0x06009FD9 RID: 40921 RVA: 0x003C21F4 File Offset: 0x003C03F4
	public int Count
	{
		get
		{
			return this.dic.Count;
		}
	}

	// Token: 0x06009FDA RID: 40922 RVA: 0x003C2201 File Offset: 0x003C0401
	[PublicizedFrom(EAccessModifier.Private)]
	static DictionarySave()
	{
		DictionarySave<T1, T2>.RegisterFormatter();
	}

	// Token: 0x06009FDB RID: 40923 RVA: 0x003C221C File Offset: 0x003C041C
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

	// Token: 0x06009FDC RID: 40924 RVA: 0x003C2254 File Offset: 0x003C0454
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

	// Token: 0x06009FDD RID: 40925 RVA: 0x003C22A0 File Offset: 0x003C04A0
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

	// Token: 0x040078F3 RID: 30963
	[MemoryPackInclude]
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<T1, T2> dic = new Dictionary<T1, T2>();

	// Token: 0x040078F4 RID: 30964
	[PublicizedFrom(EAccessModifier.Private)]
	public List<T1> toRemove = new List<T1>();

	// Token: 0x040078F5 RID: 30965
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly bool KeyIsValuetype = typeof(T1).IsValueType;

	// Token: 0x020013E5 RID: 5093
	// (Invoke) Token: 0x06009FDF RID: 40927
	public delegate void DictionaryRemoveCallback(T1 _o);

	// Token: 0x020013E6 RID: 5094
	[NullableContext(1)]
	[Nullable(new byte[] { 0, 1, 1, 1 })]
	[Preserve]
	[PublicizedFrom(EAccessModifier.Private)]
	public sealed class DictionarySaveFormatter : MemoryPackFormatter<DictionarySave<T1, T2>>
	{
		// Token: 0x06009FE2 RID: 40930 RVA: 0x003C238C File Offset: 0x003C058C
		[Preserve]
		public override void Serialize(ref MemoryPackWriter writer, ref DictionarySave<T1, T2> value)
		{
			DictionarySave<T1, T2>.Serialize(ref writer, ref value);
		}

		// Token: 0x06009FE3 RID: 40931 RVA: 0x003C2395 File Offset: 0x003C0595
		[Preserve]
		public override void Deserialize(ref MemoryPackReader reader, ref DictionarySave<T1, T2> value)
		{
			DictionarySave<T1, T2>.Deserialize(ref reader, ref value);
		}
	}
}
