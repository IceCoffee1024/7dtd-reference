using System;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006E7 RID: 1767
[Preserve]
public class MinEventActionAwardChallenge : MinEventActionTargetedBase
{
	// Token: 0x06003664 RID: 13924 RVA: 0x00167B70 File Offset: 0x00165D70
	public override void Execute(MinEventParams _params)
	{
		if (this.targets == null)
		{
			return;
		}
		for (int i = 0; i < this.targets.Count; i++)
		{
			EntityPlayerLocal entityPlayerLocal = this.targets[i] as EntityPlayerLocal;
			if (entityPlayerLocal != null)
			{
				QuestEventManager.Current.ChallengeAwardCredited(this.challengeStat, (!this.cvarRef) ? this.challengeAwardCount : ((int)entityPlayerLocal.Buffs.GetCustomVar(this.refCvarName)));
			}
		}
	}

	// Token: 0x06003665 RID: 13925 RVA: 0x00167BE3 File Offset: 0x00165DE3
	public override bool CanExecute(MinEventTypes _eventType, MinEventParams _params)
	{
		return base.CanExecute(_eventType, _params) && (this.cvarRef || this.challengeAwardCount > 0);
	}

	// Token: 0x06003666 RID: 13926 RVA: 0x00167C04 File Offset: 0x00165E04
	public override bool ParseXmlAttribute(XAttribute _attribute)
	{
		bool flag = base.ParseXmlAttribute(_attribute);
		if (!flag)
		{
			string localName = _attribute.Name.LocalName;
			if (!(localName == "count"))
			{
				if (localName == "challenge_stat")
				{
					this.challengeStat = _attribute.Value;
				}
			}
			else if (_attribute.Value.StartsWith("@"))
			{
				this.cvarRef = true;
				this.refCvarName = _attribute.Value.Substring(1);
			}
			else
			{
				this.challengeAwardCount = StringParsers.ParseSInt32(_attribute.Value, 0, -1, NumberStyles.Integer);
			}
		}
		return flag;
	}

	// Token: 0x04002BF4 RID: 11252
	[PublicizedFrom(EAccessModifier.Private)]
	public string challengeStat;

	// Token: 0x04002BF5 RID: 11253
	[PublicizedFrom(EAccessModifier.Private)]
	public int challengeAwardCount = 1;

	// Token: 0x04002BF6 RID: 11254
	[PublicizedFrom(EAccessModifier.Private)]
	public bool cvarRef;

	// Token: 0x04002BF7 RID: 11255
	[PublicizedFrom(EAccessModifier.Private)]
	public string refCvarName;
}
