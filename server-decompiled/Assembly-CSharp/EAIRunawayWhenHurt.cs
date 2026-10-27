using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000456 RID: 1110
[Preserve]
public class EAIRunawayWhenHurt : EAIRunAway
{
	// Token: 0x060021B7 RID: 8631 RVA: 0x000CC421 File Offset: 0x000CA621
	public EAIRunawayWhenHurt()
	{
		this.MutexBits = 1;
	}

	// Token: 0x060021B8 RID: 8632 RVA: 0x000CC43C File Offset: 0x000CA63C
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

	// Token: 0x060021B9 RID: 8633 RVA: 0x000CC4CC File Offset: 0x000CA6CC
	public override bool CanExecute()
	{
		this.enemy = this.theEntity.GetRevengeTarget();
		return this.enemy && (this.lowHealthPercent >= 1f || (float)this.theEntity.Health / (float)this.theEntity.GetMaxHealth() < this.lowHealthPercent) && base.CanExecute();
	}

	// Token: 0x060021BA RID: 8634 RVA: 0x000CC52F File Offset: 0x000CA72F
	public override bool Continue()
	{
		return base.Continue();
	}

	// Token: 0x060021BB RID: 8635 RVA: 0x000CC537 File Offset: 0x000CA737
	public override void Update()
	{
		base.Update();
		this.theEntity.navigator.setMoveSpeed(this.theEntity.IsInWater() ? this.theEntity.GetMoveSpeed() : this.theEntity.GetMoveSpeedPanic());
	}

	// Token: 0x060021BC RID: 8636 RVA: 0x000CC574 File Offset: 0x000CA774
	[PublicizedFrom(EAccessModifier.Protected)]
	public override Vector3 GetFleeFromPos()
	{
		if (this.enemy)
		{
			return this.enemy.position;
		}
		return this.theEntity.position;
	}

	// Token: 0x060021BD RID: 8637 RVA: 0x000CC59A File Offset: 0x000CA79A
	public override string ToString()
	{
		return string.Format("{0}, per {1}", base.ToString(), this.lowHealthPercent);
	}

	// Token: 0x0400175C RID: 5980
	[PublicizedFrom(EAccessModifier.Private)]
	public float lowHealthPercent = 1f;
}
