using System;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x0200070F RID: 1807
[Preserve]
public class MinEventActionModifyScreenEffect : MinEventActionBase
{
	// Token: 0x060036F8 RID: 14072 RVA: 0x0016AF18 File Offset: 0x00169118
	public override void Execute(MinEventParams _params)
	{
		EntityPlayerLocal entityPlayerLocal = _params.Self as EntityPlayerLocal;
		if (entityPlayerLocal != null)
		{
			entityPlayerLocal.ScreenEffectManager.SetScreenEffect(this.effect_name, this.intensity, this.fade);
		}
	}

	// Token: 0x060036F9 RID: 14073 RVA: 0x0016AF57 File Offset: 0x00169157
	public override bool CanExecute(MinEventTypes _eventType, MinEventParams _params)
	{
		return base.CanExecute(_eventType, _params) && _params.Self is EntityPlayerLocal && this.effect_name != "";
	}

	// Token: 0x060036FA RID: 14074 RVA: 0x0016AF84 File Offset: 0x00169184
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

	// Token: 0x04002C7A RID: 11386
	[PublicizedFrom(EAccessModifier.Private)]
	public string effect_name = "";

	// Token: 0x04002C7B RID: 11387
	[PublicizedFrom(EAccessModifier.Private)]
	public float intensity = 1f;

	// Token: 0x04002C7C RID: 11388
	[PublicizedFrom(EAccessModifier.Private)]
	public float fade = 4f;
}
