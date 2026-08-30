using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;

// Token: 0x020013F4 RID: 5108
public static class EnumUtils
{
	// Token: 0x0600A062 RID: 41058 RVA: 0x003C443F File Offset: 0x003C263F
	public static string ToStringCached<TEnum>(this TEnum _enumValue) where TEnum : struct, IConvertible
	{
		return EnumUtils.EnumInfoCache<TEnum>.Instance.GetName(_enumValue);
	}

	// Token: 0x0600A063 RID: 41059 RVA: 0x003C444C File Offset: 0x003C264C
	public static int Ordinal<TEnum>(this TEnum _enumValue) where TEnum : struct, IConvertible
	{
		return EnumInt32ToInt.Convert<TEnum>(_enumValue);
	}

	// Token: 0x0600A064 RID: 41060 RVA: 0x003C4454 File Offset: 0x003C2654
	public static bool TryFromOrdinal<TEnum>(int _ordinal, out TEnum _result) where TEnum : struct, IConvertible
	{
		IList<TEnum> list = EnumUtils.Values<TEnum>();
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i].Ordinal<TEnum>() == _ordinal)
			{
				_result = list[i];
				return true;
			}
		}
		_result = default(TEnum);
		return false;
	}

	// Token: 0x0600A065 RID: 41061 RVA: 0x003C44A0 File Offset: 0x003C26A0
	public static TEnum Parse<TEnum>(string _name, TEnum _default, bool _ignoreCase = false) where TEnum : struct, IConvertible
	{
		TEnum tenum;
		if (!EnumUtils.TryParse<TEnum>(_name, out tenum, _ignoreCase))
		{
			tenum = _default;
		}
		return tenum;
	}

	// Token: 0x0600A066 RID: 41062 RVA: 0x003C44BB File Offset: 0x003C26BB
	public static TEnum Parse<TEnum>(string _name, bool _ignoreCase = false) where TEnum : struct, IConvertible
	{
		return EnumUtils.EnumInfoCache<TEnum>.Instance.Parse(_name, _ignoreCase);
	}

	// Token: 0x0600A067 RID: 41063 RVA: 0x003C44C9 File Offset: 0x003C26C9
	public static bool TryParse<TEnum>(string _name, out TEnum _result, bool _ignoreCase = false) where TEnum : struct, IConvertible
	{
		return EnumUtils.EnumInfoCache<TEnum>.Instance.TryParse(_name, out _result, _ignoreCase);
	}

	// Token: 0x0600A068 RID: 41064 RVA: 0x003C44D8 File Offset: 0x003C26D8
	public static bool TryParseIgnoreCase<TEnum>(string _name, out TEnum _result) where TEnum : struct, IConvertible
	{
		return EnumUtils.EnumInfoCache<TEnum>.Instance.TryParse(_name, out _result, true);
	}

	// Token: 0x0600A069 RID: 41065 RVA: 0x003C44E7 File Offset: 0x003C26E7
	public static bool HasName<TEnum>(string _name, bool _ignoreCase = false) where TEnum : struct, IConvertible
	{
		return EnumUtils.EnumInfoCache<TEnum>.Instance.HasName(_name, _ignoreCase);
	}

	// Token: 0x0600A06A RID: 41066 RVA: 0x003C44F5 File Offset: 0x003C26F5
	public static IList<TEnum> Values<TEnum>() where TEnum : struct, IConvertible
	{
		return EnumUtils.EnumInfoCache<TEnum>.Instance.EnumValues;
	}

	// Token: 0x0600A06B RID: 41067 RVA: 0x003C4501 File Offset: 0x003C2701
	public static IList<string> Names<TEnum>() where TEnum : struct, IConvertible
	{
		return EnumUtils.EnumInfoCache<TEnum>.Instance.EnumNames;
	}

	// Token: 0x0600A06C RID: 41068 RVA: 0x003C4510 File Offset: 0x003C2710
	public static TEnum CycleEnum<TEnum>(this TEnum _enumVal, bool _reverse = false, bool _wrap = true) where TEnum : struct, IConvertible
	{
		if (!typeof(TEnum).IsEnum)
		{
			throw new ArgumentException("Argument " + typeof(TEnum).FullName + " is not an Enum");
		}
		IList<TEnum> enumValues = EnumUtils.EnumInfoCache<TEnum>.Instance.EnumValues;
		int num = enumValues.IndexOf(_enumVal) + (_reverse ? (-1) : 1);
		if (num >= enumValues.Count)
		{
			num = (_wrap ? 0 : (enumValues.Count - 1));
		}
		else if (num < 0)
		{
			num = (_wrap ? (enumValues.Count - 1) : 0);
		}
		return enumValues[num];
	}

	// Token: 0x0600A06D RID: 41069 RVA: 0x003C45A4 File Offset: 0x003C27A4
	public static TEnum CycleEnum<TEnum>(this TEnum _enumVal, TEnum _minVal, TEnum _maxVal, bool _reverse = false, bool _wrap = true) where TEnum : struct, IConvertible
	{
		if (!typeof(TEnum).IsEnum)
		{
			throw new ArgumentException("Argument " + typeof(TEnum).FullName + " is not an Enum");
		}
		IList<TEnum> enumValues = EnumUtils.EnumInfoCache<TEnum>.Instance.EnumValues;
		int num = enumValues.IndexOf(_minVal);
		if (num < 0)
		{
			throw new ArgumentException(string.Format("Could not find index of {0}", _minVal), "_minVal");
		}
		int num2 = enumValues.IndexOf(_maxVal);
		if (num2 < 0)
		{
			throw new ArgumentException(string.Format("Could not find index of {0}", _maxVal), "_maxVal");
		}
		if (num2 < num)
		{
			throw new ArgumentException(string.Format("Max of {0} with index {1} is less than min of {2} with index {3}", new object[] { _maxVal, num2, _minVal, num }));
		}
		int num3 = enumValues.IndexOf(_enumVal);
		if (num3 < 0)
		{
			Log.Warning(string.Format("Could not find index of {0}: {1} (using min)", "_enumVal", _enumVal));
			return enumValues[num];
		}
		int num4 = num2 - num + 1;
		if (num4 <= 1)
		{
			return enumValues[num];
		}
		int num5 = num3 - num + (_reverse ? (-1) : 1);
		if (_wrap)
		{
			num5 %= num4;
			if (num5 < 0)
			{
				num5 += num4;
			}
		}
		else if (num5 < 0)
		{
			num5 = 0;
		}
		else if (num5 >= num4)
		{
			num5 = num4 - 1;
		}
		int num6 = num + num5;
		return enumValues[num6];
	}

	// Token: 0x0600A06E RID: 41070 RVA: 0x003C4709 File Offset: 0x003C2909
	public static TEnum MaxValue<TEnum>() where TEnum : struct, IConvertible
	{
		return EnumUtils.EnumInfoCache<TEnum>.Instance.EnumValues[EnumUtils.EnumInfoCache<TEnum>.Instance.EnumValues.Count - 1];
	}

	// Token: 0x0600A06F RID: 41071 RVA: 0x003C472B File Offset: 0x003C292B
	public static TEnum MinValue<TEnum>() where TEnum : struct, IConvertible
	{
		return EnumUtils.EnumInfoCache<TEnum>.Instance.EnumValues[0];
	}

	// Token: 0x020013F5 RID: 5109
	[PublicizedFrom(EAccessModifier.Private)]
	public class EnumInfoCache<TEnum> where TEnum : struct, IConvertible
	{
		// Token: 0x170012E3 RID: 4835
		// (get) Token: 0x0600A070 RID: 41072 RVA: 0x003C473D File Offset: 0x003C293D
		public static EnumUtils.EnumInfoCache<TEnum> Instance
		{
			get
			{
				if (EnumUtils.EnumInfoCache<TEnum>.instance == null)
				{
					EnumUtils.EnumInfoCache<TEnum>.instance = new EnumUtils.EnumInfoCache<TEnum>();
				}
				return EnumUtils.EnumInfoCache<TEnum>.instance;
			}
		}

		// Token: 0x0600A071 RID: 41073 RVA: 0x003C4758 File Offset: 0x003C2958
		[PublicizedFrom(EAccessModifier.Private)]
		public EnumInfoCache()
		{
			if (!typeof(TEnum).IsEnum)
			{
				throw new NotSupportedException(typeof(TEnum).FullName + " is not an enum type.");
			}
			object[] customAttributes = typeof(TEnum).GetCustomAttributes(false);
			for (int i = 0; i < customAttributes.Length; i++)
			{
				if (((Attribute)customAttributes[i]).GetType().Name == "FlagsAttribute")
				{
					this.isFlags = true;
					break;
				}
			}
			Array values = Enum.GetValues(typeof(TEnum));
			this.enumValues = new List<TEnum>(values.Length);
			this.enumToName = new EnumDictionary<TEnum, string>(values.Length);
			foreach (object obj in values)
			{
				TEnum tenum = (TEnum)((object)obj);
				string text = tenum.ToString(CultureInfo.InvariantCulture);
				if (!this.enumValues.Contains(tenum))
				{
					this.enumValues.Add(tenum);
				}
				if (!this.enumToName.ContainsKey(tenum))
				{
					this.enumToName.Add(tenum, text);
				}
			}
			this.enumValues.Sort((TEnum _enumA, TEnum _enumB) => _enumA.Ordinal<TEnum>().CompareTo(_enumB.Ordinal<TEnum>()));
			string[] names = Enum.GetNames(typeof(TEnum));
			this.enumNames = new List<string>(names.Length);
			this.nameToEnumCaseSensitive = new Dictionary<string, TEnum>(names.Length, StringComparer.Ordinal);
			this.nameToEnumCaseInsensitive = new CaseInsensitiveStringDictionary<TEnum>(names.Length);
			foreach (string text2 in names)
			{
				TEnum tenum2 = (TEnum)((object)Enum.Parse(typeof(TEnum), text2));
				if (!this.enumNames.Contains(text2))
				{
					this.enumNames.Add(text2);
				}
				if (!this.nameToEnumCaseSensitive.ContainsKey(text2))
				{
					this.nameToEnumCaseSensitive.Add(text2, tenum2);
				}
				if (!this.nameToEnumCaseInsensitive.ContainsKey(text2))
				{
					this.nameToEnumCaseInsensitive.Add(text2, tenum2);
				}
			}
			this.EnumValues = new ReadOnlyCollection<TEnum>(this.enumValues);
			this.EnumNames = new ReadOnlyCollection<string>(this.enumNames);
		}

		// Token: 0x0600A072 RID: 41074 RVA: 0x003C49C0 File Offset: 0x003C2BC0
		public string GetName(TEnum _enumValue)
		{
			if (this.isFlags)
			{
				if (!this.enumToName.ContainsKey(_enumValue))
				{
					this.enumToName.Add(_enumValue, _enumValue.ToString(CultureInfo.InvariantCulture));
				}
				return this.enumToName[_enumValue];
			}
			if (this.enumToName.ContainsKey(_enumValue))
			{
				return this.enumToName[_enumValue];
			}
			return _enumValue.ToString(CultureInfo.InvariantCulture);
		}

		// Token: 0x0600A073 RID: 41075 RVA: 0x003C4A3C File Offset: 0x003C2C3C
		public TEnum Parse(string _name, bool _ignoreCase)
		{
			if (string.IsNullOrEmpty(_name))
			{
				throw new ArgumentException("Value null or empty", "_name");
			}
			TEnum tenum;
			if ((_ignoreCase ? this.nameToEnumCaseInsensitive : this.nameToEnumCaseSensitive).TryGetValue(_name, out tenum))
			{
				return tenum;
			}
			TEnum tenum2 = (TEnum)((object)Enum.Parse(typeof(TEnum), _name, _ignoreCase));
			this.nameToEnumCaseSensitive.Add(_name, tenum2);
			this.nameToEnumCaseInsensitive.Add(_name, tenum2);
			return tenum2;
		}

		// Token: 0x0600A074 RID: 41076 RVA: 0x003C4AB0 File Offset: 0x003C2CB0
		public bool TryParse(string _name, out TEnum _result, bool _ignoreCase)
		{
			_result = default(TEnum);
			if (string.IsNullOrEmpty(_name))
			{
				return false;
			}
			if ((_ignoreCase ? this.nameToEnumCaseInsensitive : this.nameToEnumCaseSensitive).TryGetValue(_name, out _result))
			{
				return true;
			}
			bool flag;
			try
			{
				_result = (TEnum)((object)Enum.Parse(typeof(TEnum), _name, _ignoreCase));
				this.nameToEnumCaseSensitive.Add(_name, _result);
				this.nameToEnumCaseInsensitive.Add(_name, _result);
				flag = true;
			}
			catch (Exception)
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x0600A075 RID: 41077 RVA: 0x003C4B48 File Offset: 0x003C2D48
		public bool HasName(string _name, bool _ignoreCase)
		{
			if (string.IsNullOrEmpty(_name))
			{
				throw new ArgumentException("Value null or empty", "_name");
			}
			return (_ignoreCase ? this.nameToEnumCaseInsensitive : this.nameToEnumCaseSensitive).ContainsKey(_name);
		}

		// Token: 0x04007914 RID: 30996
		[PublicizedFrom(EAccessModifier.Private)]
		public static EnumUtils.EnumInfoCache<TEnum> instance;

		// Token: 0x04007915 RID: 30997
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly List<TEnum> enumValues;

		// Token: 0x04007916 RID: 30998
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly List<string> enumNames;

		// Token: 0x04007917 RID: 30999
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Dictionary<TEnum, string> enumToName;

		// Token: 0x04007918 RID: 31000
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Dictionary<string, TEnum> nameToEnumCaseSensitive;

		// Token: 0x04007919 RID: 31001
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Dictionary<string, TEnum> nameToEnumCaseInsensitive;

		// Token: 0x0400791A RID: 31002
		public readonly ReadOnlyCollection<TEnum> EnumValues;

		// Token: 0x0400791B RID: 31003
		public readonly ReadOnlyCollection<string> EnumNames;

		// Token: 0x0400791C RID: 31004
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly bool isFlags;
	}
}
