using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x0200071A RID: 1818
[Preserve]
public class MinEventActionShowToolbeltMessage : MinEventActionTargetedBase
{
	// Token: 0x06003717 RID: 14103 RVA: 0x0016BC78 File Offset: 0x00169E78
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

	// Token: 0x06003718 RID: 14104 RVA: 0x0016BD20 File Offset: 0x00169F20
	public override bool CanExecute(MinEventTypes _eventType, MinEventParams _params)
	{
		return base.CanExecute(_eventType, _params) && this.message != null;
	}

	// Token: 0x06003719 RID: 14105 RVA: 0x0016BD38 File Offset: 0x00169F38
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

	// Token: 0x04002CA7 RID: 11431
	[PublicizedFrom(EAccessModifier.Private)]
	public string message;

	// Token: 0x04002CA8 RID: 11432
	[PublicizedFrom(EAccessModifier.Private)]
	public string sound;

	// Token: 0x04002CA9 RID: 11433
	[PublicizedFrom(EAccessModifier.Private)]
	public bool showImmediately;
}
