using System;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000498 RID: 1176
[Preserve]
public abstract class EntityContainerAbs : EntityItem
{
	// Token: 0x06002436 RID: 9270 RVA: 0x000DD2D4 File Offset: 0x000DB4D4
	public override void CopyPropertiesFromEntityClass()
	{
		base.CopyPropertiesFromEntityClass();
		EntityClass entityClass = EntityClass.list[this.entityClass];
		float num = 5f;
		entityClass.Properties.ParseFloat(EntityClass.PropTimeStayAfterDeath, ref num);
		this.timeStayAfterDeath = (int)(num * 20f);
	}

	// Token: 0x06002437 RID: 9271 RVA: 0x000DD31C File Offset: 0x000DB51C
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

	// Token: 0x06002438 RID: 9272 RVA: 0x000DD38C File Offset: 0x000DB58C
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

	// Token: 0x06002439 RID: 9273 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void updatePosition()
	{
	}

	// Token: 0x0600243A RID: 9274 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void updateMarkers()
	{
	}

	// Token: 0x0600243B RID: 9275 RVA: 0x000DD454 File Offset: 0x000DB654
	public override bool AllowActivationCommand(ReadOnlySpan<char> _commandName, EntityPlayerLocal _playerFocusing)
	{
		if (base.CommandIs(_commandName, "search"))
		{
			return this.bag != null;
		}
		return base.AllowActivationCommand(_commandName, _playerFocusing);
	}

	// Token: 0x0600243C RID: 9276 RVA: 0x000DD476 File Offset: 0x000DB676
	[PublicizedFrom(EAccessModifier.Protected)]
	public void removeBackpack(string reason)
	{
		this.deathUpdateTime = this.timeStayAfterDeath;
		this.onBackpackRemoved(reason);
		this.bRemoved = true;
		this.MarkToUnload();
	}

	// Token: 0x0600243D RID: 9277 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void onBackpackRemoved(string reason)
	{
	}

	// Token: 0x0600243E RID: 9278 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void createMesh()
	{
	}

	// Token: 0x0600243F RID: 9279 RVA: 0x000DD498 File Offset: 0x000DB698
	public override bool IsMarkedForUnload()
	{
		return base.IsMarkedForUnload() && this.bRemoved;
	}

	// Token: 0x06002440 RID: 9280 RVA: 0x000DD4AA File Offset: 0x000DB6AA
	public override void OnUnlockedServer(int _unlockingPlayerId, ushort _channel)
	{
		if (this.bag.IsEmpty())
		{
			this.KillLootContainer();
		}
	}

	// Token: 0x06002441 RID: 9281 RVA: 0x000DD4BF File Offset: 0x000DB6BF
	[PublicizedFrom(EAccessModifier.Protected)]
	public EntityContainerAbs()
	{
	}

	// Token: 0x04001ADD RID: 6877
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int deathUpdateTime;

	// Token: 0x04001ADE RID: 6878
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int timeStayAfterDeath;

	// Token: 0x04001ADF RID: 6879
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool bRemoved;

	// Token: 0x04001AE0 RID: 6880
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public ProfilerMarker pmEntityType;
}
