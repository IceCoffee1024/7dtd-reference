using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Xml;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000063 RID: 99
[DoNotTouchSerializableFlags]
[Preserve]
[Serializable]
public abstract class PlatformUserIdentifierAbs : IEquatable<PlatformUserIdentifierAbs>
{
	// Token: 0x1700002B RID: 43
	// (get) Token: 0x060001E2 RID: 482
	public abstract EPlatformIdentifier PlatformIdentifier { get; }

	// Token: 0x1700002C RID: 44
	// (get) Token: 0x060001E3 RID: 483
	public abstract string PlatformIdentifierString { get; }

	// Token: 0x1700002D RID: 45
	// (get) Token: 0x060001E4 RID: 484
	public abstract string ReadablePlatformUserIdentifier { get; }

	// Token: 0x1700002E RID: 46
	// (get) Token: 0x060001E5 RID: 485
	public abstract string CombinedString { get; }

	// Token: 0x060001E6 RID: 486
	public abstract bool DecodeTicket(string _ticket);

	// Token: 0x060001E7 RID: 487 RVA: 0x00010E62 File Offset: 0x0000F062
	[PublicizedFrom(EAccessModifier.ProtectedInternal)]
	public virtual int GetCustomDataLengthEstimate()
	{
		return 0;
	}

	// Token: 0x060001E8 RID: 488 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.ProtectedInternal)]
	public virtual void WriteCustomData(BinaryWriter _writer)
	{
	}

	// Token: 0x060001E9 RID: 489 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.ProtectedInternal)]
	public virtual void ReadCustomData(BinaryReader _reader)
	{
	}

	// Token: 0x060001EA RID: 490 RVA: 0x00010E68 File Offset: 0x0000F068
	public static PlatformUserIdentifierAbs FromStream(Stream _sourceStream, bool _errorOnEmpty = false, bool _inclCustomData = false)
	{
		PlatformUserIdentifierAbs platformUserIdentifierAbs;
		using (PooledBinaryReader pooledBinaryReader = MemoryPools.poolBinaryReader.AllocSync(true))
		{
			pooledBinaryReader.SetBaseStream(_sourceStream);
			platformUserIdentifierAbs = PlatformUserIdentifierAbs.FromStream(pooledBinaryReader, _errorOnEmpty, false);
		}
		return platformUserIdentifierAbs;
	}

	// Token: 0x060001EB RID: 491 RVA: 0x00010EB0 File Offset: 0x0000F0B0
	public static PlatformUserIdentifierAbs FromStream(BinaryReader _sourceReader, bool _errorOnEmpty = false, bool _inclCustomData = false)
	{
		if (!_sourceReader.ReadBoolean())
		{
			if (_errorOnEmpty)
			{
				Log.Error("Empty user identifier string\nFrom: " + StackTraceUtility.ExtractStackTrace());
			}
			return null;
		}
		_sourceReader.ReadByte();
		string text = _sourceReader.ReadString();
		string text2 = _sourceReader.ReadString();
		bool flag = GameManager.Instance;
		PlatformUserIdentifierAbs platformUserIdentifierAbs = PlatformUserIdentifierAbs.FromPlatformAndId(text, text2, flag);
		if (_inclCustomData)
		{
			platformUserIdentifierAbs.ReadCustomData(_sourceReader);
		}
		return platformUserIdentifierAbs;
	}

	// Token: 0x060001EC RID: 492 RVA: 0x00010F10 File Offset: 0x0000F110
	[return: TupleElementNames(new string[] { "platformName", "userId" })]
	public static ValueTuple<string, string>? FieldsFromStream(BinaryReader _sourceReader, bool _errorOnEmpty = false)
	{
		if (!_sourceReader.ReadBoolean())
		{
			if (_errorOnEmpty)
			{
				Log.Error("Empty user identifier string\nFrom: " + StackTraceUtility.ExtractStackTrace());
			}
			return null;
		}
		_sourceReader.ReadByte();
		string text = _sourceReader.ReadString();
		string text2 = _sourceReader.ReadString();
		return new ValueTuple<string, string>?(new ValueTuple<string, string>(text, text2));
	}

	// Token: 0x060001ED RID: 493 RVA: 0x00010F65 File Offset: 0x0000F165
	public void ToXml(XmlElement _xmlElement, string _attributePrefix = "")
	{
		_xmlElement.SetAttrib(_attributePrefix + "platform", this.PlatformIdentifierString);
		_xmlElement.SetAttrib(_attributePrefix + "userid", this.ReadablePlatformUserIdentifier);
	}

	// Token: 0x060001EE RID: 494 RVA: 0x00010F98 File Offset: 0x0000F198
	public static PlatformUserIdentifierAbs FromXml(XmlElement _xmlElement, bool _warnings = true, string _attributePrefix = null)
	{
		string text = "platform";
		string text2 = "userid";
		if (!string.IsNullOrEmpty(_attributePrefix))
		{
			text = _attributePrefix + text;
			text2 = _attributePrefix + text2;
		}
		if (_xmlElement.HasAttribute(text) && _xmlElement.HasAttribute(text2))
		{
			string attribute = _xmlElement.GetAttribute(text);
			string attribute2 = _xmlElement.GetAttribute(text2);
			return PlatformUserIdentifierAbs.FromPlatformAndId(attribute, attribute2, true);
		}
		if (_warnings)
		{
			Log.Warning(string.Concat(new string[] { "Entry missing '", text, "' or '", text2, "' attribute: ", _xmlElement.OuterXml }));
		}
		return null;
	}

	// Token: 0x060001EF RID: 495 RVA: 0x0001102F File Offset: 0x0000F22F
	public static bool TryFromCombinedString(string _combinedString, out PlatformUserIdentifierAbs _userIdentifier)
	{
		_userIdentifier = PlatformUserIdentifierAbs.FromCombinedString(_combinedString, false);
		return _userIdentifier != null;
	}

	// Token: 0x060001F0 RID: 496 RVA: 0x00011040 File Offset: 0x0000F240
	public static PlatformUserIdentifierAbs FromCombinedString(string _combinedString, bool _logErrors = true)
	{
		if (_combinedString == null)
		{
			if (_logErrors)
			{
				Log.Error("Empty user identifier string\nFrom: " + StackTraceUtility.ExtractStackTrace());
			}
			return null;
		}
		int num = _combinedString.IndexOf('_');
		if (num < 0)
		{
			if (_logErrors)
			{
				Log.Error("Missing separator '_' in string: " + _combinedString + "\nFrom: " + StackTraceUtility.ExtractStackTrace());
			}
			return null;
		}
		if (num == 0)
		{
			if (_logErrors)
			{
				Log.Error("Missing platform (before the separator '_') in string: " + _combinedString + "\nFrom: " + StackTraceUtility.ExtractStackTrace());
			}
			return null;
		}
		if (num + 1 >= _combinedString.Length)
		{
			if (_logErrors)
			{
				Log.Error("Missing user identifier (after the separator '_') in string: " + _combinedString + "\nFrom: " + StackTraceUtility.ExtractStackTrace());
			}
			return null;
		}
		string text = _combinedString.Substring(0, num);
		string text2 = _combinedString.Substring(num + 1, _combinedString.Length - num - 1);
		return PlatformUserIdentifierAbs.FromPlatformAndId(text, text2, _logErrors);
	}

	// Token: 0x060001F1 RID: 497 RVA: 0x00011104 File Offset: 0x0000F304
	public static PlatformUserIdentifierAbs FromPlatformAndId(string _platformName, string _userId, bool _logErrors = true)
	{
		EPlatformIdentifier eplatformIdentifier;
		if (!PlatformManager.TryPlatformIdentifierFromString(_platformName, out eplatformIdentifier))
		{
			if (_logErrors)
			{
				Log.Error("Invalid platform name in user identifier: " + _platformName + "\nFrom: " + StackTraceUtility.ExtractStackTrace());
			}
			return null;
		}
		AbsUserIdentifierFactory absUserIdentifierFactory;
		if (PlatformManager.UserIdentifierFactories.TryGetValue(eplatformIdentifier, out absUserIdentifierFactory))
		{
			PlatformUserIdentifierAbs platformUserIdentifierAbs = null;
			try
			{
				platformUserIdentifierAbs = absUserIdentifierFactory.FromId(_userId);
			}
			catch (Exception)
			{
				if (_logErrors)
				{
					throw;
				}
			}
			return platformUserIdentifierAbs;
		}
		if (_logErrors)
		{
			throw new ArgumentOutOfRangeException("platformIdentifier", "Invalid platform " + eplatformIdentifier.ToString());
		}
		return null;
	}

	// Token: 0x060001F2 RID: 498
	public abstract bool Equals(PlatformUserIdentifierAbs _other);

	// Token: 0x060001F3 RID: 499 RVA: 0x00011198 File Offset: 0x0000F398
	public override bool Equals(object _obj)
	{
		if (_obj == null)
		{
			return false;
		}
		if (this == _obj)
		{
			return true;
		}
		PlatformUserIdentifierAbs platformUserIdentifierAbs = _obj as PlatformUserIdentifierAbs;
		return platformUserIdentifierAbs != null && this.Equals(platformUserIdentifierAbs);
	}

	// Token: 0x060001F4 RID: 500 RVA: 0x000111C3 File Offset: 0x0000F3C3
	public static bool Equals(PlatformUserIdentifierAbs _a, PlatformUserIdentifierAbs _b)
	{
		return _a == _b || (_a != null && _a.Equals(_b));
	}

	// Token: 0x060001F5 RID: 501 RVA: 0x000111D7 File Offset: 0x0000F3D7
	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	// Token: 0x060001F6 RID: 502 RVA: 0x000111DF File Offset: 0x0000F3DF
	public override string ToString()
	{
		return this.CombinedString;
	}

	// Token: 0x060001F7 RID: 503 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Protected)]
	public PlatformUserIdentifierAbs()
	{
	}

	// Token: 0x040002A8 RID: 680
	public const byte UserIdentifierVersion = 1;
}
