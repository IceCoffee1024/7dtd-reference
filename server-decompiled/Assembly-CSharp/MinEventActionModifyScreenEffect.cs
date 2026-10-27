using System;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x02000719 RID: 1817
[Preserve]
public class MinEventActionModifyScreenEffect : MinEventActionBase
{
	// Token: 0x06003752 RID: 14162 RVA: 0x0016C814 File Offset: 0x0016AA14
	public override void Execute(MinEventParams _params)
	{
		EntityPlayerLocal entityPlayerLocal = _params.Self as EntityPlayerLocal;
		if (entityPlayerLocal != null)
		{
			entityPlayerLocal.ScreenEffectManager.SetScreenEffect(this.effect_name, this.intensity, this.fade);
		}
	}

	// Token: 0x06003753 RID: 14163 RVA: 0x0016C853 File Offset: 0x0016AA53
	public override bool CanExecute(MinEventTypes _eventType, MinEventParams _params)
	{
		return base.CanExecute(_eventType, _params) && _params.Self is EntityPlayerLocal && this.effect_name != "";
	}

	// Token: 0x06003754 RID: 14164 RVA: 0x0016C880 File Offset: 0x0016AA80
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

	// Token: 0x04002C8F RID: 11407
	[PublicizedFrom(EAccessModifier.Private)]
	public string effect_name = "";

	// Token: 0x04002C90 RID: 11408
	[PublicizedFrom(EAccessModifier.Private)]
	public float intensity = 1f;

	// Token: 0x04002C91 RID: 11409
	[PublicizedFrom(EAccessModifier.Private)]
	public float fade = 4f;
}
