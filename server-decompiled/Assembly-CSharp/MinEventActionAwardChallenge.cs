using System;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006E9 RID: 1769
[Preserve]
public class MinEventActionAwardChallenge : MinEventActionTargetedBase
{
	// Token: 0x060036A7 RID: 13991 RVA: 0x001684D4 File Offset: 0x001666D4
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

	// Token: 0x060036A8 RID: 13992 RVA: 0x00168547 File Offset: 0x00166747
	public override bool CanExecute(MinEventTypes _eventType, MinEventParams _params)
	{
		return base.CanExecute(_eventType, _params) && (this.cvarRef || this.challengeAwardCount > 0);
	}

	// Token: 0x060036A9 RID: 13993 RVA: 0x00168568 File Offset: 0x00166768
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

	// Token: 0x04002BED RID: 11245
	[PublicizedFrom(EAccessModifier.Private)]
	public string challengeStat;

	// Token: 0x04002BEE RID: 11246
	[PublicizedFrom(EAccessModifier.Private)]
	public int challengeAwardCount = 1;

	// Token: 0x04002BEF RID: 11247
	[PublicizedFrom(EAccessModifier.Private)]
	public bool cvarRef;

	// Token: 0x04002BF0 RID: 11248
	[PublicizedFrom(EAccessModifier.Private)]
	public string refCvarName;
}
