using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200044A RID: 1098
[Preserve]
public class EAILook : EAIBase
{
	// Token: 0x06002162 RID: 8546 RVA: 0x000C9649 File Offset: 0x000C7849
	public EAILook()
	{
		this.MutexBits = 1;
	}

	// Token: 0x06002163 RID: 8547 RVA: 0x000C9658 File Offset: 0x000C7858
	public override bool CanExecute()
	{
		return this.manager.lookTime > 0f && !this.theEntity.Jumping;
	}

	// Token: 0x06002164 RID: 8548 RVA: 0x000C967C File Offset: 0x000C787C
	public override void Start()
	{
		this.waitTicks = (int)(this.manager.lookTime * 20f);
		this.manager.lookTime = 0f;
		this.theEntity.GetEntitySenses().Clear();
		this.lookAtTicks = 0;
		this.turnTicks = 0;
		this.theEntity.moveHelper.Stop();
	}

	// Token: 0x06002165 RID: 8549 RVA: 0x000C96E0 File Offset: 0x000C78E0
	public override bool Continue()
	{
		if (this.theEntity.bodyDamage.CurrentStun != EnumEntityStunType.None)
		{
			return false;
		}
		int num;
		if (this.theEntity.IsAlert)
		{
			this.waitTicks--;
			this.lookAtTicks -= 2;
			num = this.turnTicks - 1;
			this.turnTicks = num;
			if (num <= 0)
			{
				this.turnTicks = 14;
				this.theEntity.SeekYaw(this.theEntity.rotation.y + (base.RandomFloat * 120f - 60f), 0f, 35f);
			}
		}
		num = this.waitTicks - 1;
		this.waitTicks = num;
		if (num <= 0)
		{
			return false;
		}
		num = this.lookAtTicks - 1;
		this.lookAtTicks = num;
		if (num <= 0)
		{
			this.lookAtTicks = 40;
			Vector3 headPosition = this.theEntity.getHeadPosition();
			Vector3 vector = this.theEntity.GetForwardVector() * 20f;
			vector = Quaternion.Euler(base.RandomFloat * 60f - 30f, base.RandomFloat * 120f - 60f, 0f) * vector;
			this.theEntity.SetLookPosition(headPosition + vector);
		}
		return true;
	}

	// Token: 0x06002166 RID: 8550 RVA: 0x000C981B File Offset: 0x000C7A1B
	public override void Reset()
	{
		this.theEntity.SetLookPosition(Vector3.zero);
	}

	// Token: 0x06002167 RID: 8551 RVA: 0x000C982D File Offset: 0x000C7A2D
	public override string ToString()
	{
		return string.Format("{0}, wait {1}", base.ToString(), ((float)this.waitTicks / 20f).ToCultureInvariantString());
	}

	// Token: 0x040016F9 RID: 5881
	[PublicizedFrom(EAccessModifier.Private)]
	public int waitTicks;

	// Token: 0x040016FA RID: 5882
	[PublicizedFrom(EAccessModifier.Private)]
	public int lookAtTicks;

	// Token: 0x040016FB RID: 5883
	[PublicizedFrom(EAccessModifier.Private)]
	public int turnTicks;
}
