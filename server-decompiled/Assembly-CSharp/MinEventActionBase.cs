using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x0200070D RID: 1805
[Preserve]
public class MinEventActionBase : IMinEventAction
{
	// Token: 0x0600372D RID: 14125 RVA: 0x0016ADB5 File Offset: 0x00168FB5
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

	// Token: 0x0600372E RID: 14126 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void Execute(MinEventParams _params)
	{
	}

	// Token: 0x0600372F RID: 14127 RVA: 0x0016ADEA File Offset: 0x00168FEA
	public virtual bool CanExecute(MinEventTypes _eventType, MinEventParams _params)
	{
		return this.Requirements == null || this.Requirements.IsValid(_params);
	}

	// Token: 0x06003730 RID: 14128 RVA: 0x0016AE04 File Offset: 0x00169004
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

	// Token: 0x06003731 RID: 14129 RVA: 0x0016AE64 File Offset: 0x00169064
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

	// Token: 0x06003732 RID: 14130 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void ParseXMLPostProcess()
	{
	}

	// Token: 0x04002C5F RID: 11359
	public MinEventTypes EventType;

	// Token: 0x04002C60 RID: 11360
	public float Delay;

	// Token: 0x04002C61 RID: 11361
	[PublicizedFrom(EAccessModifier.Protected)]
	public RequirementGroup Requirements;
}
