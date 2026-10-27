using System;
using System.IO;
using System.Text.RegularExpressions;

// Token: 0x0200132C RID: 4908
public class VersionInformation : IComparable<VersionInformation>
{
	// Token: 0x06009AD7 RID: 39639 RVA: 0x003AABD8 File Offset: 0x003A8DD8
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

	// Token: 0x06009AD8 RID: 39640 RVA: 0x003AAF04 File Offset: 0x003A9104
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

	// Token: 0x06009AD9 RID: 39641 RVA: 0x003AAF7B File Offset: 0x003A917B
	public bool EqualsMinor(VersionInformation _other)
	{
		return this.ReleaseType == _other.ReleaseType && this.Major == _other.Major && this.Minor == _other.Minor;
	}

	// Token: 0x06009ADA RID: 39642 RVA: 0x003AAFA9 File Offset: 0x003A91A9
	public bool EqualsMajor(VersionInformation _other)
	{
		return this.ReleaseType == _other.ReleaseType && this.Major == _other.Major;
	}

	// Token: 0x06009ADB RID: 39643 RVA: 0x003AAFCC File Offset: 0x003A91CC
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

	// Token: 0x06009ADC RID: 39644 RVA: 0x003AB030 File Offset: 0x003A9230
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

	// Token: 0x06009ADD RID: 39645 RVA: 0x003AB098 File Offset: 0x003A9298
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

	// Token: 0x06009ADE RID: 39646 RVA: 0x003AB133 File Offset: 0x003A9333
	public void Write(BinaryWriter _writer)
	{
		_writer.Write((byte)this.ReleaseType);
		_writer.Write(this.Major);
		_writer.Write(this.Minor);
		_writer.Write(this.Build);
	}

	// Token: 0x06009ADF RID: 39647 RVA: 0x003AB168 File Offset: 0x003A9368
	public static VersionInformation Read(BinaryReader _reader)
	{
		VersionInformation.EGameReleaseType egameReleaseType = (VersionInformation.EGameReleaseType)_reader.ReadByte();
		int num = _reader.ReadInt32();
		int num2 = _reader.ReadInt32();
		int num3 = _reader.ReadInt32();
		return new VersionInformation(egameReleaseType, num, num2, num3);
	}

	// Token: 0x040074AA RID: 29866
	public readonly VersionInformation.EGameReleaseType ReleaseType;

	// Token: 0x040074AB RID: 29867
	public readonly int Major;

	// Token: 0x040074AC RID: 29868
	public readonly int Minor;

	// Token: 0x040074AD RID: 29869
	public readonly int Build;

	// Token: 0x040074AE RID: 29870
	public readonly bool IsValid;

	// Token: 0x040074AF RID: 29871
	public readonly int NumericalRepresentation;

	// Token: 0x040074B0 RID: 29872
	public readonly string ShortString;

	// Token: 0x040074B1 RID: 29873
	public readonly string LongStringNoBuild;

	// Token: 0x040074B2 RID: 29874
	public readonly string LongString;

	// Token: 0x040074B3 RID: 29875
	public readonly string SerializableString;

	// Token: 0x040074B4 RID: 29876
	public readonly Version Version;

	// Token: 0x040074B5 RID: 29877
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Regex legacyVersionStringMatcher = new Regex("^\\s*Alpha\\s*(\\d+)(?:\\.(\\d+))?\\s*(?:\\(b(\\d+)\\))?\\s*$");

	// Token: 0x0200132D RID: 4909
	public enum EGameReleaseType
	{
		// Token: 0x040074B7 RID: 29879
		Alpha,
		// Token: 0x040074B8 RID: 29880
		V
	}

	// Token: 0x0200132E RID: 4910
	public enum EVersionComparisonResult
	{
		// Token: 0x040074BA RID: 29882
		SameBuild,
		// Token: 0x040074BB RID: 29883
		SameMinor,
		// Token: 0x040074BC RID: 29884
		NewerMinor,
		// Token: 0x040074BD RID: 29885
		OlderMinor,
		// Token: 0x040074BE RID: 29886
		DifferentMajor
	}
}
