using System;
using System.Globalization;
using System.Xml.Linq;

// Token: 0x020006C4 RID: 1732
public class EffectGroupDescription
{
	// Token: 0x17000584 RID: 1412
	// (get) Token: 0x06003633 RID: 13875 RVA: 0x0016464A File Offset: 0x0016284A
	public string Description
	{
		get
		{
			if (Localization.Exists(this.DescriptionKey, false))
			{
				return Localization.Get(this.DescriptionKey, false, null);
			}
			return this.CustomDescription;
		}
	}

	// Token: 0x17000585 RID: 1413
	// (get) Token: 0x06003634 RID: 13876 RVA: 0x0016466E File Offset: 0x0016286E
	public string LongDescription
	{
		get
		{
			return Localization.Get(this.LongDescriptionKey, false, null);
		}
	}

	// Token: 0x06003635 RID: 13877 RVA: 0x0016467D File Offset: 0x0016287D
	public EffectGroupDescription(int _minLevel, int _maxLevel, string _desc_key, string _description, string _long_desc_key)
	{
		this.MinLevel = _minLevel;
		this.MaxLevel = _maxLevel;
		this.DescriptionKey = _desc_key;
		this.CustomDescription = _description;
		this.LongDescriptionKey = _long_desc_key;
	}

	// Token: 0x06003636 RID: 13878 RVA: 0x001646AC File Offset: 0x001628AC
	public static EffectGroupDescription ParseDescription(XElement _element)
	{
		if (!_element.HasAttribute("level") || (!_element.HasAttribute("desc_key") && !_element.HasAttribute("desc_base")))
		{
			return null;
		}
		int num2;
		int num;
		if (_element.GetAttribute("level").Contains(","))
		{
			string[] array = _element.GetAttribute("level").Split(',', StringSplitOptions.None);
			if (array.Length < 1)
			{
				return null;
			}
			if (array.Length == 1)
			{
				num = (num2 = StringParsers.ParseSInt32(array[0], 0, -1, NumberStyles.Integer));
			}
			else
			{
				num2 = StringParsers.ParseSInt32(array[0], 0, -1, NumberStyles.Integer);
				num = StringParsers.ParseSInt32(array[1], 0, -1, NumberStyles.Integer);
			}
		}
		else
		{
			num = (num2 = StringParsers.ParseSInt32(_element.GetAttribute("level"), 0, -1, NumberStyles.Integer));
		}
		return new EffectGroupDescription(num2, num, _element.GetAttribute("desc_key"), _element.GetAttribute("desc_base"), _element.HasAttribute("long_desc_key") ? _element.GetAttribute("long_desc_key") : "");
	}

	// Token: 0x04002B83 RID: 11139
	public readonly int MinLevel;

	// Token: 0x04002B84 RID: 11140
	public readonly int MaxLevel;

	// Token: 0x04002B85 RID: 11141
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly string DescriptionKey;

	// Token: 0x04002B86 RID: 11142
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly string CustomDescription;

	// Token: 0x04002B87 RID: 11143
	public readonly string LongDescriptionKey;
}
