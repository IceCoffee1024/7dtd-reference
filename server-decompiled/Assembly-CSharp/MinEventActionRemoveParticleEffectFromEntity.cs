using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006E8 RID: 1768
[Preserve]
public class MinEventActionRemoveParticleEffectFromEntity : MinEventActionTargetedBase
{
	// Token: 0x0600369E RID: 13982 RVA: 0x00168592 File Offset: 0x00166792
	public override void Execute(MinEventParams _params)
	{
		if (_params.Self == null)
		{
			return;
		}
		_params.Self.RemoveParticle(this.particleEffectName);
	}

	// Token: 0x0600369F RID: 13983 RVA: 0x001685B5 File Offset: 0x001667B5
	public override bool CanExecute(MinEventTypes _eventType, MinEventParams _params)
	{
		return base.CanExecute(_eventType, _params) && _params.Self != null && this.particleEffectName != null;
	}

	// Token: 0x060036A0 RID: 13984 RVA: 0x001685DC File Offset: 0x001667DC
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

	// Token: 0x04002BF2 RID: 11250
	[PublicizedFrom(EAccessModifier.Private)]
	public string particleEffectName;
}
