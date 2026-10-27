using System;
using System.Collections.Generic;
using MemoryPack;

// Token: 0x0200141D RID: 5149
public class DataItem<T> : IDataItem
{
	// Token: 0x14000101 RID: 257
	// (add) Token: 0x0600A0EF RID: 41199 RVA: 0x003C93F0 File Offset: 0x003C75F0
	// (remove) Token: 0x0600A0F0 RID: 41200 RVA: 0x003C9428 File Offset: 0x003C7628
	public event DataItem<T>.OnChangeDelegate OnChangeDelegates;

	// Token: 0x17001332 RID: 4914
	// (get) Token: 0x0600A0F1 RID: 41201 RVA: 0x003C945D File Offset: 0x003C765D
	public string Name
	{
		get
		{
			return this.name;
		}
	}

	// Token: 0x0600A0F2 RID: 41202 RVA: 0x003C9468 File Offset: 0x003C7668
	[MemoryPackConstructor]
	public DataItem()
		: this(default(T))
	{
	}

	// Token: 0x0600A0F3 RID: 41203 RVA: 0x003C9484 File Offset: 0x003C7684
	public DataItem(T _startValue)
		: this(null, _startValue)
	{
	}

	// Token: 0x0600A0F4 RID: 41204 RVA: 0x003C948E File Offset: 0x003C768E
	public DataItem(string _name, T _startValue)
	{
		this.name = _name;
		this.internalValue = _startValue;
	}

	// Token: 0x17001333 RID: 4915
	// (get) Token: 0x0600A0F6 RID: 41206 RVA: 0x003C94D4 File Offset: 0x003C76D4
	// (set) Token: 0x0600A0F5 RID: 41205 RVA: 0x003C94A4 File Offset: 0x003C76A4
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

	// Token: 0x0600A0F7 RID: 41207 RVA: 0x003C94DC File Offset: 0x003C76DC
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

	// Token: 0x0600A0F8 RID: 41208 RVA: 0x003C952C File Offset: 0x003C772C
	public static bool operator ==(DataItem<T> v1, T v2)
	{
		if (v1 != null)
		{
			return EqualityComparer<T>.Default.Equals(v1.internalValue, v2);
		}
		return v2 == null;
	}

	// Token: 0x0600A0F9 RID: 41209 RVA: 0x003C954C File Offset: 0x003C774C
	public static bool operator !=(DataItem<T> v1, T v2)
	{
		if (v1 != null)
		{
			return !EqualityComparer<T>.Default.Equals(v1.internalValue, v2);
		}
		return v2 != null;
	}

	// Token: 0x0600A0FA RID: 41210 RVA: 0x003C956F File Offset: 0x003C776F
	public override bool Equals(object obj)
	{
		return obj != null && this.internalValue.Equals(obj);
	}

	// Token: 0x0600A0FB RID: 41211 RVA: 0x003C9588 File Offset: 0x003C7788
	public override int GetHashCode()
	{
		return this.internalValue.GetHashCode();
	}

	// Token: 0x040079B3 RID: 31155
	[PublicizedFrom(EAccessModifier.Private)]
	public string name;

	// Token: 0x040079B4 RID: 31156
	[PublicizedFrom(EAccessModifier.Private)]
	public T internalValue;

	// Token: 0x040079B5 RID: 31157
	public IDataItemFormatter Formatter;

	// Token: 0x0200141E RID: 5150
	// (Invoke) Token: 0x0600A0FD RID: 41213
	public delegate void OnChangeDelegate(T _oldValue, T _newValue);
}
