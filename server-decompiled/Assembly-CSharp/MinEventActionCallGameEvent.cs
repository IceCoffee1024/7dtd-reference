using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006F7 RID: 1783
[Preserve]
public class MinEventActionCallGameEvent : MinEventActionTargetedBase
{
	// Token: 0x060036E7 RID: 14055 RVA: 0x00169658 File Offset: 0x00167858
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

	// Token: 0x060036E8 RID: 14056 RVA: 0x001696E0 File Offset: 0x001678E0
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

	// Token: 0x04002C17 RID: 11287
	[PublicizedFrom(EAccessModifier.Protected)]
	public string eventName = "";

	// Token: 0x04002C18 RID: 11288
	[PublicizedFrom(EAccessModifier.Protected)]
	public string sequenceLink = "";

	// Token: 0x04002C19 RID: 11289
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool allowClientCall;
}
