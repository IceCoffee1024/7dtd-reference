using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006E0 RID: 1760
[Preserve]
public class MinEventActionRemoveParticleEffectFromEntity : MinEventActionTargetedBase
{
	// Token: 0x06003687 RID: 13959 RVA: 0x001677AA File Offset: 0x001659AA
	public override void Execute(MinEventParams _params)
	{
		if (_params.Self == null)
		{
			return;
		}
		_params.Self.RemoveParticle(this.particleEffectName);
	}

	// Token: 0x06003688 RID: 13960 RVA: 0x001677CD File Offset: 0x001659CD
	public override bool CanExecute(MinEventTypes _eventType, MinEventParams _params)
	{
		return base.CanExecute(_eventType, _params) && _params.Self != null && this.particleEffectName != null;
	}

	// Token: 0x06003689 RID: 13961 RVA: 0x001677F4 File Offset: 0x001659F4
	public override bool ParseXmlAttribute(XAttribute _attribute)
	{
		bool flag = base.ParseXmlAttribute(_attribute);
		if (!flag && _attribute.Name.LocalName == "particle")
		{
			this.particleEffectName = "Ptl_" + _attribute.Value;
			return true;
		}
		return flag;
	}

	// Token: 0x04002BD6 RID: 11222
	[PublicizedFrom(EAccessModifier.Private)]
	public string particleEffectName;
}
