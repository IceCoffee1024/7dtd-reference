using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006DE RID: 1758
[Preserve]
public class MinEventActionRemoveParticleEffectFromEntity : MinEventActionTargetedBase
{
	// Token: 0x06003644 RID: 13892 RVA: 0x00166E3A File Offset: 0x0016503A
	public override void Execute(MinEventParams _params)
	{
		if (_params.Self == null)
		{
			return;
		}
		_params.Self.RemoveParticle(this.particleEffectName);
	}

	// Token: 0x06003645 RID: 13893 RVA: 0x00166E5D File Offset: 0x0016505D
	public override bool CanExecute(MinEventTypes _eventType, MinEventParams _params)
	{
		return base.CanExecute(_eventType, _params) && _params.Self != null && this.particleEffectName != null;
	}

	// Token: 0x06003646 RID: 13894 RVA: 0x00166E84 File Offset: 0x00165084
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

	// Token: 0x04002BDD RID: 11229
	[PublicizedFrom(EAccessModifier.Private)]
	public string particleEffectName;
}
