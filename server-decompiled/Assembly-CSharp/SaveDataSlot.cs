using System;

// Token: 0x02000A21 RID: 2593
public readonly struct SaveDataSlot : IEquatable<SaveDataSlot>, IComparable<SaveDataSlot>, IComparable
{
	// Token: 0x06004D74 RID: 19828 RVA: 0x001DCD9C File Offset: 0x001DAF9C
	[PublicizedFrom(EAccessModifier.Private)]
	public SaveDataSlot(SaveDataType type, StringSpan slotPath)
	{
		this = new SaveDataSlot(new SaveDataManagedPath((slotPath.Length > 0) ? SpanUtils.Concat(type.GetPathRaw(), "/", slotPath, "/d") : SpanUtils.Concat(type.GetPathRaw(), "/d")));
		if (this.Type != type)
		{
			throw new ArgumentException(string.Format("Got type {0} but expected {1}. Make sure that concatenating the slot path does not match another type.", this.Type, type), "type");
		}
		if (this.SlotPath != slotPath)
		{
			throw new ArgumentException(SpanUtils.Concat("Expected slot path to be '", slotPath, "', but was: ", this.SlotPath), "slotPath");
		}
	}

	// Token: 0x06004D75 RID: 19829 RVA: 0x001DCE67 File Offset: 0x001DB067
	public SaveDataSlot(SaveDataManagedPath managedPath)
	{
		this.m_internalPath = managedPath;
	}

	// Token: 0x17000866 RID: 2150
	// (get) Token: 0x06004D76 RID: 19830 RVA: 0x001DCE70 File Offset: 0x001DB070
	public SaveDataType Type
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.m_internalPath.Type;
		}
	}

	// Token: 0x17000867 RID: 2151
	// (get) Token: 0x06004D77 RID: 19831 RVA: 0x001DCE7D File Offset: 0x001DB07D
	public StringSpan SlotPath
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.m_internalPath.SlotPath;
		}
	}

	// Token: 0x06004D78 RID: 19832 RVA: 0x001DCE8A File Offset: 0x001DB08A
	public SaveDataSlot GetSimpleSlot()
	{
		if (!(this.m_internalPath.PathRelativeToSlot == "d"))
		{
			return new SaveDataSlot(this.Type, this.SlotPath);
		}
		return this;
	}

	// Token: 0x06004D79 RID: 19833 RVA: 0x001DCEC0 File Offset: 0x001DB0C0
	public override string ToString()
	{
		if (this.SlotPath.Length != 0)
		{
			return SpanUtils.Concat(this.Type.ToStringCached<SaveDataType>(), "[", this.SlotPath, "]");
		}
		return this.Type.ToStringCached<SaveDataType>();
	}

	// Token: 0x06004D7A RID: 19834 RVA: 0x001DCF18 File Offset: 0x001DB118
	public bool Equals(SaveDataSlot other)
	{
		if (other.m_internalPath == null)
		{
			return this.m_internalPath == null;
		}
		return this.Type == other.Type && this.SlotPath == other.SlotPath;
	}

	// Token: 0x06004D7B RID: 19835 RVA: 0x001DCF58 File Offset: 0x001DB158
	public override bool Equals(object obj)
	{
		if (obj is SaveDataSlot)
		{
			SaveDataSlot saveDataSlot = (SaveDataSlot)obj;
			return this.Equals(saveDataSlot);
		}
		return false;
	}

	// Token: 0x06004D7C RID: 19836 RVA: 0x001DCF80 File Offset: 0x001DB180
	public override int GetHashCode()
	{
		return (int)((this.Type * (SaveDataType)397) ^ (SaveDataType)this.SlotPath.GetHashCode());
	}

	// Token: 0x06004D7D RID: 19837 RVA: 0x001DCFAE File Offset: 0x001DB1AE
	public static bool operator ==(SaveDataSlot left, SaveDataSlot right)
	{
		return left.Equals(right);
	}

	// Token: 0x06004D7E RID: 19838 RVA: 0x001DCFB8 File Offset: 0x001DB1B8
	public static bool operator !=(SaveDataSlot left, SaveDataSlot right)
	{
		return !left.Equals(right);
	}

	// Token: 0x06004D7F RID: 19839 RVA: 0x001DCFC8 File Offset: 0x001DB1C8
	public int CompareTo(SaveDataSlot other)
	{
		int num = this.Type.CompareTo(other.Type);
		if (num != 0)
		{
			return num;
		}
		return this.SlotPath.CompareTo(other.SlotPath, StringComparison.Ordinal);
	}

	// Token: 0x06004D80 RID: 19840 RVA: 0x001DD014 File Offset: 0x001DB214
	public int CompareTo(object obj)
	{
		if (obj == null)
		{
			return 1;
		}
		if (obj is SaveDataSlot)
		{
			SaveDataSlot saveDataSlot = (SaveDataSlot)obj;
			return this.CompareTo(saveDataSlot);
		}
		throw new ArgumentException("Object must be of type SaveDataSlot");
	}

	// Token: 0x06004D81 RID: 19841 RVA: 0x001DD049 File Offset: 0x001DB249
	public static bool operator <(SaveDataSlot left, SaveDataSlot right)
	{
		return left.CompareTo(right) < 0;
	}

	// Token: 0x06004D82 RID: 19842 RVA: 0x001DD056 File Offset: 0x001DB256
	public static bool operator >(SaveDataSlot left, SaveDataSlot right)
	{
		return left.CompareTo(right) > 0;
	}

	// Token: 0x06004D83 RID: 19843 RVA: 0x001DD063 File Offset: 0x001DB263
	public static bool operator <=(SaveDataSlot left, SaveDataSlot right)
	{
		return left.CompareTo(right) <= 0;
	}

	// Token: 0x06004D84 RID: 19844 RVA: 0x001DD073 File Offset: 0x001DB273
	public static bool operator >=(SaveDataSlot left, SaveDataSlot right)
	{
		return left.CompareTo(right) >= 0;
	}

	// Token: 0x04003C37 RID: 15415
	[PublicizedFrom(EAccessModifier.Private)]
	public const string DUMMY_POSTFIX_WITHOUT_SLASH = "d";

	// Token: 0x04003C38 RID: 15416
	[PublicizedFrom(EAccessModifier.Private)]
	public const string DUMMY_POSTFIX_WITH_SLASH = "/d";

	// Token: 0x04003C39 RID: 15417
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly SaveDataManagedPath m_internalPath;
}
