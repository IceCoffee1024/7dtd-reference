using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006D4 RID: 1748
[Preserve]
public class MinEventActionRemoveParticleEffectFromEntity : MinEventActionTargetedBase
{
	// Token: 0x060035EE RID: 13806 RVA: 0x00164C62 File Offset: 0x00162E62
	public override void Execute(MinEventParams _params)
	{
		if (_params.Self == null)
		{
			return;
		}
		_params.Self.RemoveParticle(this.particleEffectName);
	}

	// Token: 0x060035EF RID: 13807 RVA: 0x00164C85 File Offset: 0x00162E85
	public override bool CanExecute(MinEventTypes _eventType, MinEventParams _params)
	{
		return base.CanExecute(_eventType, _params) && _params.Self != null && this.particleEffectName != null;
	}

	// Token: 0x060035F0 RID: 13808 RVA: 0x00164CAC File Offset: 0x00162EAC
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

	// Token: 0x04002B69 RID: 11113
	[PublicizedFrom(EAccessModifier.Private)]
	public string particleEffectName;
}
