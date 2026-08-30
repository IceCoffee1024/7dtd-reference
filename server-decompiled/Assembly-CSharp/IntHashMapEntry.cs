using System;

// Token: 0x02001407 RID: 5127
[PublicizedFrom(EAccessModifier.Internal)]
public class IntHashMapEntry
{
	// Token: 0x0600A105 RID: 41221 RVA: 0x003C6ABA File Offset: 0x003C4CBA
	public IntHashMapEntry(int _i1, int _i2, object _obj, IntHashMapEntry _entry)
	{
		this.valueEntry = _obj;
		this.nextEntry = _entry;
		this.hashEntry = _i2;
		this.slotHash = _i1;
	}

	// Token: 0x0600A106 RID: 41222 RVA: 0x003C6ADF File Offset: 0x003C4CDF
	public int getHash()
	{
		return this.hashEntry;
	}

	// Token: 0x0600A107 RID: 41223 RVA: 0x003C6AE7 File Offset: 0x003C4CE7
	public object getValue()
	{
		return this.valueEntry;
	}

	// Token: 0x0600A108 RID: 41224 RVA: 0x003C6AF0 File Offset: 0x003C4CF0
	public bool equals(object _obj)
	{
		if (!(_obj is IntHashMapEntry))
		{
			return false;
		}
		IntHashMapEntry intHashMapEntry = (IntHashMapEntry)_obj;
		int hash = this.getHash();
		int hash2 = intHashMapEntry.getHash();
		if (hash == hash2)
		{
			object value = this.getValue();
			object value2 = intHashMapEntry.getValue();
			if (value == value2 || (value != null && value.Equals(value2)))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600A109 RID: 41225 RVA: 0x003C6B3F File Offset: 0x003C4D3F
	public int hashCode()
	{
		return IntHashMap.getHash(this.hashEntry);
	}

	// Token: 0x0600A10A RID: 41226 RVA: 0x003C6B4C File Offset: 0x003C4D4C
	public string toString()
	{
		string text = this.getHash().ToString();
		string text2 = "=";
		object value = this.getValue();
		return text + text2 + ((value != null) ? value.ToString() : null);
	}

	// Token: 0x040078DC RID: 30940
	public int hashEntry;

	// Token: 0x040078DD RID: 30941
	public object valueEntry;

	// Token: 0x040078DE RID: 30942
	public IntHashMapEntry nextEntry;

	// Token: 0x040078DF RID: 30943
	public int slotHash;
}
