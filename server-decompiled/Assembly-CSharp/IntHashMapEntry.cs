using System;

// Token: 0x02001422 RID: 5154
[PublicizedFrom(EAccessModifier.Internal)]
public class IntHashMapEntry
{
	// Token: 0x0600A202 RID: 41474 RVA: 0x003CBDFE File Offset: 0x003C9FFE
	public IntHashMapEntry(int _i1, int _i2, object _obj, IntHashMapEntry _entry)
	{
		this.valueEntry = _obj;
		this.nextEntry = _entry;
		this.hashEntry = _i2;
		this.slotHash = _i1;
	}

	// Token: 0x0600A203 RID: 41475 RVA: 0x003CBE23 File Offset: 0x003CA023
	public int getHash()
	{
		return this.hashEntry;
	}

	// Token: 0x0600A204 RID: 41476 RVA: 0x003CBE2B File Offset: 0x003CA02B
	public object getValue()
	{
		return this.valueEntry;
	}

	// Token: 0x0600A205 RID: 41477 RVA: 0x003CBE34 File Offset: 0x003CA034
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

	// Token: 0x0600A206 RID: 41478 RVA: 0x003CBE83 File Offset: 0x003CA083
	public int hashCode()
	{
		return IntHashMap.getHash(this.hashEntry);
	}

	// Token: 0x0600A207 RID: 41479 RVA: 0x003CBE90 File Offset: 0x003CA090
	public string toString()
	{
		string text = this.getHash().ToString();
		string text2 = "=";
		object value = this.getValue();
		return text + text2 + ((value != null) ? value.ToString() : null);
	}

	// Token: 0x040079B5 RID: 31157
	public int hashEntry;

	// Token: 0x040079B6 RID: 31158
	public object valueEntry;

	// Token: 0x040079B7 RID: 31159
	public IntHashMapEntry nextEntry;

	// Token: 0x040079B8 RID: 31160
	public int slotHash;
}
