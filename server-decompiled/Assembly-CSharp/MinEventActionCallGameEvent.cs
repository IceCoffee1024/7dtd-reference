using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006FF RID: 1791
[Preserve]
public class MinEventActionCallGameEvent : MinEventActionTargetedBase
{
	// Token: 0x060036FE RID: 14078 RVA: 0x0016A5F0 File Offset: 0x001687F0
	public override void Execute(MinEventParams _params)
	{
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer || this.allowClientCall)
		{
			for (int i = 0; i < this.targets.Count; i++)
			{
				if (this.targets[i] != null)
				{
					GameEventManager.Current.HandleAction(this.eventName, _params.Self as EntityPlayer, this.targets[i], false, "", "", false, true, this.sequenceLink, null);
				}
			}
		}
	}

	// Token: 0x060036FF RID: 14079 RVA: 0x0016A678 File Offset: 0x00168878
	public override bool ParseXmlAttribute(XAttribute _attribute)
	{
		bool flag = base.ParseXmlAttribute(_attribute);
		if (!flag)
		{
			string localName = _attribute.Name.LocalName;
			if (!(localName == "event"))
			{
				if (!(localName == "sequence_link"))
				{
					if (localName == "allow_client_call")
					{
						this.allowClientCall = StringParsers.ParseBool(_attribute.Value, 0, -1, true);
					}
				}
				else
				{
					this.sequenceLink = _attribute.Value;
				}
			}
			else
			{
				this.eventName = _attribute.Value;
			}
		}
		return flag;
	}

	// Token: 0x04002C33 RID: 11315
	[PublicizedFrom(EAccessModifier.Protected)]
	public string eventName = "";

	// Token: 0x04002C34 RID: 11316
	[PublicizedFrom(EAccessModifier.Protected)]
	public string sequenceLink = "";

	// Token: 0x04002C35 RID: 11317
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool allowClientCall;
}
