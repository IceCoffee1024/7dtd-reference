using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Linq;

// Token: 0x02000659 RID: 1625
public class RequirementBase : IRequirement
{
	// Token: 0x0600346F RID: 13423 RVA: 0x0015C41B File Offset: 0x0015A61B
	public virtual bool IsValid(MinEventParams _params)
	{
		if (this.cvarName != null)
		{
			this.value = _params.Self.Buffs.GetCustomVar(this.cvarName);
		}
		return true;
	}

	// Token: 0x06003470 RID: 13424 RVA: 0x0015C442 File Offset: 0x0015A642
	public string GetDescription()
	{
		return this.Description;
	}

	// Token: 0x06003471 RID: 13425 RVA: 0x0015C44A File Offset: 0x0015A64A
	public void SetDescription(string desc)
	{
		this.Description = desc;
	}

	// Token: 0x06003472 RID: 13426 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void GetInfoStrings(ref List<string> list)
	{
	}

	// Token: 0x06003473 RID: 13427 RVA: 0x0015C454 File Offset: 0x0015A654
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

	// Token: 0x06003474 RID: 13428 RVA: 0x0015C500 File Offset: 0x0015A700
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

	// Token: 0x06003475 RID: 13429 RVA: 0x0015C588 File Offset: 0x0015A788
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

	// Token: 0x06003476 RID: 13430 RVA: 0x0015C660 File Offset: 0x0015A860
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

	// Token: 0x06003477 RID: 13431 RVA: 0x0015C810 File Offset: 0x0015AA10
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

	// Token: 0x04002A81 RID: 10881
	[PublicizedFrom(EAccessModifier.Protected)]
	public static readonly char[] commaSeparator = new char[] { ',' };

	// Token: 0x04002A82 RID: 10882
	[PublicizedFrom(EAccessModifier.Protected)]
	public RequirementBase.OperationTypes operation;

	// Token: 0x04002A83 RID: 10883
	[PublicizedFrom(EAccessModifier.Protected)]
	public string cvarName;

	// Token: 0x04002A84 RID: 10884
	public bool invert;

	// Token: 0x04002A85 RID: 10885
	[PublicizedFrom(EAccessModifier.Protected)]
	public float value;

	// Token: 0x04002A86 RID: 10886
	public string Description;

	// Token: 0x0200065A RID: 1626
	[PublicizedFrom(EAccessModifier.Protected)]
	public enum OperationTypes
	{
		// Token: 0x04002A88 RID: 10888
		None,
		// Token: 0x04002A89 RID: 10889
		Equals,
		// Token: 0x04002A8A RID: 10890
		EQ,
		// Token: 0x04002A8B RID: 10891
		E,
		// Token: 0x04002A8C RID: 10892
		NotEquals,
		// Token: 0x04002A8D RID: 10893
		NEQ,
		// Token: 0x04002A8E RID: 10894
		NE,
		// Token: 0x04002A8F RID: 10895
		Less,
		// Token: 0x04002A90 RID: 10896
		LessThan,
		// Token: 0x04002A91 RID: 10897
		LT,
		// Token: 0x04002A92 RID: 10898
		Greater,
		// Token: 0x04002A93 RID: 10899
		GreaterThan,
		// Token: 0x04002A94 RID: 10900
		GT,
		// Token: 0x04002A95 RID: 10901
		LessOrEqual,
		// Token: 0x04002A96 RID: 10902
		LessThanOrEqualTo,
		// Token: 0x04002A97 RID: 10903
		LTE,
		// Token: 0x04002A98 RID: 10904
		GreaterOrEqual,
		// Token: 0x04002A99 RID: 10905
		GreaterThanOrEqualTo,
		// Token: 0x04002A9A RID: 10906
		GTE
	}
}
