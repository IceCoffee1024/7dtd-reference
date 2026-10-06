using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x0200071C RID: 1820
[Preserve]
public class MinEventActionShowToolbeltMessage : MinEventActionTargetedBase
{
	// Token: 0x0600375A RID: 14170 RVA: 0x0016C5DC File Offset: 0x0016A7DC
	public override void Execute(MinEventParams _params)
	{
		for (int i = 0; i < this.targets.Count; i++)
		{
			if (this.targets[i] as EntityPlayerLocal != null)
			{
				if (this.sound != null)
				{
					GameManager.ShowTooltip(this.targets[i] as EntityPlayerLocal, this.message, string.Empty, this.sound, null, this.showImmediately, false, 0f);
				}
				else
				{
					GameManager.ShowTooltip(this.targets[i] as EntityPlayerLocal, this.message, this.showImmediately, false, 0f);
				}
			}
		}
	}

	// Token: 0x0600375B RID: 14171 RVA: 0x0016C684 File Offset: 0x0016A884
	public override bool CanExecute(MinEventTypes _eventType, MinEventParams _params)
	{
		return base.CanExecute(_eventType, _params) && this.message != null;
	}

	// Token: 0x0600375C RID: 14172 RVA: 0x0016C69C File Offset: 0x0016A89C
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
			if (localName == "show_immediately")
			{
				if (_attribute.Value != "")
				{
					this.showImmediately = StringParsers.ParseBool(_attribute.Value, 0, -1, true);
				}
				return true;
			}
		}
		return flag;
	}

	// Token: 0x04002CA0 RID: 11424
	[PublicizedFrom(EAccessModifier.Private)]
	public string message;

	// Token: 0x04002CA1 RID: 11425
	[PublicizedFrom(EAccessModifier.Private)]
	public string sound;

	// Token: 0x04002CA2 RID: 11426
	[PublicizedFrom(EAccessModifier.Private)]
	public bool showImmediately;
}
