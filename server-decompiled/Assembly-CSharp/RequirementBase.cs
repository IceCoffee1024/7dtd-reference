using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Linq;

// Token: 0x02000664 RID: 1636
public class RequirementBase : IRequirement
{
	// Token: 0x06003505 RID: 13573 RVA: 0x0015EEEF File Offset: 0x0015D0EF
	public virtual bool IsValid(MinEventParams _params)
	{
		if (this.cvarName != null)
		{
			this.value = _params.Self.Buffs.GetCustomVar(this.cvarName);
		}
		return true;
	}

	// Token: 0x06003506 RID: 13574 RVA: 0x0015EF16 File Offset: 0x0015D116
	public string GetDescription()
	{
		return this.Description;
	}

	// Token: 0x06003507 RID: 13575 RVA: 0x0015EF1E File Offset: 0x0015D11E
	public void SetDescription(string desc)
	{
		this.Description = desc;
	}

	// Token: 0x06003508 RID: 13576 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void GetInfoStrings(ref List<string> list)
	{
	}

	// Token: 0x06003509 RID: 13577 RVA: 0x0015EF28 File Offset: 0x0015D128
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

	// Token: 0x0600350A RID: 13578 RVA: 0x0015EFD4 File Offset: 0x0015D1D4
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

	// Token: 0x0600350B RID: 13579 RVA: 0x0015F05C File Offset: 0x0015D25C
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

	// Token: 0x0600350C RID: 13580 RVA: 0x0015F134 File Offset: 0x0015D334
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

	// Token: 0x0600350D RID: 13581 RVA: 0x0015F2E4 File Offset: 0x0015D4E4
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

	// Token: 0x04002AEE RID: 10990
	[PublicizedFrom(EAccessModifier.Protected)]
	public static readonly char[] commaSeparator = new char[] { ',' };

	// Token: 0x04002AEF RID: 10991
	[PublicizedFrom(EAccessModifier.Protected)]
	public RequirementBase.OperationTypes operation;

	// Token: 0x04002AF0 RID: 10992
	[PublicizedFrom(EAccessModifier.Protected)]
	public string cvarName;

	// Token: 0x04002AF1 RID: 10993
	public bool invert;

	// Token: 0x04002AF2 RID: 10994
	[PublicizedFrom(EAccessModifier.Protected)]
	public float value;

	// Token: 0x04002AF3 RID: 10995
	public string Description;

	// Token: 0x02000665 RID: 1637
	[PublicizedFrom(EAccessModifier.Protected)]
	public enum OperationTypes
	{
		// Token: 0x04002AF5 RID: 10997
		None,
		// Token: 0x04002AF6 RID: 10998
		Equals,
		// Token: 0x04002AF7 RID: 10999
		EQ,
		// Token: 0x04002AF8 RID: 11000
		E,
		// Token: 0x04002AF9 RID: 11001
		NotEquals,
		// Token: 0x04002AFA RID: 11002
		NEQ,
		// Token: 0x04002AFB RID: 11003
		NE,
		// Token: 0x04002AFC RID: 11004
		Less,
		// Token: 0x04002AFD RID: 11005
		LessThan,
		// Token: 0x04002AFE RID: 11006
		LT,
		// Token: 0x04002AFF RID: 11007
		Greater,
		// Token: 0x04002B00 RID: 11008
		GreaterThan,
		// Token: 0x04002B01 RID: 11009
		GT,
		// Token: 0x04002B02 RID: 11010
		LessOrEqual,
		// Token: 0x04002B03 RID: 11011
		LessThanOrEqualTo,
		// Token: 0x04002B04 RID: 11012
		LTE,
		// Token: 0x04002B05 RID: 11013
		GreaterOrEqual,
		// Token: 0x04002B06 RID: 11014
		GreaterThanOrEqualTo,
		// Token: 0x04002B07 RID: 11015
		GTE
	}
}
