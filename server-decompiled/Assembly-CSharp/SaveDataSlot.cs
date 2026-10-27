using System;

// Token: 0x02000A2B RID: 2603
public readonly struct SaveDataSlot : IEquatable<SaveDataSlot>, IComparable<SaveDataSlot>, IComparable
{
	// Token: 0x06004D98 RID: 19864 RVA: 0x001DDE84 File Offset: 0x001DC084
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

	// Token: 0x06004D99 RID: 19865 RVA: 0x001DDF4F File Offset: 0x001DC14F
	public SaveDataSlot(SaveDataManagedPath managedPath)
	{
		this.m_internalPath = managedPath;
	}

	// Token: 0x1700086C RID: 2156
	// (get) Token: 0x06004D9A RID: 19866 RVA: 0x001DDF58 File Offset: 0x001DC158
	public SaveDataType Type
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.m_internalPath.Type;
		}
	}

	// Token: 0x1700086D RID: 2157
	// (get) Token: 0x06004D9B RID: 19867 RVA: 0x001DDF65 File Offset: 0x001DC165
	public StringSpan SlotPath
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.m_internalPath.SlotPath;
		}
	}

	// Token: 0x06004D9C RID: 19868 RVA: 0x001DDF72 File Offset: 0x001DC172
	public SaveDataSlot GetSimpleSlot()
	{
		if (!(this.m_internalPath.PathRelativeToSlot == "d"))
		{
			return new SaveDataSlot(this.Type, this.SlotPath);
		}
		return this;
	}

	// Token: 0x06004D9D RID: 19869 RVA: 0x001DDFA8 File Offset: 0x001DC1A8
	public override string ToString()
	{
		if (this.SlotPath.Length != 0)
		{
			return SpanUtils.Concat(this.Type.ToStringCached<SaveDataType>(), "[", this.SlotPath, "]");
		}
		return this.Type.ToStringCached<SaveDataType>();
	}

	// Token: 0x06004D9E RID: 19870 RVA: 0x001DE000 File Offset: 0x001DC200
	public bool Equals(SaveDataSlot other)
	{
		if (other.m_internalPath == null)
		{
			return this.m_internalPath == null;
		}
		return this.Type == other.Type && this.SlotPath == other.SlotPath;
	}

	// Token: 0x06004D9F RID: 19871 RVA: 0x001DE040 File Offset: 0x001DC240
	public override bool Equals(object obj)
	{
		if (obj is SaveDataSlot)
		{
			SaveDataSlot saveDataSlot = (SaveDataSlot)obj;
			return this.Equals(saveDataSlot);
		}
		return false;
	}

	// Token: 0x06004DA0 RID: 19872 RVA: 0x001DE068 File Offset: 0x001DC268
	public override int GetHashCode()
	{
		return (int)((this.Type * (SaveDataType)397) ^ (SaveDataType)this.SlotPath.GetHashCode());
	}

	// Token: 0x06004DA1 RID: 19873 RVA: 0x001DE096 File Offset: 0x001DC296
	public static bool operator ==(SaveDataSlot left, SaveDataSlot right)
	{
		return left.Equals(right);
	}

	// Token: 0x06004DA2 RID: 19874 RVA: 0x001DE0A0 File Offset: 0x001DC2A0
	public static bool operator !=(SaveDataSlot left, SaveDataSlot right)
	{
		return !left.Equals(right);
	}

	// Token: 0x06004DA3 RID: 19875 RVA: 0x001DE0B0 File Offset: 0x001DC2B0
	public int CompareTo(SaveDataSlot other)
	{
		int num = this.Type.CompareTo(other.Type);
		if (num != 0)
		{
			return num;
		}
		return this.SlotPath.CompareTo(other.SlotPath, StringComparison.Ordinal);
	}

	// Token: 0x06004DA4 RID: 19876 RVA: 0x001DE0FC File Offset: 0x001DC2FC
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

	// Token: 0x06004DA5 RID: 19877 RVA: 0x001DE131 File Offset: 0x001DC331
	public static bool operator <(SaveDataSlot left, SaveDataSlot right)
	{
		return left.CompareTo(right) < 0;
	}

	// Token: 0x06004DA6 RID: 19878 RVA: 0x001DE13E File Offset: 0x001DC33E
	public static bool operator >(SaveDataSlot left, SaveDataSlot right)
	{
		return left.CompareTo(right) > 0;
	}

	// Token: 0x06004DA7 RID: 19879 RVA: 0x001DE14B File Offset: 0x001DC34B
	public static bool operator <=(SaveDataSlot left, SaveDataSlot right)
	{
		return left.CompareTo(right) <= 0;
	}

	// Token: 0x06004DA8 RID: 19880 RVA: 0x001DE15B File Offset: 0x001DC35B
	public static bool operator >=(SaveDataSlot left, SaveDataSlot right)
	{
		return left.CompareTo(right) >= 0;
	}

	// Token: 0x04003C58 RID: 15448
	[PublicizedFrom(EAccessModifier.Private)]
	public const string DUMMY_POSTFIX_WITHOUT_SLASH = "d";

	// Token: 0x04003C59 RID: 15449
	[PublicizedFrom(EAccessModifier.Private)]
	public const string DUMMY_POSTFIX_WITH_SLASH = "/d";

	// Token: 0x04003C5A RID: 15450
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly SaveDataManagedPath m_internalPath;
}
