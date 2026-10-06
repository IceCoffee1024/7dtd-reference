using System;
using System.Collections.Generic;
using Audio;
using UnityEngine;

namespace GameEvent.GameEventHelpers
{
	// Token: 0x0200197A RID: 6522
	public class HomerunGoalController : MonoBehaviour
	{
		// Token: 0x0600C8EE RID: 51438 RVA: 0x00496C90 File Offset: 0x00494E90
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

		// Token: 0x040096FD RID: 38653
		public HomerunData Owner;

		// Token: 0x040096FE RID: 38654
		public Vector3 position;

		// Token: 0x040096FF RID: 38655
		public bool ReadyForDelete;

		// Token: 0x04009700 RID: 38656
		public int ScoreAdded = 1;

		// Token: 0x04009701 RID: 38657
		public float Size = 2f;

		// Token: 0x04009702 RID: 38658
		public Vector3 StartPosition;

		// Token: 0x04009703 RID: 38659
		public HomerunGoalController.Direction direction;

		// Token: 0x0200197B RID: 6523
		public enum Direction
		{
			// Token: 0x04009705 RID: 38661
			YPositive,
			// Token: 0x04009706 RID: 38662
			XPositive,
			// Token: 0x04009707 RID: 38663
			XNegative,
			// Token: 0x04009708 RID: 38664
			ZPositive,
			// Token: 0x04009709 RID: 38665
			ZNegative,
			// Token: 0x0400970A RID: 38666
			Max
		}
	}
}
