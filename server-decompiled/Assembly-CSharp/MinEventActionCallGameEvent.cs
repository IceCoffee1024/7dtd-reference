using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006F5 RID: 1781
[Preserve]
public class MinEventActionCallGameEvent : MinEventActionTargetedBase
{
	// Token: 0x060036A4 RID: 13988 RVA: 0x00168CF4 File Offset: 0x00166EF4
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

	// Token: 0x060036A5 RID: 13989 RVA: 0x00168D7C File Offset: 0x00166F7C
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

	// Token: 0x04002C1E RID: 11294
	[PublicizedFrom(EAccessModifier.Protected)]
	public string eventName = "";

	// Token: 0x04002C1F RID: 11295
	[PublicizedFrom(EAccessModifier.Protected)]
	public string sequenceLink = "";

	// Token: 0x04002C20 RID: 11296
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool allowClientCall;
}
