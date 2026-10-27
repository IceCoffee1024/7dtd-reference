using System;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200049A RID: 1178
[Preserve]
public abstract class EntityContainerAbs : EntityItem
{
	// Token: 0x0600243C RID: 9276 RVA: 0x000DD658 File Offset: 0x000DB858
	public override void CopyPropertiesFromEntityClass()
	{
		base.CopyPropertiesFromEntityClass();
		EntityClass entityClass = EntityClass.list[this.entityClass];
		float num = 5f;
		entityClass.Properties.ParseFloat(EntityClass.PropTimeStayAfterDeath, ref num);
		this.timeStayAfterDeath = (int)(num * 20f);
	}

	// Token: 0x0600243D RID: 9277 RVA: 0x000DD6A0 File Offset: 0x000DB8A0
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Start()
	{
		base.Start();
		foreach (Collider collider in base.transform.GetComponentsInChildren<Collider>())
		{
			collider.gameObject.tag = "E_BP_Body";
			collider.gameObject.layer = 13;
			collider.enabled = true;
			collider.gameObject.AddMissingComponent<RootTransformRefEntity>().RootTransform = base.transform;
		}
		this.SetDead();
	}

	// Token: 0x0600243E RID: 9278 RVA: 0x000DD710 File Offset: 0x000DB910
	public override void OnUpdateEntity()
	{
		base.OnUpdateEntity();
		using (this.pmEntityType.Auto())
		{
			if (!(SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer ? LockManager.Instance.IsLockedServer(this, 0) : LockManager.Instance.IsLockedByLocalPlayer(this, 0)))
			{
				if (this.bag.Touched && this.bag.IsEmpty())
				{
					this.removeBackpack("empty");
				}
				else if (this.deathUpdateTime >= this.timeStayAfterDeath - 1)
				{
					this.removeBackpack("old");
				}
				else
				{
					this.deathUpdateTime++;
					this.updatePosition();
					this.updateMarkers();
				}
			}
		}
	}

	// Token: 0x0600243F RID: 9279 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void updatePosition()
	{
	}

	// Token: 0x06002440 RID: 9280 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void updateMarkers()
	{
	}

	// Token: 0x06002441 RID: 9281 RVA: 0x000DD7D8 File Offset: 0x000DB9D8
	public override bool AllowActivationCommand(ReadOnlySpan<char> _commandName, EntityPlayerLocal _playerFocusing)
	{
		if (base.CommandIs(_commandName, "search"))
		{
			return this.bag != null;
		}
		return base.AllowActivationCommand(_commandName, _playerFocusing);
	}

	// Token: 0x06002442 RID: 9282 RVA: 0x000DD7FA File Offset: 0x000DB9FA
	[PublicizedFrom(EAccessModifier.Protected)]
	public void removeBackpack(string reason)
	{
		this.deathUpdateTime = this.timeStayAfterDeath;
		this.onBackpackRemoved(reason);
		this.bRemoved = true;
		this.MarkToUnload();
	}

	// Token: 0x06002443 RID: 9283 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void onBackpackRemoved(string reason)
	{
	}

	// Token: 0x06002444 RID: 9284 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void createMesh()
	{
	}

	// Token: 0x06002445 RID: 9285 RVA: 0x000DD81C File Offset: 0x000DBA1C
	public override bool IsMarkedForUnload()
	{
		return base.IsMarkedForUnload() && this.bRemoved;
	}

	// Token: 0x06002446 RID: 9286 RVA: 0x000DD82E File Offset: 0x000DBA2E
	public override void OnUnlockedServer(int _unlockingPlayerId, ushort _channel)
	{
		if (this.bag.IsEmpty())
		{
			this.KillLootContainer();
		}
	}

	// Token: 0x06002447 RID: 9287 RVA: 0x000DD843 File Offset: 0x000DBA43
	[PublicizedFrom(EAccessModifier.Protected)]
	public EntityContainerAbs()
	{
	}

	// Token: 0x04001AED RID: 6893
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int deathUpdateTime;

	// Token: 0x04001AEE RID: 6894
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int timeStayAfterDeath;

	// Token: 0x04001AEF RID: 6895
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool bRemoved;

	// Token: 0x04001AF0 RID: 6896
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public ProfilerMarker pmEntityType;
}
