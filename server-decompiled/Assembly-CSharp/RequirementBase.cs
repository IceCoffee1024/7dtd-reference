using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Linq;

// Token: 0x02000662 RID: 1634
public class RequirementBase : IRequirement
{
	// Token: 0x060034C2 RID: 13506 RVA: 0x0015E58B File Offset: 0x0015C78B
	public virtual bool IsValid(MinEventParams _params)
	{
		if (this.cvarName != null)
		{
			this.value = _params.Self.Buffs.GetCustomVar(this.cvarName);
		}
		return true;
	}

	// Token: 0x060034C3 RID: 13507 RVA: 0x0015E5B2 File Offset: 0x0015C7B2
	public string GetDescription()
	{
		return this.Description;
	}

	// Token: 0x060034C4 RID: 13508 RVA: 0x0015E5BA File Offset: 0x0015C7BA
	public void SetDescription(string desc)
	{
		this.Description = desc;
	}

	// Token: 0x060034C5 RID: 13509 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void GetInfoStrings(ref List<string> list)
	{
	}

	// Token: 0x060034C6 RID: 13510 RVA: 0x0015E5C4 File Offset: 0x0015C7C4
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

	// Token: 0x060034C7 RID: 13511 RVA: 0x0015E670 File Offset: 0x0015C870
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

	// Token: 0x060034C8 RID: 13512 RVA: 0x0015E6F8 File Offset: 0x0015C8F8
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

	// Token: 0x060034C9 RID: 13513 RVA: 0x0015E7D0 File Offset: 0x0015C9D0
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

	// Token: 0x060034CA RID: 13514 RVA: 0x0015E980 File Offset: 0x0015CB80
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

	// Token: 0x04002AF5 RID: 10997
	[PublicizedFrom(EAccessModifier.Protected)]
	public static readonly char[] commaSeparator = new char[] { ',' };

	// Token: 0x04002AF6 RID: 10998
	[PublicizedFrom(EAccessModifier.Protected)]
	public RequirementBase.OperationTypes operation;

	// Token: 0x04002AF7 RID: 10999
	[PublicizedFrom(EAccessModifier.Protected)]
	public string cvarName;

	// Token: 0x04002AF8 RID: 11000
	public bool invert;

	// Token: 0x04002AF9 RID: 11001
	[PublicizedFrom(EAccessModifier.Protected)]
	public float value;

	// Token: 0x04002AFA RID: 11002
	public string Description;

	// Token: 0x02000663 RID: 1635
	[PublicizedFrom(EAccessModifier.Protected)]
	public enum OperationTypes
	{
		// Token: 0x04002AFC RID: 11004
		None,
		// Token: 0x04002AFD RID: 11005
		Equals,
		// Token: 0x04002AFE RID: 11006
		EQ,
		// Token: 0x04002AFF RID: 11007
		E,
		// Token: 0x04002B00 RID: 11008
		NotEquals,
		// Token: 0x04002B01 RID: 11009
		NEQ,
		// Token: 0x04002B02 RID: 11010
		NE,
		// Token: 0x04002B03 RID: 11011
		Less,
		// Token: 0x04002B04 RID: 11012
		LessThan,
		// Token: 0x04002B05 RID: 11013
		LT,
		// Token: 0x04002B06 RID: 11014
		Greater,
		// Token: 0x04002B07 RID: 11015
		GreaterThan,
		// Token: 0x04002B08 RID: 11016
		GT,
		// Token: 0x04002B09 RID: 11017
		LessOrEqual,
		// Token: 0x04002B0A RID: 11018
		LessThanOrEqualTo,
		// Token: 0x04002B0B RID: 11019
		LTE,
		// Token: 0x04002B0C RID: 11020
		GreaterOrEqual,
		// Token: 0x04002B0D RID: 11021
		GreaterThanOrEqualTo,
		// Token: 0x04002B0E RID: 11022
		GTE
	}
}
