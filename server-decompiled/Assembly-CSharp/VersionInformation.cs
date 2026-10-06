using System;
using System.IO;
using System.Text.RegularExpressions;

// Token: 0x0200131A RID: 4890
public class VersionInformation : IComparable<VersionInformation>
{
	// Token: 0x06009A7F RID: 39551 RVA: 0x003A87D4 File Offset: 0x003A69D4
	public VersionInformation(VersionInformation.EGameReleaseType _releaseType, int _major, int _minor, int _build)
	{
		this.ReleaseType = _releaseType;
		this.Major = _major;
		this.Minor = _minor;
		this.Build = _build;
		this.NumericalRepresentation = (int)((this.Major < 1) ? ((VersionInformation.EGameReleaseType)(-1)) : (((this.ReleaseType * (VersionInformation.EGameReleaseType)100 + this.Major) * (VersionInformation.EGameReleaseType)100 + this.Minor) * (VersionInformation.EGameReleaseType)1000 + this.Build));
		if (_releaseType == VersionInformation.EGameReleaseType.Alpha || (_releaseType == VersionInformation.EGameReleaseType.V && _major < 3))
		{
			this.ShortString = ((this.Major < 1) ? "Unk" : string.Format("{0}{1}.{2}", this.ReleaseType.ToStringCached<VersionInformation.EGameReleaseType>()[0], this.Major, _minor));
			this.LongStringNoBuild = ((this.Major < 1) ? "Unknown" : string.Format("{0} {1}.{2}", this.ReleaseType.ToStringCached<VersionInformation.EGameReleaseType>(), this.Major, _minor));
			this.LongString = ((this.Major < 1) ? "Unknown" : string.Format("{0} {1}.{2} (b{3})", new object[]
			{
				this.ReleaseType.ToStringCached<VersionInformation.EGameReleaseType>(),
				this.Major,
				_minor,
				this.Build
			}));
		}
		else
		{
			int num = _minor / 10;
			int num2 = _minor % 10;
			this.ShortString = ((this.Major < 1) ? "Unk" : string.Format("{0}{1}.{2}.{3}", new object[]
			{
				this.ReleaseType.ToStringCached<VersionInformation.EGameReleaseType>()[0],
				this.Major,
				num,
				num2
			}));
			this.LongStringNoBuild = ((this.Major < 1) ? "Unknown" : string.Format("{0} {1}.{2}.{3}", new object[]
			{
				this.ReleaseType.ToStringCached<VersionInformation.EGameReleaseType>(),
				this.Major,
				num,
				num2
			}));
			this.LongString = ((this.Major < 1) ? "Unknown" : string.Format("{0} {1}.{2}.{3} (b{4})", new object[]
			{
				this.ReleaseType.ToStringCached<VersionInformation.EGameReleaseType>(),
				this.Major,
				num,
				num2,
				this.Build
			}));
		}
		this.SerializableString = string.Format("{0}.{1}.{2}.{3}", new object[]
		{
			this.ReleaseType.ToStringCached<VersionInformation.EGameReleaseType>(),
			this.Major,
			this.Minor,
			this.Build
		});
		this.Version = new Version((int)((this.ReleaseType >= VersionInformation.EGameReleaseType.Alpha) ? this.ReleaseType : VersionInformation.EGameReleaseType.Alpha), (this.Major >= 0) ? this.Major : 0, (this.Minor >= 0) ? this.Minor : 0, (this.Build >= 0) ? this.Build : 0);
		this.IsValid = this.Major > 0;
	}

	// Token: 0x06009A80 RID: 39552 RVA: 0x003A8B00 File Offset: 0x003A6D00
	public int CompareTo(VersionInformation _other)
	{
		int num = this.ReleaseType.CompareTo(_other.ReleaseType);
		if (num != 0)
		{
			return num;
		}
		int num2 = this.Major.CompareTo(_other.Major);
		if (num2 != 0)
		{
			return num2;
		}
		int num3 = this.Minor.CompareTo(_other.Minor);
		if (num3 != 0)
		{
			return num3;
		}
		return this.Build.CompareTo(_other.Build);
	}

	// Token: 0x06009A81 RID: 39553 RVA: 0x003A8B77 File Offset: 0x003A6D77
	public bool EqualsMinor(VersionInformation _other)
	{
		return this.ReleaseType == _other.ReleaseType && this.Major == _other.Major && this.Minor == _other.Minor;
	}

	// Token: 0x06009A82 RID: 39554 RVA: 0x003A8BA5 File Offset: 0x003A6DA5
	public bool EqualsMajor(VersionInformation _other)
	{
		return this.ReleaseType == _other.ReleaseType && this.Major == _other.Major;
	}

	// Token: 0x06009A83 RID: 39555 RVA: 0x003A8BC8 File Offset: 0x003A6DC8
	public VersionInformation.EVersionComparisonResult CompareToRunningBuild()
	{
		VersionInformation cVersionInformation = Constants.cVersionInformation;
		if (this.ReleaseType != cVersionInformation.ReleaseType || this.Major != cVersionInformation.Major)
		{
			return VersionInformation.EVersionComparisonResult.DifferentMajor;
		}
		if (this.Minor < cVersionInformation.Minor)
		{
			return VersionInformation.EVersionComparisonResult.OlderMinor;
		}
		if (this.Minor > cVersionInformation.Minor)
		{
			return VersionInformation.EVersionComparisonResult.NewerMinor;
		}
		if (this.Build != cVersionInformation.Build)
		{
			return VersionInformation.EVersionComparisonResult.SameMinor;
		}
		return VersionInformation.EVersionComparisonResult.SameBuild;
	}

	// Token: 0x06009A84 RID: 39556 RVA: 0x003A8C2C File Offset: 0x003A6E2C
	public static bool TryParseSerializedString(string _serializedVersionInformation, out VersionInformation _result)
	{
		_result = null;
		string[] array = _serializedVersionInformation.Split('.', StringSplitOptions.None);
		if (array.Length != 4)
		{
			return false;
		}
		VersionInformation.EGameReleaseType egameReleaseType;
		if (!EnumUtils.TryParse<VersionInformation.EGameReleaseType>(array[0], out egameReleaseType, false))
		{
			return false;
		}
		int num;
		if (!StringParsers.TryParseSInt32(array[1], out num))
		{
			return false;
		}
		int num2;
		if (!StringParsers.TryParseSInt32(array[2], out num2))
		{
			return false;
		}
		int num3;
		if (!StringParsers.TryParseSInt32(array[3], out num3))
		{
			return false;
		}
		_result = new VersionInformation(egameReleaseType, num, num2, num3);
		return true;
	}

	// Token: 0x06009A85 RID: 39557 RVA: 0x003A8C94 File Offset: 0x003A6E94
	public static bool TryParseLegacyString(string _legacyVersionString, out VersionInformation _verInfo)
	{
		Match match = VersionInformation.legacyVersionStringMatcher.Match(_legacyVersionString);
		_verInfo = null;
		if (!match.Success)
		{
			return false;
		}
		int num;
		if (!StringParsers.TryParseSInt32(match.Groups[1].Value, out num))
		{
			return false;
		}
		int num2;
		if (match.Groups[2].Success)
		{
			if (!StringParsers.TryParseSInt32(match.Groups[2].Value, out num2))
			{
				return false;
			}
		}
		else
		{
			num2 = 0;
		}
		int num3;
		if (!StringParsers.TryParseSInt32(match.Groups[3].Value, out num3))
		{
			return false;
		}
		_verInfo = new VersionInformation(VersionInformation.EGameReleaseType.Alpha, num, num2, num3);
		return true;
	}

	// Token: 0x06009A86 RID: 39558 RVA: 0x003A8D2F File Offset: 0x003A6F2F
	public void Write(BinaryWriter _writer)
	{
		_writer.Write((byte)this.ReleaseType);
		_writer.Write(this.Major);
		_writer.Write(this.Minor);
		_writer.Write(this.Build);
	}

	// Token: 0x06009A87 RID: 39559 RVA: 0x003A8D64 File Offset: 0x003A6F64
	public static VersionInformation Read(BinaryReader _reader)
	{
		VersionInformation.EGameReleaseType egameReleaseType = (VersionInformation.EGameReleaseType)_reader.ReadByte();
		int num = _reader.ReadInt32();
		int num2 = _reader.ReadInt32();
		int num3 = _reader.ReadInt32();
		return new VersionInformation(egameReleaseType, num, num2, num3);
	}

	// Token: 0x04007469 RID: 29801
	public readonly VersionInformation.EGameReleaseType ReleaseType;

	// Token: 0x0400746A RID: 29802
	public readonly int Major;

	// Token: 0x0400746B RID: 29803
	public readonly int Minor;

	// Token: 0x0400746C RID: 29804
	public readonly int Build;

	// Token: 0x0400746D RID: 29805
	public readonly bool IsValid;

	// Token: 0x0400746E RID: 29806
	public readonly int NumericalRepresentation;

	// Token: 0x0400746F RID: 29807
	public readonly string ShortString;

	// Token: 0x04007470 RID: 29808
	public readonly string LongStringNoBuild;

	// Token: 0x04007471 RID: 29809
	public readonly string LongString;

	// Token: 0x04007472 RID: 29810
	public readonly string SerializableString;

	// Token: 0x04007473 RID: 29811
	public readonly Version Version;

	// Token: 0x04007474 RID: 29812
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Regex legacyVersionStringMatcher = new Regex("^\\s*Alpha\\s*(\\d+)(?:\\.(\\d+))?\\s*(?:\\(b(\\d+)\\))?\\s*$");

	// Token: 0x0200131B RID: 4891
	public enum EGameReleaseType
	{
		// Token: 0x04007476 RID: 29814
		Alpha,
		// Token: 0x04007477 RID: 29815
		V
	}

	// Token: 0x0200131C RID: 4892
	public enum EVersionComparisonResult
	{
		// Token: 0x04007479 RID: 29817
		SameBuild,
		// Token: 0x0400747A RID: 29818
		SameMinor,
		// Token: 0x0400747B RID: 29819
		NewerMinor,
		// Token: 0x0400747C RID: 29820
		OlderMinor,
		// Token: 0x0400747D RID: 29821
		DifferentMajor
	}
}
