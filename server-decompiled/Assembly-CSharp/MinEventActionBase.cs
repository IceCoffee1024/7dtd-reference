using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x02000701 RID: 1793
[Preserve]
public class MinEventActionBase : IMinEventAction
{
	// Token: 0x06003694 RID: 13972 RVA: 0x00168279 File Offset: 0x00166479
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

	// Token: 0x06003695 RID: 13973 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void Execute(MinEventParams _params)
	{
	}

	// Token: 0x06003696 RID: 13974 RVA: 0x001682AE File Offset: 0x001664AE
	public virtual bool CanExecute(MinEventTypes _eventType, MinEventParams _params)
	{
		return this.Requirements == null || this.Requirements.IsValid(_params);
	}

	// Token: 0x06003697 RID: 13975 RVA: 0x001682C8 File Offset: 0x001664C8
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

	// Token: 0x06003698 RID: 13976 RVA: 0x00168328 File Offset: 0x00166528
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

	// Token: 0x06003699 RID: 13977 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void ParseXMLPostProcess()
	{
	}

	// Token: 0x04002BF2 RID: 11250
	public MinEventTypes EventType;

	// Token: 0x04002BF3 RID: 11251
	public float Delay;

	// Token: 0x04002BF4 RID: 11252
	[PublicizedFrom(EAccessModifier.Protected)]
	public RequirementGroup Requirements;
}
