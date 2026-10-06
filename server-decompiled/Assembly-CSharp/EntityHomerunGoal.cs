using System;
using System.Collections.Generic;
using Audio;
using GameEvent.GameEventHelpers;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020004BA RID: 1210
[Preserve]
public class EntityHomerunGoal : Entity
{
	// Token: 0x06002618 RID: 9752 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool CanCollideWithBlocks()
	{
		return false;
	}

	// Token: 0x06002619 RID: 9753 RVA: 0x00020119 File Offset: 0x0001E319
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool isEntityStatic()
	{
		return true;
	}

	// Token: 0x0600261A RID: 9754 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool CanBePushed()
	{
		return false;
	}

	// Token: 0x0600261B RID: 9755 RVA: 0x000E9228 File Offset: 0x000E7428
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

	// Token: 0x0600261C RID: 9756 RVA: 0x000E9580 File Offset: 0x000E7780
	public override void CopyPropertiesFromEntityClass()
	{
		base.CopyPropertiesFromEntityClass();
		EntityClass entityClass = EntityClass.list[this.entityClass];
		entityClass.Properties.ParseInt("ScoreAdded", ref this.ScoreAdded);
		entityClass.Properties.ParseFloat("Size", ref this.Size);
		entityClass.Properties.ParseBool("IsMoving", ref this.IsMoving);
	}

	// Token: 0x04001C57 RID: 7255
	public HomerunData Owner;

	// Token: 0x04001C58 RID: 7256
	public bool ReadyForDelete;

	// Token: 0x04001C59 RID: 7257
	public int ScoreAdded = 1;

	// Token: 0x04001C5A RID: 7258
	public float Size = 2f;

	// Token: 0x04001C5B RID: 7259
	public Vector3 StartPosition;

	// Token: 0x04001C5C RID: 7260
	public float TimeRemaining = 20f;

	// Token: 0x04001C5D RID: 7261
	public bool IsMoving = true;

	// Token: 0x04001C5E RID: 7262
	public EntityHomerunGoal.Direction direction;

	// Token: 0x020004BB RID: 1211
	public enum Direction
	{
		// Token: 0x04001C60 RID: 7264
		YPositive,
		// Token: 0x04001C61 RID: 7265
		XPositive,
		// Token: 0x04001C62 RID: 7266
		XNegative,
		// Token: 0x04001C63 RID: 7267
		ZPositive,
		// Token: 0x04001C64 RID: 7268
		ZNegative,
		// Token: 0x04001C65 RID: 7269
		Max
	}
}
