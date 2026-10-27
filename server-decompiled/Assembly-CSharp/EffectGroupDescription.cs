using System;
using System.Globalization;
using System.Xml.Linq;

// Token: 0x020006CB RID: 1739
public class EffectGroupDescription
{
	// Token: 0x17000584 RID: 1412
	// (get) Token: 0x06003641 RID: 13889 RVA: 0x0016523E File Offset: 0x0016343E
	public string GetDescriptionKey
	{
		get
		{
			return this.DescriptionKey;
		}
	}

	// Token: 0x17000585 RID: 1413
	// (get) Token: 0x06003642 RID: 13890 RVA: 0x00165246 File Offset: 0x00163446
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

	// Token: 0x17000586 RID: 1414
	// (get) Token: 0x06003643 RID: 13891 RVA: 0x0016526A File Offset: 0x0016346A
	public string LongDescription
	{
		get
		{
			return Localization.Get(this.LongDescriptionKey, false, null);
		}
	}

	// Token: 0x06003644 RID: 13892 RVA: 0x00165279 File Offset: 0x00163479
	public EffectGroupDescription(int _minLevel, int _maxLevel, string _desc_key, string _description, string _long_desc_key)
	{
		this.MinLevel = _minLevel;
		this.MaxLevel = _maxLevel;
		this.DescriptionKey = _desc_key;
		this.CustomDescription = _description;
		this.LongDescriptionKey = _long_desc_key;
	}

	// Token: 0x06003645 RID: 13893 RVA: 0x001652A8 File Offset: 0x001634A8
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

	// Token: 0x04002B9E RID: 11166
	public readonly int MinLevel;

	// Token: 0x04002B9F RID: 11167
	public readonly int MaxLevel;

	// Token: 0x04002BA0 RID: 11168
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly string DescriptionKey;

	// Token: 0x04002BA1 RID: 11169
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly string CustomDescription;

	// Token: 0x04002BA2 RID: 11170
	public readonly string LongDescriptionKey;
}
