using System;
using System.Globalization;
using System.Xml.Linq;

// Token: 0x020006B8 RID: 1720
public class EffectGroupDescription
{
	// Token: 0x17000555 RID: 1365
	// (get) Token: 0x0600359A RID: 13722 RVA: 0x00161B02 File Offset: 0x0015FD02
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

	// Token: 0x17000556 RID: 1366
	// (get) Token: 0x0600359B RID: 13723 RVA: 0x00161B26 File Offset: 0x0015FD26
	public string LongDescription
	{
		get
		{
			return Localization.Get(this.LongDescriptionKey, false, null);
		}
	}

	// Token: 0x0600359C RID: 13724 RVA: 0x00161B35 File Offset: 0x0015FD35
	public EffectGroupDescription(int _minLevel, int _maxLevel, string _desc_key, string _description, string _long_desc_key)
	{
		this.MinLevel = _minLevel;
		this.MaxLevel = _maxLevel;
		this.DescriptionKey = _desc_key;
		this.CustomDescription = _description;
		this.LongDescriptionKey = _long_desc_key;
	}

	// Token: 0x0600359D RID: 13725 RVA: 0x00161B64 File Offset: 0x0015FD64
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

	// Token: 0x04002B16 RID: 11030
	public readonly int MinLevel;

	// Token: 0x04002B17 RID: 11031
	public readonly int MaxLevel;

	// Token: 0x04002B18 RID: 11032
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly string DescriptionKey;

	// Token: 0x04002B19 RID: 11033
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly string CustomDescription;

	// Token: 0x04002B1A RID: 11034
	public readonly string LongDescriptionKey;
}
