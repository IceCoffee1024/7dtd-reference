using System;

// Token: 0x02001454 RID: 5204
[PublicizedFrom(EAccessModifier.Internal)]
public class IntHashMapEntry
{
	// Token: 0x0600A305 RID: 41733 RVA: 0x003D2D9E File Offset: 0x003D0F9E
	public IntHashMapEntry(int _i1, int _i2, object _obj, IntHashMapEntry _entry)
	{
		this.valueEntry = _obj;
		this.nextEntry = _entry;
		this.hashEntry = _i2;
		this.slotHash = _i1;
	}

	// Token: 0x0600A306 RID: 41734 RVA: 0x003D2DC3 File Offset: 0x003D0FC3
	public int getHash()
	{
		return this.hashEntry;
	}

	// Token: 0x0600A307 RID: 41735 RVA: 0x003D2DCB File Offset: 0x003D0FCB
	public object getValue()
	{
		return this.valueEntry;
	}

	// Token: 0x0600A308 RID: 41736 RVA: 0x003D2DD4 File Offset: 0x003D0FD4
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

	// Token: 0x0600A309 RID: 41737 RVA: 0x003D2E23 File Offset: 0x003D1023
	public int hashCode()
	{
		return IntHashMap.getHash(this.hashEntry);
	}

	// Token: 0x0600A30A RID: 41738 RVA: 0x003D2E30 File Offset: 0x003D1030
	public string toString()
	{
		string text = this.getHash().ToString();
		string text2 = "=";
		object value = this.getValue();
		return text + text2 + ((value != null) ? value.ToString() : null);
	}

	// Token: 0x04007A71 RID: 31345
	public int hashEntry;

	// Token: 0x04007A72 RID: 31346
	public object valueEntry;

	// Token: 0x04007A73 RID: 31347
	public IntHashMapEntry nextEntry;

	// Token: 0x04007A74 RID: 31348
	public int slotHash;
}
