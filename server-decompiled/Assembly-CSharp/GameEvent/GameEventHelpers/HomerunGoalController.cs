using System;
using System.Collections.Generic;
using Audio;
using UnityEngine;

namespace GameEvent.GameEventHelpers
{
	// Token: 0x0200194E RID: 6478
	public class HomerunGoalController : MonoBehaviour
	{
		// Token: 0x0600C796 RID: 51094 RVA: 0x004927A4 File Offset: 0x004909A4
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

		// Token: 0x04009637 RID: 38455
		public HomerunData Owner;

		// Token: 0x04009638 RID: 38456
		public Vector3 position;

		// Token: 0x04009639 RID: 38457
		public bool ReadyForDelete;

		// Token: 0x0400963A RID: 38458
		public int ScoreAdded = 1;

		// Token: 0x0400963B RID: 38459
		public float Size = 2f;

		// Token: 0x0400963C RID: 38460
		public Vector3 StartPosition;

		// Token: 0x0400963D RID: 38461
		public HomerunGoalController.Direction direction;

		// Token: 0x0200194F RID: 6479
		public enum Direction
		{
			// Token: 0x0400963F RID: 38463
			YPositive,
			// Token: 0x04009640 RID: 38464
			XPositive,
			// Token: 0x04009641 RID: 38465
			XNegative,
			// Token: 0x04009642 RID: 38466
			ZPositive,
			// Token: 0x04009643 RID: 38467
			ZNegative,
			// Token: 0x04009644 RID: 38468
			Max
		}
	}
}
