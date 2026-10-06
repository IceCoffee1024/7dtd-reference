using System;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine;

// Token: 0x020006C3 RID: 1731
public class EffectDisplayValue
{
	// Token: 0x0600362D RID: 13869 RVA: 0x001642CB File Offset: 0x001624CB
	public EffectDisplayValue(string _name, float[] _value, float[] _levels, RequirementGroup _requirements)
	{
		this.Name = _name;
		this.Values = _value;
		this.Levels = _levels;
		this.Requirements = _requirements;
	}

	// Token: 0x0600362E RID: 13870 RVA: 0x001642F0 File Offset: 0x001624F0
	public bool IsValid(MinEventParams _params)
	{
		return this.canRun(_params);
	}

	// Token: 0x0600362F RID: 13871 RVA: 0x001642F9 File Offset: 0x001624F9
	[PublicizedFrom(EAccessModifier.Private)]
	public bool canRun(MinEventParams _params)
	{
		return this.Requirements == null || this.Requirements.IsValid(_params);
	}

	// Token: 0x06003630 RID: 13872 RVA: 0x00164311 File Offset: 0x00162511
	[PublicizedFrom(EAccessModifier.Protected)]
	public static bool InLevelRange(float _level, float _min, float _max)
	{
		return _level >= _min && _level <= _max;
	}

	// Token: 0x06003631 RID: 13873 RVA: 0x00164320 File Offset: 0x00162520
	public float GetValue(int _level)
	{
		if (this.Levels != null)
		{
			if (this.Values != null)
			{
				if (this.Values.Length == this.Levels.Length)
				{
					if (this.Levels.Length >= 2)
					{
						for (int i = 0; i < this.Levels.Length - 1; i += 2)
						{
							if (EffectDisplayValue.InLevelRange((float)_level, this.Levels[i], this.Levels[i + 1]))
							{
								return Mathf.Lerp(this.Values[i], this.Values[i + 1], ((float)_level - this.Levels[i]) / (this.Levels[i + 1] - this.Levels[i]));
							}
						}
					}
					else if (this.Levels.Length >= 1 && (float)_level == this.Levels[0])
					{
						return this.Values[0];
					}
				}
				else if (this.Values.Length == 2 && this.Levels.Length == 1)
				{
					GameRandom tempGameRandom = GameRandomManager.Instance.GetTempGameRandom(MinEventParams.CachedEventParam.Seed);
					if (MinEventParams.CachedEventParam.Seed == 0)
					{
						return (this.Values[0] + this.Values[1]) * 0.5f;
					}
					return tempGameRandom.RandomRange(this.Values[0], this.Values[1]);
				}
				else if (this.Values.Length == 1 && this.Levels.Length == 2 && EffectDisplayValue.InLevelRange((float)_level, this.Levels[0], this.Levels[1]))
				{
					return this.Values[0];
				}
			}
		}
		else if (this.Values != null)
		{
			if (this.Values.Length == 1)
			{
				return this.Values[0];
			}
			if (this.Values.Length == 2)
			{
				return GameRandomManager.Instance.GetTempGameRandom(MinEventParams.CachedEventParam.Seed).RandomRange(this.Values[0], this.Values[1]);
			}
			return this.Values[0];
		}
		return 0f;
	}

	// Token: 0x06003632 RID: 13874 RVA: 0x001644F8 File Offset: 0x001626F8
	public static EffectDisplayValue ParseDisplayValue(XElement _element)
	{
		if (!_element.HasAttribute("name") || !_element.HasAttribute("value"))
		{
			return null;
		}
		RequirementGroup requirementGroup = RequirementBase.ParseRequirementGroup(_element);
		string attribute = _element.GetAttribute("value");
		float[] array = null;
		if (!string.IsNullOrEmpty(attribute))
		{
			if (attribute.Contains(","))
			{
				string[] array2 = attribute.Split(',', StringSplitOptions.None);
				array = new float[array2.Length];
				for (int i = 0; i < array2.Length; i++)
				{
					float num;
					if (StringParsers.TryParseFloat(array2[i], out num))
					{
						array[i] = num;
					}
				}
			}
			else
			{
				array = new float[] { StringParsers.ParseFloat(attribute, 0, -1, NumberStyles.Any) };
			}
		}
		string attribute2 = _element.GetAttribute("tier");
		float[] array3 = null;
		if (!string.IsNullOrEmpty(attribute2))
		{
			if (attribute2.Contains(","))
			{
				string[] array4 = attribute2.Split(',', StringSplitOptions.None);
				array3 = new float[array4.Length];
				for (int j = 0; j < array4.Length; j++)
				{
					array3[j] = StringParsers.ParseFloat(array4[j], 0, -1, NumberStyles.Any);
				}
			}
			else
			{
				array3 = new float[] { StringParsers.ParseFloat(attribute2, 0, -1, NumberStyles.Any) };
			}
		}
		return new EffectDisplayValue(_element.GetAttribute("name"), array, array3, requirementGroup);
	}

	// Token: 0x04002B7E RID: 11134
	public string Name;

	// Token: 0x04002B7F RID: 11135
	public float[] Values;

	// Token: 0x04002B80 RID: 11136
	public float[] Levels;

	// Token: 0x04002B81 RID: 11137
	public RequirementGroup Requirements;

	// Token: 0x04002B82 RID: 11138
	public bool OrCompare;
}
