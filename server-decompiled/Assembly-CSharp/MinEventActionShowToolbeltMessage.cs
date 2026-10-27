using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x02000724 RID: 1828
[Preserve]
public class MinEventActionShowToolbeltMessage : MinEventActionTargetedBase
{
	// Token: 0x06003771 RID: 14193 RVA: 0x0016D574 File Offset: 0x0016B774
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

	// Token: 0x06003772 RID: 14194 RVA: 0x0016D61C File Offset: 0x0016B81C
	public override bool CanExecute(MinEventTypes _eventType, MinEventParams _params)
	{
		return base.CanExecute(_eventType, _params) && this.message != null;
	}

	// Token: 0x06003773 RID: 14195 RVA: 0x0016D634 File Offset: 0x0016B834
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

	// Token: 0x04002CBC RID: 11452
	[PublicizedFrom(EAccessModifier.Private)]
	public string message;

	// Token: 0x04002CBD RID: 11453
	[PublicizedFrom(EAccessModifier.Private)]
	public string sound;

	// Token: 0x04002CBE RID: 11454
	[PublicizedFrom(EAccessModifier.Private)]
	public bool showImmediately;
}
