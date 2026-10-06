using System;
using UnityEngine;

// Token: 0x020000CE RID: 206
public class AvatarControllerDummy : LegacyAvatarController
{
	// Token: 0x060004BC RID: 1212 RVA: 0x0002113C File Offset: 0x0001F33C
	public override void SwitchModelAndView(string _modelName, bool _bFPV, bool _bMale)
	{
		if (this.modelTransform != null)
		{
			this.bipedTransform = this.modelTransform.Find(_modelName + (_bFPV ? "_FP" : ""));
			if (this.bipedTransform != null && this.entity != null)
			{
				this.rightHand = this.bipedTransform.FindInChilds(this.entity.GetRightHandTransformName(), false);
				base.SetAnimator(this.bipedTransform);
			}
		}
	}

	// Token: 0x060004BD RID: 1213 RVA: 0x000211C2 File Offset: 0x0001F3C2
	public override Transform GetRightHandTransform()
	{
		return this.rightHand;
	}

	// Token: 0x060004BE RID: 1214 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void assignStates()
	{
	}

	// Token: 0x060004BF RID: 1215 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void updateSpineRotation()
	{
	}

	// Token: 0x060004C0 RID: 1216 RVA: 0x000211CA File Offset: 0x0001F3CA
	public override bool IsAnimationSpecialAttackPlaying()
	{
		return this.bSpecialAttackPlaying;
	}

	// Token: 0x060004C1 RID: 1217 RVA: 0x000211D2 File Offset: 0x0001F3D2
	public override void StartAnimationSpecialAttack(bool _b, int _animType)
	{
		this.idleTime = 0f;
		this.bSpecialAttackPlaying = _b;
	}

	// Token: 0x060004C2 RID: 1218 RVA: 0x000211E6 File Offset: 0x0001F3E6
	public override bool IsAnimationSpecialAttack2Playing()
	{
		return this.timeSpecialAttack2Playing > 0f;
	}

	// Token: 0x060004C3 RID: 1219 RVA: 0x000211F5 File Offset: 0x0001F3F5
	public override void StartAnimationSpecialAttack2()
	{
		this.idleTime = 0f;
		this.timeSpecialAttack2Playing = 0.3f;
	}

	// Token: 0x060004C4 RID: 1220 RVA: 0x0002120D File Offset: 0x0001F40D
	public override bool IsAnimationRagingPlaying()
	{
		return this.timeRagePlaying > 0f;
	}

	// Token: 0x060004C5 RID: 1221 RVA: 0x0002121C File Offset: 0x0001F41C
	public override void StartAnimationRaging()
	{
		this.idleTime = 0f;
		this.ragingTicks = 3;
		this.timeRagePlaying = 0.3f;
	}

	// Token: 0x060004C6 RID: 1222 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsAnimationWithMotionRunning()
	{
		return true;
	}

	// Token: 0x060004C7 RID: 1223 RVA: 0x0002123C File Offset: 0x0001F43C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Update()
	{
		base.Update();
		if (this.timeAttackAnimationPlaying > 0f)
		{
			this.timeAttackAnimationPlaying -= Time.deltaTime;
		}
		if (this.timeUseAnimationPlaying > 0f)
		{
			this.timeUseAnimationPlaying -= Time.deltaTime;
		}
		if (this.timeRagePlaying > 0f)
		{
			this.timeRagePlaying -= Time.deltaTime;
		}
		if (this.timeSpecialAttack2Playing > 0f)
		{
			this.timeSpecialAttack2Playing -= Time.deltaTime;
		}
	}

	// Token: 0x04000555 RID: 1365
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public new bool bSpecialAttackPlaying;

	// Token: 0x04000556 RID: 1366
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public new float timeSpecialAttack2Playing;

	// Token: 0x04000557 RID: 1367
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float timeRagePlaying;

	// Token: 0x04000558 RID: 1368
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int ragingTicks;
}
