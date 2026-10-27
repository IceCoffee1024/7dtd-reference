using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x02000715 RID: 1813
[Preserve]
public class MinEventActionBase : IMinEventAction
{
	// Token: 0x06003744 RID: 14148 RVA: 0x0016BD4D File Offset: 0x00169F4D
	public virtual void GetInfoStrings(ref List<string> list)
	{
		list.Add(this.EventType.ToStringCached<MinEventTypes>() + ": " + this.ToString());
		RequirementGroup requirements = this.Requirements;
		if (requirements == null)
		{
			return;
		}
		requirements.GetInfoStrings(ref list);
	}

	// Token: 0x06003745 RID: 14149 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void Execute(MinEventParams _params)
	{
	}

	// Token: 0x06003746 RID: 14150 RVA: 0x0016BD82 File Offset: 0x00169F82
	public virtual bool CanExecute(MinEventTypes _eventType, MinEventParams _params)
	{
		return this.Requirements == null || this.Requirements.IsValid(_params);
	}

	// Token: 0x06003747 RID: 14151 RVA: 0x0016BD9C File Offset: 0x00169F9C
	public virtual bool ParseXmlAttribute(XAttribute _attribute)
	{
		string localName = _attribute.Name.LocalName;
		if (localName == "trigger")
		{
			this.EventType = EnumUtils.Parse<MinEventTypes>(_attribute.Value, false);
			return true;
		}
		if (!(localName == "delay"))
		{
			return false;
		}
		this.Delay = float.Parse(_attribute.Value);
		return true;
	}

	// Token: 0x06003748 RID: 14152 RVA: 0x0016BDFC File Offset: 0x00169FFC
	public static MinEventActionBase ParseAction(XElement _element)
	{
		if (!_element.HasAttribute("action"))
		{
			return null;
		}
		Type typeWithPrefix = ReflectionHelpers.GetTypeWithPrefix("MinEventAction", _element.GetAttribute("action"));
		if (typeWithPrefix == null)
		{
			Log.Out("Unable to find class: MinEventAction{0}", new object[] { _element.GetAttribute("action") });
			return null;
		}
		MinEventActionBase minEventActionBase = (MinEventActionBase)Activator.CreateInstance(typeWithPrefix);
		foreach (XAttribute xattribute in _element.Attributes())
		{
			minEventActionBase.ParseXmlAttribute(xattribute);
		}
		minEventActionBase.Requirements = RequirementBase.ParseRequirementGroup(_element);
		minEventActionBase.ParseXMLPostProcess();
		return minEventActionBase;
	}

	// Token: 0x06003749 RID: 14153 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void ParseXMLPostProcess()
	{
	}

	// Token: 0x04002C7B RID: 11387
	public MinEventTypes EventType;

	// Token: 0x04002C7C RID: 11388
	public float Delay;

	// Token: 0x04002C7D RID: 11389
	[PublicizedFrom(EAccessModifier.Protected)]
	public RequirementGroup Requirements;
}
