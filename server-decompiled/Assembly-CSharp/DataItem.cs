using System;
using System.Collections.Generic;
using MemoryPack;

// Token: 0x0200140B RID: 5131
public class DataItem<T> : IDataItem
{
	// Token: 0x14000101 RID: 257
	// (add) Token: 0x0600A097 RID: 41111 RVA: 0x003C6FEC File Offset: 0x003C51EC
	// (remove) Token: 0x0600A098 RID: 41112 RVA: 0x003C7024 File Offset: 0x003C5224
	public event DataItem<T>.OnChangeDelegate OnChangeDelegates;

	// Token: 0x17001326 RID: 4902
	// (get) Token: 0x0600A099 RID: 41113 RVA: 0x003C7059 File Offset: 0x003C5259
	public string Name
	{
		get
		{
			return this.name;
		}
	}

	// Token: 0x0600A09A RID: 41114 RVA: 0x003C7064 File Offset: 0x003C5264
	[MemoryPackConstructor]
	public DataItem()
		: this(default(T))
	{
	}

	// Token: 0x0600A09B RID: 41115 RVA: 0x003C7080 File Offset: 0x003C5280
	public DataItem(T _startValue)
		: this(null, _startValue)
	{
	}

	// Token: 0x0600A09C RID: 41116 RVA: 0x003C708A File Offset: 0x003C528A
	public DataItem(string _name, T _startValue)
	{
		this.name = _name;
		this.internalValue = _startValue;
	}

	// Token: 0x17001327 RID: 4903
	// (get) Token: 0x0600A09E RID: 41118 RVA: 0x003C70D0 File Offset: 0x003C52D0
	// (set) Token: 0x0600A09D RID: 41117 RVA: 0x003C70A0 File Offset: 0x003C52A0
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

	// Token: 0x0600A09F RID: 41119 RVA: 0x003C70D8 File Offset: 0x003C52D8
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

	// Token: 0x0600A0A0 RID: 41120 RVA: 0x003C7128 File Offset: 0x003C5328
	public static bool operator ==(DataItem<T> v1, T v2)
	{
		if (v1 != null)
		{
			return EqualityComparer<T>.Default.Equals(v1.internalValue, v2);
		}
		return v2 == null;
	}

	// Token: 0x0600A0A1 RID: 41121 RVA: 0x003C7148 File Offset: 0x003C5348
	public static bool operator !=(DataItem<T> v1, T v2)
	{
		if (v1 != null)
		{
			return !EqualityComparer<T>.Default.Equals(v1.internalValue, v2);
		}
		return v2 != null;
	}

	// Token: 0x0600A0A2 RID: 41122 RVA: 0x003C716B File Offset: 0x003C536B
	public override bool Equals(object obj)
	{
		return obj != null && this.internalValue.Equals(obj);
	}

	// Token: 0x0600A0A3 RID: 41123 RVA: 0x003C7184 File Offset: 0x003C5384
	public override int GetHashCode()
	{
		return this.internalValue.GetHashCode();
	}

	// Token: 0x04007972 RID: 31090
	[PublicizedFrom(EAccessModifier.Private)]
	public string name;

	// Token: 0x04007973 RID: 31091
	[PublicizedFrom(EAccessModifier.Private)]
	public T internalValue;

	// Token: 0x04007974 RID: 31092
	public IDataItemFormatter Formatter;

	// Token: 0x0200140C RID: 5132
	// (Invoke) Token: 0x0600A0A5 RID: 41125
	public delegate void OnChangeDelegate(T _oldValue, T _newValue);
}
