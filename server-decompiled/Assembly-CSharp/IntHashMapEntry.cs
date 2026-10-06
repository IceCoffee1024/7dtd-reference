using System;

// Token: 0x02001442 RID: 5186
[PublicizedFrom(EAccessModifier.Internal)]
public class IntHashMapEntry
{
	// Token: 0x0600A2AD RID: 41645 RVA: 0x003D099A File Offset: 0x003CEB9A
	public IntHashMapEntry(int _i1, int _i2, object _obj, IntHashMapEntry _entry)
	{
		this.valueEntry = _obj;
		this.nextEntry = _entry;
		this.hashEntry = _i2;
		this.slotHash = _i1;
	}

	// Token: 0x0600A2AE RID: 41646 RVA: 0x003D09BF File Offset: 0x003CEBBF
	public int getHash()
	{
		return this.hashEntry;
	}

	// Token: 0x0600A2AF RID: 41647 RVA: 0x003D09C7 File Offset: 0x003CEBC7
	public object getValue()
	{
		return this.valueEntry;
	}

	// Token: 0x0600A2B0 RID: 41648 RVA: 0x003D09D0 File Offset: 0x003CEBD0
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

	// Token: 0x0600A2B1 RID: 41649 RVA: 0x003D0A1F File Offset: 0x003CEC1F
	public int hashCode()
	{
		return IntHashMap.getHash(this.hashEntry);
	}

	// Token: 0x0600A2B2 RID: 41650 RVA: 0x003D0A2C File Offset: 0x003CEC2C
	public string toString()
	{
		string text = this.getHash().ToString();
		string text2 = "=";
		object value = this.getValue();
		return text + text2 + ((value != null) ? value.ToString() : null);
	}

	// Token: 0x04007A30 RID: 31280
	public int hashEntry;

	// Token: 0x04007A31 RID: 31281
	public object valueEntry;

	// Token: 0x04007A32 RID: 31282
	public IntHashMapEntry nextEntry;

	// Token: 0x04007A33 RID: 31283
	public int slotHash;
}
