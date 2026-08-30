using System;

// Token: 0x02000A17 RID: 2583
public readonly struct SaveDataSlot : IEquatable<SaveDataSlot>, IComparable<SaveDataSlot>, IComparable
{
	// Token: 0x06004DA5 RID: 19877 RVA: 0x001DBAEC File Offset: 0x001D9CEC
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

	// Token: 0x06004DA6 RID: 19878 RVA: 0x001DBBB7 File Offset: 0x001D9DB7
	public SaveDataSlot(SaveDataManagedPath managedPath)
	{
		this.m_internalPath = managedPath;
	}

	// Token: 0x17000846 RID: 2118
	// (get) Token: 0x06004DA7 RID: 19879 RVA: 0x001DBBC0 File Offset: 0x001D9DC0
	public SaveDataType Type
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.m_internalPath.Type;
		}
	}

	// Token: 0x17000847 RID: 2119
	// (get) Token: 0x06004DA8 RID: 19880 RVA: 0x001DBBCD File Offset: 0x001D9DCD
	public StringSpan SlotPath
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.m_internalPath.SlotPath;
		}
	}

	// Token: 0x06004DA9 RID: 19881 RVA: 0x001DBBDA File Offset: 0x001D9DDA
	public SaveDataSlot GetSimpleSlot()
	{
		if (!(this.m_internalPath.PathRelativeToSlot == "d"))
		{
			return new SaveDataSlot(this.Type, this.SlotPath);
		}
		return this;
	}

	// Token: 0x06004DAA RID: 19882 RVA: 0x001DBC10 File Offset: 0x001D9E10
	public override string ToString()
	{
		if (this.SlotPath.Length != 0)
		{
			return SpanUtils.Concat(this.Type.ToStringCached<SaveDataType>(), "[", this.SlotPath, "]");
		}
		return this.Type.ToStringCached<SaveDataType>();
	}

	// Token: 0x06004DAB RID: 19883 RVA: 0x001DBC68 File Offset: 0x001D9E68
	public bool Equals(SaveDataSlot other)
	{
		if (other.m_internalPath == null)
		{
			return this.m_internalPath == null;
		}
		return this.Type == other.Type && this.SlotPath == other.SlotPath;
	}

	// Token: 0x06004DAC RID: 19884 RVA: 0x001DBCA8 File Offset: 0x001D9EA8
	public override bool Equals(object obj)
	{
		if (obj is SaveDataSlot)
		{
			SaveDataSlot saveDataSlot = (SaveDataSlot)obj;
			return this.Equals(saveDataSlot);
		}
		return false;
	}

	// Token: 0x06004DAD RID: 19885 RVA: 0x001DBCD0 File Offset: 0x001D9ED0
	public override int GetHashCode()
	{
		return (int)((this.Type * (SaveDataType)397) ^ (SaveDataType)this.SlotPath.GetHashCode());
	}

	// Token: 0x06004DAE RID: 19886 RVA: 0x001DBCFE File Offset: 0x001D9EFE
	public static bool operator ==(SaveDataSlot left, SaveDataSlot right)
	{
		return left.Equals(right);
	}

	// Token: 0x06004DAF RID: 19887 RVA: 0x001DBD08 File Offset: 0x001D9F08
	public static bool operator !=(SaveDataSlot left, SaveDataSlot right)
	{
		return !left.Equals(right);
	}

	// Token: 0x06004DB0 RID: 19888 RVA: 0x001DBD18 File Offset: 0x001D9F18
	public int CompareTo(SaveDataSlot other)
	{
		int num = this.Type.CompareTo(other.Type);
		if (num != 0)
		{
			return num;
		}
		return this.SlotPath.CompareTo(other.SlotPath, StringComparison.Ordinal);
	}

	// Token: 0x06004DB1 RID: 19889 RVA: 0x001DBD64 File Offset: 0x001D9F64
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

	// Token: 0x06004DB2 RID: 19890 RVA: 0x001DBD99 File Offset: 0x001D9F99
	public static bool operator <(SaveDataSlot left, SaveDataSlot right)
	{
		return left.CompareTo(right) < 0;
	}

	// Token: 0x06004DB3 RID: 19891 RVA: 0x001DBDA6 File Offset: 0x001D9FA6
	public static bool operator >(SaveDataSlot left, SaveDataSlot right)
	{
		return left.CompareTo(right) > 0;
	}

	// Token: 0x06004DB4 RID: 19892 RVA: 0x001DBDB3 File Offset: 0x001D9FB3
	public static bool operator <=(SaveDataSlot left, SaveDataSlot right)
	{
		return left.CompareTo(right) <= 0;
	}

	// Token: 0x06004DB5 RID: 19893 RVA: 0x001DBDC3 File Offset: 0x001D9FC3
	public static bool operator >=(SaveDataSlot left, SaveDataSlot right)
	{
		return left.CompareTo(right) >= 0;
	}

	// Token: 0x04003C2A RID: 15402
	[PublicizedFrom(EAccessModifier.Private)]
	public const string DUMMY_POSTFIX_WITHOUT_SLASH = "d";

	// Token: 0x04003C2B RID: 15403
	[PublicizedFrom(EAccessModifier.Private)]
	public const string DUMMY_POSTFIX_WITH_SLASH = "/d";

	// Token: 0x04003C2C RID: 15404
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly SaveDataManagedPath m_internalPath;
}
