using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000444 RID: 1092
[Preserve]
public class EAILook : EAIBase
{
	// Token: 0x06002156 RID: 8534 RVA: 0x000C8D89 File Offset: 0x000C6F89
	public EAILook()
	{
		this.MutexBits = 1;
	}

	// Token: 0x06002157 RID: 8535 RVA: 0x000C8D98 File Offset: 0x000C6F98
	public override bool CanExecute()
	{
		return this.manager.lookTime > 0f && !this.theEntity.Jumping;
	}

	// Token: 0x06002158 RID: 8536 RVA: 0x000C8DBC File Offset: 0x000C6FBC
	public override void Start()
	{
		this.waitTicks = (int)(this.manager.lookTime * 20f);
		this.manager.lookTime = 0f;
		this.theEntity.GetEntitySenses().Clear();
		this.lookAtTicks = 0;
		this.turnTicks = 0;
		this.theEntity.moveHelper.Stop();
	}

	// Token: 0x06002159 RID: 8537 RVA: 0x000C8E20 File Offset: 0x000C7020
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

	// Token: 0x0600215A RID: 8538 RVA: 0x000C8F5B File Offset: 0x000C715B
	public override void Reset()
	{
		this.theEntity.SetLookPosition(Vector3.zero);
	}

	// Token: 0x0600215B RID: 8539 RVA: 0x000C8F6D File Offset: 0x000C716D
	public override string ToString()
	{
		return string.Format("{0}, wait {1}", base.ToString(), ((float)this.waitTicks / 20f).ToCultureInvariantString());
	}

	// Token: 0x040016ED RID: 5869
	[PublicizedFrom(EAccessModifier.Private)]
	public int waitTicks;

	// Token: 0x040016EE RID: 5870
	[PublicizedFrom(EAccessModifier.Private)]
	public int lookAtTicks;

	// Token: 0x040016EF RID: 5871
	[PublicizedFrom(EAccessModifier.Private)]
	public int turnTicks;
}
