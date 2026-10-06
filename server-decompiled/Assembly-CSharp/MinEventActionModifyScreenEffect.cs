using System;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x02000711 RID: 1809
[Preserve]
public class MinEventActionModifyScreenEffect : MinEventActionBase
{
	// Token: 0x0600373B RID: 14139 RVA: 0x0016B87C File Offset: 0x00169A7C
	public override void Execute(MinEventParams _params)
	{
		EntityPlayerLocal entityPlayerLocal = _params.Self as EntityPlayerLocal;
		if (entityPlayerLocal != null)
		{
			entityPlayerLocal.ScreenEffectManager.SetScreenEffect(this.effect_name, this.intensity, this.fade);
		}
	}

	// Token: 0x0600373C RID: 14140 RVA: 0x0016B8BB File Offset: 0x00169ABB
	public override bool CanExecute(MinEventTypes _eventType, MinEventParams _params)
	{
		return base.CanExecute(_eventType, _params) && _params.Self is EntityPlayerLocal && this.effect_name != "";
	}

	// Token: 0x0600373D RID: 14141 RVA: 0x0016B8E8 File Offset: 0x00169AE8
	public override bool ParseXmlAttribute(XAttribute _attribute)
	{
		bool flag = base.ParseXmlAttribute(_attribute);
		if (!flag)
		{
			string localName = _attribute.Name.LocalName;
			if (localName == "effect_name")
			{
				this.effect_name = _attribute.Value;
				return true;
			}
			if (localName == "intensity")
			{
				this.intensity = StringParsers.ParseFloat(_attribute.Value, 0, -1, NumberStyles.Any);
				return true;
			}
			if (localName == "fade")
			{
				this.fade = StringParsers.ParseFloat(_attribute.Value, 0, -1, NumberStyles.Any);
				return true;
			}
		}
		return flag;
	}

	// Token: 0x04002C73 RID: 11379
	[PublicizedFrom(EAccessModifier.Private)]
	public string effect_name = "";

	// Token: 0x04002C74 RID: 11380
	[PublicizedFrom(EAccessModifier.Private)]
	public float intensity = 1f;

	// Token: 0x04002C75 RID: 11381
	[PublicizedFrom(EAccessModifier.Private)]
	public float fade = 4f;
}
