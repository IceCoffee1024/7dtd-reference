using System;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x02000705 RID: 1797
[Preserve]
public class MinEventActionModifyScreenEffect : MinEventActionBase
{
	// Token: 0x060036A2 RID: 13986 RVA: 0x00168D40 File Offset: 0x00166F40
	public override void Execute(MinEventParams _params)
	{
		EntityPlayerLocal entityPlayerLocal = _params.Self as EntityPlayerLocal;
		if (entityPlayerLocal != null)
		{
			entityPlayerLocal.ScreenEffectManager.SetScreenEffect(this.effect_name, this.intensity, this.fade);
		}
	}

	// Token: 0x060036A3 RID: 13987 RVA: 0x00168D7F File Offset: 0x00166F7F
	public override bool CanExecute(MinEventTypes _eventType, MinEventParams _params)
	{
		return base.CanExecute(_eventType, _params) && _params.Self is EntityPlayerLocal && this.effect_name != "";
	}

	// Token: 0x060036A4 RID: 13988 RVA: 0x00168DAC File Offset: 0x00166FAC
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

	// Token: 0x04002C06 RID: 11270
	[PublicizedFrom(EAccessModifier.Private)]
	public string effect_name = "";

	// Token: 0x04002C07 RID: 11271
	[PublicizedFrom(EAccessModifier.Private)]
	public float intensity = 1f;

	// Token: 0x04002C08 RID: 11272
	[PublicizedFrom(EAccessModifier.Private)]
	public float fade = 4f;
}
