using System;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006DD RID: 1757
[Preserve]
public class MinEventActionAwardChallenge : MinEventActionTargetedBase
{
	// Token: 0x0600360E RID: 13838 RVA: 0x00165998 File Offset: 0x00163B98
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

	// Token: 0x0600360F RID: 13839 RVA: 0x00165A0B File Offset: 0x00163C0B
	public override bool CanExecute(MinEventTypes _eventType, MinEventParams _params)
	{
		return base.CanExecute(_eventType, _params) && (this.cvarRef || this.challengeAwardCount > 0);
	}

	// Token: 0x06003610 RID: 13840 RVA: 0x00165A2C File Offset: 0x00163C2C
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

	// Token: 0x04002B80 RID: 11136
	[PublicizedFrom(EAccessModifier.Private)]
	public string challengeStat;

	// Token: 0x04002B81 RID: 11137
	[PublicizedFrom(EAccessModifier.Private)]
	public int challengeAwardCount = 1;

	// Token: 0x04002B82 RID: 11138
	[PublicizedFrom(EAccessModifier.Private)]
	public bool cvarRef;

	// Token: 0x04002B83 RID: 11139
	[PublicizedFrom(EAccessModifier.Private)]
	public string refCvarName;
}
