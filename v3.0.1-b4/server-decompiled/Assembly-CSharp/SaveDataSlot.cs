using System;

// Token: 0x02000A06 RID: 2566
public readonly struct SaveDataSlot : IEquatable<SaveDataSlot>, IComparable<SaveDataSlot>, IComparable
{
	// Token: 0x06004D1F RID: 19743 RVA: 0x001D9054 File Offset: 0x001D7254
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

	// Token: 0x06004D20 RID: 19744 RVA: 0x001D911F File Offset: 0x001D731F
	public SaveDataSlot(SaveDataManagedPath managedPath)
	{
		this.m_internalPath = managedPath;
	}

	// Token: 0x17000834 RID: 2100
	// (get) Token: 0x06004D21 RID: 19745 RVA: 0x001D9128 File Offset: 0x001D7328
	public SaveDataType Type
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.m_internalPath.Type;
		}
	}

	// Token: 0x17000835 RID: 2101
	// (get) Token: 0x06004D22 RID: 19746 RVA: 0x001D9135 File Offset: 0x001D7335
	public StringSpan SlotPath
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.m_internalPath.SlotPath;
		}
	}

	// Token: 0x06004D23 RID: 19747 RVA: 0x001D9142 File Offset: 0x001D7342
	public SaveDataSlot GetSimpleSlot()
	{
		if (!(this.m_internalPath.PathRelativeToSlot == "d"))
		{
			return new SaveDataSlot(this.Type, this.SlotPath);
		}
		return this;
	}

	// Token: 0x06004D24 RID: 19748 RVA: 0x001D9178 File Offset: 0x001D7378
	public override string ToString()
	{
		if (this.SlotPath.Length != 0)
		{
			return SpanUtils.Concat(this.Type.ToStringCached<SaveDataType>(), "[", this.SlotPath, "]");
		}
		return this.Type.ToStringCached<SaveDataType>();
	}

	// Token: 0x06004D25 RID: 19749 RVA: 0x001D91D0 File Offset: 0x001D73D0
	public bool Equals(SaveDataSlot other)
	{
		if (other.m_internalPath == null)
		{
			return this.m_internalPath == null;
		}
		return this.Type == other.Type && this.SlotPath == other.SlotPath;
	}

	// Token: 0x06004D26 RID: 19750 RVA: 0x001D9210 File Offset: 0x001D7410
	public override bool Equals(object obj)
	{
		if (obj is SaveDataSlot)
		{
			SaveDataSlot saveDataSlot = (SaveDataSlot)obj;
			return this.Equals(saveDataSlot);
		}
		return false;
	}

	// Token: 0x06004D27 RID: 19751 RVA: 0x001D9238 File Offset: 0x001D7438
	public override int GetHashCode()
	{
		return (int)((this.Type * (SaveDataType)397) ^ (SaveDataType)this.SlotPath.GetHashCode());
	}

	// Token: 0x06004D28 RID: 19752 RVA: 0x001D9266 File Offset: 0x001D7466
	public static bool operator ==(SaveDataSlot left, SaveDataSlot right)
	{
		return left.Equals(right);
	}

	// Token: 0x06004D29 RID: 19753 RVA: 0x001D9270 File Offset: 0x001D7470
	public static bool operator !=(SaveDataSlot left, SaveDataSlot right)
	{
		return !left.Equals(right);
	}

	// Token: 0x06004D2A RID: 19754 RVA: 0x001D9280 File Offset: 0x001D7480
	public int CompareTo(SaveDataSlot other)
	{
		int num = this.Type.CompareTo(other.Type);
		if (num != 0)
		{
			return num;
		}
		return this.SlotPath.CompareTo(other.SlotPath, StringComparison.Ordinal);
	}

	// Token: 0x06004D2B RID: 19755 RVA: 0x001D92CC File Offset: 0x001D74CC
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

	// Token: 0x06004D2C RID: 19756 RVA: 0x001D9301 File Offset: 0x001D7501
	public static bool operator <(SaveDataSlot left, SaveDataSlot right)
	{
		return left.CompareTo(right) < 0;
	}

	// Token: 0x06004D2D RID: 19757 RVA: 0x001D930E File Offset: 0x001D750E
	public static bool operator >(SaveDataSlot left, SaveDataSlot right)
	{
		return left.CompareTo(right) > 0;
	}

	// Token: 0x06004D2E RID: 19758 RVA: 0x001D931B File Offset: 0x001D751B
	public static bool operator <=(SaveDataSlot left, SaveDataSlot right)
	{
		return left.CompareTo(right) <= 0;
	}

	// Token: 0x06004D2F RID: 19759 RVA: 0x001D932B File Offset: 0x001D752B
	public static bool operator >=(SaveDataSlot left, SaveDataSlot right)
	{
		return left.CompareTo(right) >= 0;
	}

	// Token: 0x04003B99 RID: 15257
	[PublicizedFrom(EAccessModifier.Private)]
	public const string DUMMY_POSTFIX_WITHOUT_SLASH = "d";

	// Token: 0x04003B9A RID: 15258
	[PublicizedFrom(EAccessModifier.Private)]
	public const string DUMMY_POSTFIX_WITH_SLASH = "/d";

	// Token: 0x04003B9B RID: 15259
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly SaveDataManagedPath m_internalPath;
}
