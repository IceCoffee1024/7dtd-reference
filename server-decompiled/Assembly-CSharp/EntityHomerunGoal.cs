using System;
using System.Collections.Generic;
using Audio;
using GameEvent.GameEventHelpers;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020004BC RID: 1212
[Preserve]
public class EntityHomerunGoal : Entity
{
	// Token: 0x0600261F RID: 9759 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool CanCollideWithBlocks()
	{
		return false;
	}

	// Token: 0x06002620 RID: 9760 RVA: 0x00020119 File Offset: 0x0001E319
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool isEntityStatic()
	{
		return true;
	}

	// Token: 0x06002621 RID: 9761 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool CanBePushed()
	{
		return false;
	}

	// Token: 0x06002622 RID: 9762 RVA: 0x000E9638 File Offset: 0x000E7838
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Update()
	{
		base.Update();
		World world = GameManager.Instance.World;
		if (world == null)
		{
			this.ReadyForDelete = true;
			return;
		}
		if (this.Owner == null)
		{
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
			{
				world.RemoveEntity(this.entityId, EnumRemoveEntityReason.Killed);
			}
			return;
		}
		this.TimeRemaining -= Time.deltaTime;
		if (this.TimeRemaining <= 0f)
		{
			this.ReadyForDelete = true;
			return;
		}
		if (this.IsMoving)
		{
			switch (this.direction)
			{
			case EntityHomerunGoal.Direction.YPositive:
				this.SetPosition(this.StartPosition + Vector3.up * Mathf.PingPong(Time.time, 2f) * 2f, true);
				break;
			case EntityHomerunGoal.Direction.XPositive:
				this.SetPosition(this.StartPosition + Vector3.right * Mathf.PingPong(Time.time, 2f) * 2f, true);
				break;
			case EntityHomerunGoal.Direction.XNegative:
				this.SetPosition(this.StartPosition + Vector3.left * Mathf.PingPong(Time.time, 2f) * 2f, true);
				break;
			case EntityHomerunGoal.Direction.ZPositive:
				this.SetPosition(this.StartPosition + Vector3.forward * Mathf.PingPong(Time.time, 2f) * 2f, true);
				break;
			case EntityHomerunGoal.Direction.ZNegative:
				this.SetPosition(this.StartPosition + Vector3.back * Mathf.PingPong(Time.time, 2f) * 2f, true);
				break;
			}
		}
		if (Vector3.Distance(this.position, this.Owner.Player.position) > 50f)
		{
			this.ReadyForDelete = true;
			return;
		}
		List<Entity> entitiesInBounds = GameManager.Instance.World.GetEntitiesInBounds(null, new Bounds(this.position, Vector3.one * this.Size));
		for (int i = 0; i < entitiesInBounds.Count; i++)
		{
			EntityAlive entityAlive = entitiesInBounds[i] as EntityAlive;
			if (entityAlive != null && entityAlive != null && entityAlive.IsAlive() && !(entityAlive is EntityPlayer) && entityAlive.emodel != null && entityAlive.emodel.transform != null && entityAlive.emodel.IsRagdollActive)
			{
				float lightBrightness = world.GetLightBrightness(entityAlive.GetBlockPosition());
				world.GetGameManager().SpawnParticleEffectServer(new ParticleEffect("twitch_fireworks", entityAlive.position, lightBrightness, Color.white, null, null, false, 1f, ""), entityAlive.entityId, false, true);
				Manager.BroadcastPlayByLocalPlayer(entityAlive.position, "twitch_baseball_balloon_pop");
				entityAlive.DamageEntity(new DamageSource(EnumDamageSource.Internal, EnumDamageTypes.Suicide), 99999, false, 1f);
				world.RemoveEntity(entityAlive.entityId, EnumRemoveEntityReason.Killed);
				if (!this.ReadyForDelete)
				{
					this.Owner.Score += this.ScoreAdded;
					this.ReadyForDelete = true;
				}
				this.Owner.AddScoreDisplay(this.position);
				return;
			}
		}
	}

	// Token: 0x06002623 RID: 9763 RVA: 0x000E9990 File Offset: 0x000E7B90
	public override void CopyPropertiesFromEntityClass()
	{
		base.CopyPropertiesFromEntityClass();
		EntityClass entityClass = EntityClass.list[this.entityClass];
		entityClass.Properties.ParseInt("ScoreAdded", ref this.ScoreAdded);
		entityClass.Properties.ParseFloat("Size", ref this.Size);
		entityClass.Properties.ParseBool("IsMoving", ref this.IsMoving);
	}

	// Token: 0x04001C67 RID: 7271
	public HomerunData Owner;

	// Token: 0x04001C68 RID: 7272
	public bool ReadyForDelete;

	// Token: 0x04001C69 RID: 7273
	public int ScoreAdded = 1;

	// Token: 0x04001C6A RID: 7274
	public float Size = 2f;

	// Token: 0x04001C6B RID: 7275
	public Vector3 StartPosition;

	// Token: 0x04001C6C RID: 7276
	public float TimeRemaining = 20f;

	// Token: 0x04001C6D RID: 7277
	public bool IsMoving = true;

	// Token: 0x04001C6E RID: 7278
	public EntityHomerunGoal.Direction direction;

	// Token: 0x020004BD RID: 1213
	public enum Direction
	{
		// Token: 0x04001C70 RID: 7280
		YPositive,
		// Token: 0x04001C71 RID: 7281
		XPositive,
		// Token: 0x04001C72 RID: 7282
		XNegative,
		// Token: 0x04001C73 RID: 7283
		ZPositive,
		// Token: 0x04001C74 RID: 7284
		ZNegative,
		// Token: 0x04001C75 RID: 7285
		Max
	}
}
