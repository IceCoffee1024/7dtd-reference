using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006EB RID: 1771
[Preserve]
public class MinEventActionCallGameEvent : MinEventActionTargetedBase
{
	// Token: 0x0600364E RID: 13902 RVA: 0x00166B1C File Offset: 0x00164D1C
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

	// Token: 0x0600364F RID: 13903 RVA: 0x00166BA4 File Offset: 0x00164DA4
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

	// Token: 0x04002BAA RID: 11178
	[PublicizedFrom(EAccessModifier.Protected)]
	public string eventName = "";

	// Token: 0x04002BAB RID: 11179
	[PublicizedFrom(EAccessModifier.Protected)]
	public string sequenceLink = "";

	// Token: 0x04002BAC RID: 11180
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool allowClientCall;
}
