using System;
using System.IO;
using System.Text.RegularExpressions;

// Token: 0x020012FA RID: 4858
public class VersionInformation : IComparable<VersionInformation>
{
	// Token: 0x060099D3 RID: 39379 RVA: 0x003A3C24 File Offset: 0x003A1E24
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

	// Token: 0x060099D4 RID: 39380 RVA: 0x003A3F50 File Offset: 0x003A2150
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

	// Token: 0x060099D5 RID: 39381 RVA: 0x003A3FC7 File Offset: 0x003A21C7
	public bool EqualsMinor(VersionInformation _other)
	{
		return this.ReleaseType == _other.ReleaseType && this.Major == _other.Major && this.Minor == _other.Minor;
	}

	// Token: 0x060099D6 RID: 39382 RVA: 0x003A3FF5 File Offset: 0x003A21F5
	public bool EqualsMajor(VersionInformation _other)
	{
		return this.ReleaseType == _other.ReleaseType && this.Major == _other.Major;
	}

	// Token: 0x060099D7 RID: 39383 RVA: 0x003A4018 File Offset: 0x003A2218
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

	// Token: 0x060099D8 RID: 39384 RVA: 0x003A407C File Offset: 0x003A227C
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

	// Token: 0x060099D9 RID: 39385 RVA: 0x003A40E4 File Offset: 0x003A22E4
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

	// Token: 0x060099DA RID: 39386 RVA: 0x003A417F File Offset: 0x003A237F
	public void Write(BinaryWriter _writer)
	{
		_writer.Write((byte)this.ReleaseType);
		_writer.Write(this.Major);
		_writer.Write(this.Minor);
		_writer.Write(this.Build);
	}

	// Token: 0x060099DB RID: 39387 RVA: 0x003A41B4 File Offset: 0x003A23B4
	public static VersionInformation Read(BinaryReader _reader)
	{
		VersionInformation.EGameReleaseType egameReleaseType = (VersionInformation.EGameReleaseType)_reader.ReadByte();
		int num = _reader.ReadInt32();
		int num2 = _reader.ReadInt32();
		int num3 = _reader.ReadInt32();
		return new VersionInformation(egameReleaseType, num, num2, num3);
	}

	// Token: 0x040073EE RID: 29678
	public readonly VersionInformation.EGameReleaseType ReleaseType;

	// Token: 0x040073EF RID: 29679
	public readonly int Major;

	// Token: 0x040073F0 RID: 29680
	public readonly int Minor;

	// Token: 0x040073F1 RID: 29681
	public readonly int Build;

	// Token: 0x040073F2 RID: 29682
	public readonly bool IsValid;

	// Token: 0x040073F3 RID: 29683
	public readonly int NumericalRepresentation;

	// Token: 0x040073F4 RID: 29684
	public readonly string ShortString;

	// Token: 0x040073F5 RID: 29685
	public readonly string LongStringNoBuild;

	// Token: 0x040073F6 RID: 29686
	public readonly string LongString;

	// Token: 0x040073F7 RID: 29687
	public readonly string SerializableString;

	// Token: 0x040073F8 RID: 29688
	public readonly Version Version;

	// Token: 0x040073F9 RID: 29689
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Regex legacyVersionStringMatcher = new Regex("^\\s*Alpha\\s*(\\d+)(?:\\.(\\d+))?\\s*(?:\\(b(\\d+)\\))?\\s*$");

	// Token: 0x020012FB RID: 4859
	public enum EGameReleaseType
	{
		// Token: 0x040073FB RID: 29691
		Alpha,
		// Token: 0x040073FC RID: 29692
		V
	}

	// Token: 0x020012FC RID: 4860
	public enum EVersionComparisonResult
	{
		// Token: 0x040073FE RID: 29694
		SameBuild,
		// Token: 0x040073FF RID: 29695
		SameMinor,
		// Token: 0x04007400 RID: 29696
		NewerMinor,
		// Token: 0x04007401 RID: 29697
		OlderMinor,
		// Token: 0x04007402 RID: 29698
		DifferentMajor
	}
}
