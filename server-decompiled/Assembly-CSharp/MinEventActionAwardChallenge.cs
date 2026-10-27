using System;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006F1 RID: 1777
[Preserve]
public class MinEventActionAwardChallenge : MinEventActionTargetedBase
{
	// Token: 0x060036BE RID: 14014 RVA: 0x001692BC File Offset: 0x001674BC
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

	// Token: 0x060036BF RID: 14015 RVA: 0x0016932F File Offset: 0x0016752F
	public override bool CanExecute(MinEventTypes _eventType, MinEventParams _params)
	{
		return base.CanExecute(_eventType, _params) && (this.cvarRef || this.challengeAwardCount > 0);
	}

	// Token: 0x060036C0 RID: 14016 RVA: 0x00169350 File Offset: 0x00167550
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

	// Token: 0x04002C09 RID: 11273
	[PublicizedFrom(EAccessModifier.Private)]
	public string challengeStat;

	// Token: 0x04002C0A RID: 11274
	[PublicizedFrom(EAccessModifier.Private)]
	public int challengeAwardCount = 1;

	// Token: 0x04002C0B RID: 11275
	[PublicizedFrom(EAccessModifier.Private)]
	public bool cvarRef;

	// Token: 0x04002C0C RID: 11276
	[PublicizedFrom(EAccessModifier.Private)]
	public string refCvarName;
}
