using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200044C RID: 1100
[Preserve]
public class EAILook : EAIBase
{
	// Token: 0x06002167 RID: 8551 RVA: 0x000C99F5 File Offset: 0x000C7BF5
	public EAILook()
	{
		this.MutexBits = 1;
	}

	// Token: 0x06002168 RID: 8552 RVA: 0x000C9A04 File Offset: 0x000C7C04
	public override bool CanExecute()
	{
		return this.manager.lookTime > 0f && !this.theEntity.Jumping;
	}

	// Token: 0x06002169 RID: 8553 RVA: 0x000C9A28 File Offset: 0x000C7C28
	public override void Start()
	{
		this.waitTicks = (int)(this.manager.lookTime * 20f);
		this.manager.lookTime = 0f;
		this.theEntity.GetEntitySenses().Clear();
		this.lookAtTicks = 0;
		this.turnTicks = 0;
		this.theEntity.moveHelper.Stop();
	}

	// Token: 0x0600216A RID: 8554 RVA: 0x000C9A8C File Offset: 0x000C7C8C
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

	// Token: 0x0600216B RID: 8555 RVA: 0x000C9BC7 File Offset: 0x000C7DC7
	public override void Reset()
	{
		this.theEntity.SetLookPosition(Vector3.zero);
	}

	// Token: 0x0600216C RID: 8556 RVA: 0x000C9BD9 File Offset: 0x000C7DD9
	public override string ToString()
	{
		return string.Format("{0}, wait {1}", base.ToString(), ((float)this.waitTicks / 20f).ToCultureInvariantString());
	}

	// Token: 0x04001709 RID: 5897
	[PublicizedFrom(EAccessModifier.Private)]
	public int waitTicks;

	// Token: 0x0400170A RID: 5898
	[PublicizedFrom(EAccessModifier.Private)]
	public int lookAtTicks;

	// Token: 0x0400170B RID: 5899
	[PublicizedFrom(EAccessModifier.Private)]
	public int turnTicks;
}
