using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000454 RID: 1108
[Preserve]
public class EAIRunawayWhenHurt : EAIRunAway
{
	// Token: 0x060021B2 RID: 8626 RVA: 0x000CC075 File Offset: 0x000CA275
	public EAIRunawayWhenHurt()
	{
		this.MutexBits = 1;
	}

	// Token: 0x060021B3 RID: 8627 RVA: 0x000CC090 File Offset: 0x000CA290
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

	// Token: 0x060021B4 RID: 8628 RVA: 0x000CC120 File Offset: 0x000CA320
	public override bool CanExecute()
	{
		this.enemy = this.theEntity.GetRevengeTarget();
		return this.enemy && (this.lowHealthPercent >= 1f || (float)this.theEntity.Health / (float)this.theEntity.GetMaxHealth() < this.lowHealthPercent) && base.CanExecute();
	}

	// Token: 0x060021B5 RID: 8629 RVA: 0x000CC183 File Offset: 0x000CA383
	public override bool Continue()
	{
		return base.Continue();
	}

	// Token: 0x060021B6 RID: 8630 RVA: 0x000CC18B File Offset: 0x000CA38B
	public override void Update()
	{
		base.Update();
		this.theEntity.navigator.setMoveSpeed(this.theEntity.IsInWater() ? this.theEntity.GetMoveSpeed() : this.theEntity.GetMoveSpeedPanic());
	}

	// Token: 0x060021B7 RID: 8631 RVA: 0x000CC1C8 File Offset: 0x000CA3C8
	[PublicizedFrom(EAccessModifier.Protected)]
	public override Vector3 GetFleeFromPos()
	{
		if (this.enemy)
		{
			return this.enemy.position;
		}
		return this.theEntity.position;
	}

	// Token: 0x060021B8 RID: 8632 RVA: 0x000CC1EE File Offset: 0x000CA3EE
	public override string ToString()
	{
		return string.Format("{0}, per {1}", base.ToString(), this.lowHealthPercent);
	}

	// Token: 0x0400174C RID: 5964
	[PublicizedFrom(EAccessModifier.Private)]
	public float lowHealthPercent = 1f;
}
