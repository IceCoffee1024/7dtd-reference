using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x02000710 RID: 1808
[Preserve]
public class MinEventActionShowToolbeltMessage : MinEventActionTargetedBase
{
	// Token: 0x060036C1 RID: 14017 RVA: 0x00169AA0 File Offset: 0x00167CA0
	public override void Execute(MinEventParams _params)
	{
		for (int i = 0; i < this.targets.Count; i++)
		{
			if (this.targets[i] as EntityPlayerLocal != null)
			{
				if (this.sound != null)
				{
					GameManager.ShowTooltip(this.targets[i] as EntityPlayerLocal, this.message, string.Empty, this.sound, null, false, false, 0f);
				}
				else
				{
					GameManager.ShowTooltip(this.targets[i] as EntityPlayerLocal, this.message, false, false, 0f);
				}
			}
		}
	}

	// Token: 0x060036C2 RID: 14018 RVA: 0x00169B3B File Offset: 0x00167D3B
	public override bool CanExecute(MinEventTypes _eventType, MinEventParams _params)
	{
		return base.CanExecute(_eventType, _params) && this.message != null;
	}

	// Token: 0x060036C3 RID: 14019 RVA: 0x00169B54 File Offset: 0x00167D54
	public override bool ParseXmlAttribute(XAttribute _attribute)
	{
		bool flag = base.ParseXmlAttribute(_attribute);
		if (!flag)
		{
			string localName = _attribute.Name.LocalName;
			if (localName == "message")
			{
				if (this.message == null || this.message == "")
				{
					this.message = _attribute.Value;
				}
				return true;
			}
			if (localName == "message_key")
			{
				if (Localization.Exists(_attribute.Value, false))
				{
					this.message = Localization.Get(_attribute.Value, false, null);
				}
				return true;
			}
			if (localName == "sound")
			{
				if (_attribute.Value != "")
				{
					this.sound = _attribute.Value;
				}
				return true;
			}
		}
		return flag;
	}

	// Token: 0x04002C33 RID: 11315
	[PublicizedFrom(EAccessModifier.Private)]
	public string message;

	// Token: 0x04002C34 RID: 11316
	[PublicizedFrom(EAccessModifier.Private)]
	public string sound;
}
