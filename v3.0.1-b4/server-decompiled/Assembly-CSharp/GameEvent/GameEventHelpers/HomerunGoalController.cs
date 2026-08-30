using System;
using System.Collections.Generic;
using Audio;
using UnityEngine;

namespace GameEvent.GameEventHelpers
{
	// Token: 0x02001928 RID: 6440
	public class HomerunGoalController : MonoBehaviour
	{
		// Token: 0x0600C60F RID: 50703 RVA: 0x0048C458 File Offset: 0x0048A658
		[PublicizedFrom(EAccessModifier.Private)]
		public void Update()
		{
			if (GameManager.Instance.World == null)
			{
				this.ReadyForDelete = true;
				return;
			}
			switch (this.direction)
			{
			case HomerunGoalController.Direction.YPositive:
				this.position = this.StartPosition + Vector3.up * Mathf.PingPong(Time.time, 2f) * 2f;
				base.transform.position = this.position - Origin.position;
				break;
			case HomerunGoalController.Direction.XPositive:
				this.position = this.StartPosition + Vector3.right * Mathf.PingPong(Time.time, 2f) * 2f;
				base.transform.position = this.position - Origin.position;
				break;
			case HomerunGoalController.Direction.XNegative:
				this.position = this.StartPosition + Vector3.left * Mathf.PingPong(Time.time, 2f) * 2f;
				base.transform.position = this.position - Origin.position;
				break;
			case HomerunGoalController.Direction.ZPositive:
				this.position = this.StartPosition + Vector3.forward * Mathf.PingPong(Time.time, 2f) * 2f;
				base.transform.position = this.position - Origin.position;
				break;
			case HomerunGoalController.Direction.ZNegative:
				this.position = this.StartPosition + Vector3.back * Mathf.PingPong(Time.time, 2f) * 2f;
				base.transform.position = this.position - Origin.position;
				break;
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
					World world = GameManager.Instance.World;
					float lightBrightness = world.GetLightBrightness(entityAlive.GetBlockPosition());
					world.GetGameManager().SpawnParticleEffectServer(new ParticleEffect("twitch_fireworks", entityAlive.position, lightBrightness, Color.white, null, null, false, 1f, ""), entityAlive.entityId, false, true);
					Manager.BroadcastPlayByLocalPlayer(entityAlive.position, "twitch_celebrate");
					entityAlive.DamageEntity(new DamageSource(EnumDamageSource.Internal, EnumDamageTypes.Suicide), 99999, false, 1f);
					GameManager.Instance.World.RemoveEntity(entityAlive.entityId, EnumRemoveEntityReason.Killed);
					if (!this.ReadyForDelete)
					{
						this.Owner.Score += this.ScoreAdded;
						this.ReadyForDelete = true;
					}
				}
			}
		}

		// Token: 0x0400950E RID: 38158
		public HomerunData Owner;

		// Token: 0x0400950F RID: 38159
		public Vector3 position;

		// Token: 0x04009510 RID: 38160
		public bool ReadyForDelete;

		// Token: 0x04009511 RID: 38161
		public int ScoreAdded = 1;

		// Token: 0x04009512 RID: 38162
		public float Size = 2f;

		// Token: 0x04009513 RID: 38163
		public Vector3 StartPosition;

		// Token: 0x04009514 RID: 38164
		public HomerunGoalController.Direction direction;

		// Token: 0x02001929 RID: 6441
		public enum Direction
		{
			// Token: 0x04009516 RID: 38166
			YPositive,
			// Token: 0x04009517 RID: 38167
			XPositive,
			// Token: 0x04009518 RID: 38168
			XNegative,
			// Token: 0x04009519 RID: 38169
			ZPositive,
			// Token: 0x0400951A RID: 38170
			ZNegative,
			// Token: 0x0400951B RID: 38171
			Max
		}
	}
}
