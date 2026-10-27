using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Linq;

// Token: 0x0200066B RID: 1643
public class RequirementBase : IRequirement
{
	// Token: 0x06003513 RID: 13587 RVA: 0x0015FAE3 File Offset: 0x0015DCE3
	public virtual bool IsValid(MinEventParams _params)
	{
		if (this.cvarName != null)
		{
			this.value = _params.Self.Buffs.GetCustomVar(this.cvarName);
		}
		return true;
	}

	// Token: 0x06003514 RID: 13588 RVA: 0x0015FB0A File Offset: 0x0015DD0A
	public string GetDescription()
	{
		return this.Description;
	}

	// Token: 0x06003515 RID: 13589 RVA: 0x0015FB12 File Offset: 0x0015DD12
	public void SetDescription(string desc)
	{
		this.Description = desc;
	}

	// Token: 0x06003516 RID: 13590 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void GetInfoStrings(ref List<string> list)
	{
	}

	// Token: 0x06003517 RID: 13591 RVA: 0x0015FB1C File Offset: 0x0015DD1C
	public virtual bool ParseXAttribute(XAttribute _attribute)
	{
		string localName = _attribute.Name.LocalName;
		if (localName == "operation")
		{
			this.operation = EnumUtils.Parse<RequirementBase.OperationTypes>(_attribute.Value, true);
			return true;
		}
		if (localName == "value")
		{
			string text = _attribute.Value;
			if (text.Length > 0)
			{
				if (text[0] == '@')
				{
					this.cvarName = text.Substring(1);
				}
				else
				{
					this.value = StringParsers.ParseFloat(text, 0, -1, NumberStyles.Any);
				}
			}
			return true;
		}
		if (!(localName == "invert"))
		{
			return false;
		}
		this.invert = StringParsers.ParseBool(_attribute.Value, 0, -1, true);
		return true;
	}

	// Token: 0x06003518 RID: 13592 RVA: 0x0015FBC8 File Offset: 0x0015DDC8
	[PublicizedFrom(EAccessModifier.Protected)]
	public static bool compareValues(float _valueA, RequirementBase.OperationTypes _operation, float _valueB)
	{
		switch (_operation)
		{
		case RequirementBase.OperationTypes.Equals:
		case RequirementBase.OperationTypes.EQ:
		case RequirementBase.OperationTypes.E:
			return _valueA == _valueB;
		case RequirementBase.OperationTypes.NotEquals:
		case RequirementBase.OperationTypes.NEQ:
		case RequirementBase.OperationTypes.NE:
			return _valueA != _valueB;
		case RequirementBase.OperationTypes.Less:
		case RequirementBase.OperationTypes.LessThan:
		case RequirementBase.OperationTypes.LT:
			return _valueA < _valueB;
		case RequirementBase.OperationTypes.Greater:
		case RequirementBase.OperationTypes.GreaterThan:
		case RequirementBase.OperationTypes.GT:
			return _valueA > _valueB;
		case RequirementBase.OperationTypes.LessOrEqual:
		case RequirementBase.OperationTypes.LessThanOrEqualTo:
		case RequirementBase.OperationTypes.LTE:
			return _valueA <= _valueB;
		case RequirementBase.OperationTypes.GreaterOrEqual:
		case RequirementBase.OperationTypes.GreaterThanOrEqualTo:
		case RequirementBase.OperationTypes.GTE:
			return _valueA >= _valueB;
		default:
			return false;
		}
	}

	// Token: 0x06003519 RID: 13593 RVA: 0x0015FC50 File Offset: 0x0015DE50
	public static IRequirement ParseRequirement(XElement _element)
	{
		string text = _element.GetAttribute("name");
		if (text.Length == 0)
		{
			return null;
		}
		bool flag = text[0] == '!';
		if (flag)
		{
			text = text.Substring(1);
		}
		Type type = Type.GetType(text);
		if (type == null)
		{
			return null;
		}
		RequirementBase requirementBase = (RequirementBase)Activator.CreateInstance(type);
		requirementBase.invert = flag;
		string attribute = _element.GetAttribute("desc_key");
		if (attribute.Length > 0)
		{
			requirementBase.SetDescription(Localization.Get(attribute, false, null));
		}
		foreach (XAttribute xattribute in _element.Attributes())
		{
			requirementBase.ParseXAttribute(xattribute);
		}
		return requirementBase;
	}

	// Token: 0x0600351A RID: 13594 RVA: 0x0015FD28 File Offset: 0x0015DF28
	public static RequirementGroup ParseRequirementGroup(XElement _element)
	{
		RequirementGroup.Op op = RequirementGroup.Op.And;
		if (_element.GetAttribute("op").EqualsCaseInsensitive("or"))
		{
			op = RequirementGroup.Op.Or;
		}
		else if (_element.GetAttribute("compare_type").EqualsCaseInsensitive("or"))
		{
			op = RequirementGroup.Op.Or;
		}
		List<IRequirement> list = null;
		foreach (XElement xelement in _element.Elements("requirement"))
		{
			IRequirement requirement = RequirementBase.ParseRequirement(xelement);
			if (requirement != null)
			{
				if (list == null)
				{
					list = new List<IRequirement>();
				}
				list.Add(requirement);
			}
		}
		List<RequirementGroup> list2 = null;
		foreach (XElement xelement2 in _element.Elements("requirement_group"))
		{
			RequirementGroup requirementGroup = RequirementBase.ParseRequirementGroup(xelement2);
			if (requirementGroup != null)
			{
				if (list2 == null)
				{
					list2 = new List<RequirementGroup>();
				}
				list2.Add(requirementGroup);
			}
		}
		foreach (XElement xelement3 in _element.Elements("requirements"))
		{
			RequirementGroup requirementGroup2 = RequirementBase.ParseRequirementGroup(xelement3);
			if (requirementGroup2 != null)
			{
				if (list2 == null)
				{
					list2 = new List<RequirementGroup>();
				}
				list2.Add(requirementGroup2);
			}
		}
		RequirementGroup requirementGroup3 = null;
		bool flag = list != null;
		bool flag2 = list2 != null;
		if (!flag && flag2 && list2.Count == 1)
		{
			requirementGroup3 = list2[0];
		}
		else if (flag || flag2)
		{
			requirementGroup3 = new RequirementGroup(op, list, list2);
		}
		return requirementGroup3;
	}

	// Token: 0x0600351B RID: 13595 RVA: 0x0015FED8 File Offset: 0x0015E0D8
	public string GetInfoString()
	{
		if (this.Description != null)
		{
			return this.Description;
		}
		List<string> list = new List<string>();
		this.GetInfoStrings(ref list);
		if (list.Count > 0)
		{
			return list[0];
		}
		return null;
	}

	// Token: 0x04002B09 RID: 11017
	[PublicizedFrom(EAccessModifier.Protected)]
	public static readonly char[] commaSeparator = new char[] { ',' };

	// Token: 0x04002B0A RID: 11018
	[PublicizedFrom(EAccessModifier.Protected)]
	public RequirementBase.OperationTypes operation;

	// Token: 0x04002B0B RID: 11019
	[PublicizedFrom(EAccessModifier.Protected)]
	public string cvarName;

	// Token: 0x04002B0C RID: 11020
	public bool invert;

	// Token: 0x04002B0D RID: 11021
	[PublicizedFrom(EAccessModifier.Protected)]
	public float value;

	// Token: 0x04002B0E RID: 11022
	public string Description;

	// Token: 0x0200066C RID: 1644
	[PublicizedFrom(EAccessModifier.Protected)]
	public enum OperationTypes
	{
		// Token: 0x04002B10 RID: 11024
		None,
		// Token: 0x04002B11 RID: 11025
		Equals,
		// Token: 0x04002B12 RID: 11026
		EQ,
		// Token: 0x04002B13 RID: 11027
		E,
		// Token: 0x04002B14 RID: 11028
		NotEquals,
		// Token: 0x04002B15 RID: 11029
		NEQ,
		// Token: 0x04002B16 RID: 11030
		NE,
		// Token: 0x04002B17 RID: 11031
		Less,
		// Token: 0x04002B18 RID: 11032
		LessThan,
		// Token: 0x04002B19 RID: 11033
		LT,
		// Token: 0x04002B1A RID: 11034
		Greater,
		// Token: 0x04002B1B RID: 11035
		GreaterThan,
		// Token: 0x04002B1C RID: 11036
		GT,
		// Token: 0x04002B1D RID: 11037
		LessOrEqual,
		// Token: 0x04002B1E RID: 11038
		LessThanOrEqualTo,
		// Token: 0x04002B1F RID: 11039
		LTE,
		// Token: 0x04002B20 RID: 11040
		GreaterOrEqual,
		// Token: 0x04002B21 RID: 11041
		GreaterThanOrEqualTo,
		// Token: 0x04002B22 RID: 11042
		GTE
	}
}
