using System;
using System.Collections.Generic;
using MemoryPack;

// Token: 0x020013D0 RID: 5072
public class DataItem<T> : IDataItem
{
	// Token: 0x14000101 RID: 257
	// (add) Token: 0x06009EEF RID: 40687 RVA: 0x003BD10C File Offset: 0x003BB30C
	// (remove) Token: 0x06009EF0 RID: 40688 RVA: 0x003BD144 File Offset: 0x003BB344
	public event DataItem<T>.OnChangeDelegate OnChangeDelegates;

	// Token: 0x170012A8 RID: 4776
	// (get) Token: 0x06009EF1 RID: 40689 RVA: 0x003BD179 File Offset: 0x003BB379
	public string Name
	{
		get
		{
			return this.name;
		}
	}

	// Token: 0x06009EF2 RID: 40690 RVA: 0x003BD184 File Offset: 0x003BB384
	[MemoryPackConstructor]
	public DataItem()
		: this(default(T))
	{
	}

	// Token: 0x06009EF3 RID: 40691 RVA: 0x003BD1A0 File Offset: 0x003BB3A0
	public DataItem(T _startValue)
		: this(null, _startValue)
	{
	}

	// Token: 0x06009EF4 RID: 40692 RVA: 0x003BD1AA File Offset: 0x003BB3AA
	public DataItem(string _name, T _startValue)
	{
		this.name = _name;
		this.internalValue = _startValue;
	}

	// Token: 0x170012A9 RID: 4777
	// (get) Token: 0x06009EF6 RID: 40694 RVA: 0x003BD1F0 File Offset: 0x003BB3F0
	// (set) Token: 0x06009EF5 RID: 40693 RVA: 0x003BD1C0 File Offset: 0x003BB3C0
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

	// Token: 0x06009EF7 RID: 40695 RVA: 0x003BD1F8 File Offset: 0x003BB3F8
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

	// Token: 0x06009EF8 RID: 40696 RVA: 0x003BD248 File Offset: 0x003BB448
	public static bool operator ==(DataItem<T> v1, T v2)
	{
		if (v1 != null)
		{
			return EqualityComparer<T>.Default.Equals(v1.internalValue, v2);
		}
		return v2 == null;
	}

	// Token: 0x06009EF9 RID: 40697 RVA: 0x003BD268 File Offset: 0x003BB468
	public static bool operator !=(DataItem<T> v1, T v2)
	{
		if (v1 != null)
		{
			return !EqualityComparer<T>.Default.Equals(v1.internalValue, v2);
		}
		return v2 != null;
	}

	// Token: 0x06009EFA RID: 40698 RVA: 0x003BD28B File Offset: 0x003BB48B
	public override bool Equals(object obj)
	{
		return obj != null && this.internalValue.Equals(obj);
	}

	// Token: 0x06009EFB RID: 40699 RVA: 0x003BD2A4 File Offset: 0x003BB4A4
	public override int GetHashCode()
	{
		return this.internalValue.GetHashCode();
	}

	// Token: 0x0400781E RID: 30750
	[PublicizedFrom(EAccessModifier.Private)]
	public string name;

	// Token: 0x0400781F RID: 30751
	[PublicizedFrom(EAccessModifier.Private)]
	public T internalValue;

	// Token: 0x04007820 RID: 30752
	public IDataItemFormatter Formatter;

	// Token: 0x020013D1 RID: 5073
	// (Invoke) Token: 0x06009EFD RID: 40701
	public delegate void OnChangeDelegate(T _oldValue, T _newValue);
}
