using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200044E RID: 1102
[Preserve]
public class EAIRunawayWhenHurt : EAIRunAway
{
	// Token: 0x060021A6 RID: 8614 RVA: 0x000CB824 File Offset: 0x000C9A24
	public EAIRunawayWhenHurt()
	{
		this.MutexBits = 1;
	}

	// Token: 0x060021A7 RID: 8615 RVA: 0x000CB840 File Offset: 0x000C9A40
	public override void SetData(Dictionary<string, string> data)
	{
		base.SetData(data);
		string text;
		if (data.TryGetValue("runChance", out text))
		{
			this.lowHealthPercent = 0f;
			if (StringParsers.ParseFloat(text, 0, -1, NumberStyles.Any) >= base.RandomFloat)
			{
				base.GetData(data, "healthPer", ref this.lowHealthPercent);
				if (data.TryGetValue("healthPerMax", out text))
				{
					float num = StringParsers.ParseFloat(text, 0, -1, NumberStyles.Any);
					this.lowHealthPercent += base.RandomFloat * (num - this.lowHealthPercent);
				}
			}
		}
	}

	// Token: 0x060021A8 RID: 8616 RVA: 0x000CB8D0 File Offset: 0x000C9AD0
	public override bool CanExecute()
	{
		this.enemy = this.theEntity.GetRevengeTarget();
		return this.enemy && (this.lowHealthPercent >= 1f || (float)this.theEntity.Health / (float)this.theEntity.GetMaxHealth() < this.lowHealthPercent) && base.CanExecute();
	}

	// Token: 0x060021A9 RID: 8617 RVA: 0x000CB933 File Offset: 0x000C9B33
	public override bool Continue()
	{
		return base.Continue();
	}

	// Token: 0x060021AA RID: 8618 RVA: 0x000CB93B File Offset: 0x000C9B3B
	public override void Update()
	{
		base.Update();
		this.theEntity.navigator.setMoveSpeed(this.theEntity.IsInWater() ? this.theEntity.GetMoveSpeed() : this.theEntity.GetMoveSpeedPanic());
	}

	// Token: 0x060021AB RID: 8619 RVA: 0x000CB978 File Offset: 0x000C9B78
	[PublicizedFrom(EAccessModifier.Protected)]
	public override Vector3 GetFleeFromPos()
	{
		if (this.enemy)
		{
			return this.enemy.position;
		}
		return this.theEntity.position;
	}

	// Token: 0x060021AC RID: 8620 RVA: 0x000CB99E File Offset: 0x000C9B9E
	public override string ToString()
	{
		return string.Format("{0}, per {1}", base.ToString(), this.lowHealthPercent);
	}

	// Token: 0x0400173F RID: 5951
	[PublicizedFrom(EAccessModifier.Private)]
	public float lowHealthPercent = 1f;
}
