using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x0200070B RID: 1803
[Preserve]
public class MinEventActionBase : IMinEventAction
{
	// Token: 0x060036EA RID: 14058 RVA: 0x0016A451 File Offset: 0x00168651
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

	// Token: 0x060036EB RID: 14059 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void Execute(MinEventParams _params)
	{
	}

	// Token: 0x060036EC RID: 14060 RVA: 0x0016A486 File Offset: 0x00168686
	public virtual bool CanExecute(MinEventTypes _eventType, MinEventParams _params)
	{
		return this.Requirements == null || this.Requirements.IsValid(_params);
	}

	// Token: 0x060036ED RID: 14061 RVA: 0x0016A4A0 File Offset: 0x001686A0
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

	// Token: 0x060036EE RID: 14062 RVA: 0x0016A500 File Offset: 0x00168700
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

	// Token: 0x060036EF RID: 14063 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void ParseXMLPostProcess()
	{
	}

	// Token: 0x04002C66 RID: 11366
	public MinEventTypes EventType;

	// Token: 0x04002C67 RID: 11367
	public float Delay;

	// Token: 0x04002C68 RID: 11368
	[PublicizedFrom(EAccessModifier.Protected)]
	public RequirementGroup Requirements;
}
