using System;
using System.Collections.Generic;
using MemoryPack;

// Token: 0x020013EB RID: 5099
public class DataItem<T> : IDataItem
{
	// Token: 0x14000102 RID: 258
	// (add) Token: 0x06009FEC RID: 40940 RVA: 0x003C2450 File Offset: 0x003C0650
	// (remove) Token: 0x06009FED RID: 40941 RVA: 0x003C2488 File Offset: 0x003C0688
	public event DataItem<T>.OnChangeDelegate OnChangeDelegates;

	// Token: 0x170012E1 RID: 4833
	// (get) Token: 0x06009FEE RID: 40942 RVA: 0x003C24BD File Offset: 0x003C06BD
	public string Name
	{
		get
		{
			return this.name;
		}
	}

	// Token: 0x06009FEF RID: 40943 RVA: 0x003C24C8 File Offset: 0x003C06C8
	[MemoryPackConstructor]
	public DataItem()
		: this(default(T))
	{
	}

	// Token: 0x06009FF0 RID: 40944 RVA: 0x003C24E4 File Offset: 0x003C06E4
	public DataItem(T _startValue)
		: this(null, _startValue)
	{
	}

	// Token: 0x06009FF1 RID: 40945 RVA: 0x003C24EE File Offset: 0x003C06EE
	public DataItem(string _name, T _startValue)
	{
		this.name = _name;
		this.internalValue = _startValue;
	}

	// Token: 0x170012E2 RID: 4834
	// (get) Token: 0x06009FF3 RID: 40947 RVA: 0x003C2534 File Offset: 0x003C0734
	// (set) Token: 0x06009FF2 RID: 40946 RVA: 0x003C2504 File Offset: 0x003C0704
	public T Value
	{
		get
		{
			return this.internalValue;
		}
		set
		{
			T t = this.internalValue;
			this.internalValue = value;
			if (this.OnChangeDelegates != null)
			{
				this.OnChangeDelegates(t, value);
			}
		}
	}

	// Token: 0x06009FF4 RID: 40948 RVA: 0x003C253C File Offset: 0x003C073C
	public override string ToString()
	{
		if (this.Formatter != null)
		{
			return this.Formatter.ToString(this.internalValue);
		}
		if (this.internalValue == null)
		{
			return "null";
		}
		return this.internalValue.ToString();
	}

	// Token: 0x06009FF5 RID: 40949 RVA: 0x003C258C File Offset: 0x003C078C
	public static bool operator ==(DataItem<T> v1, T v2)
	{
		if (v1 != null)
		{
			return EqualityComparer<T>.Default.Equals(v1.internalValue, v2);
		}
		return v2 == null;
	}

	// Token: 0x06009FF6 RID: 40950 RVA: 0x003C25AC File Offset: 0x003C07AC
	public static bool operator !=(DataItem<T> v1, T v2)
	{
		if (v1 != null)
		{
			return !EqualityComparer<T>.Default.Equals(v1.internalValue, v2);
		}
		return v2 != null;
	}

	// Token: 0x06009FF7 RID: 40951 RVA: 0x003C25CF File Offset: 0x003C07CF
	public override bool Equals(object obj)
	{
		return obj != null && this.internalValue.Equals(obj);
	}

	// Token: 0x06009FF8 RID: 40952 RVA: 0x003C25E8 File Offset: 0x003C07E8
	public override int GetHashCode()
	{
		return this.internalValue.GetHashCode();
	}

	// Token: 0x040078F7 RID: 30967
	[PublicizedFrom(EAccessModifier.Private)]
	public string name;

	// Token: 0x040078F8 RID: 30968
	[PublicizedFrom(EAccessModifier.Private)]
	public T internalValue;

	// Token: 0x040078F9 RID: 30969
	public IDataItemFormatter Formatter;

	// Token: 0x020013EC RID: 5100
	// (Invoke) Token: 0x06009FFA RID: 40954
	public delegate void OnChangeDelegate(T _oldValue, T _newValue);
}
