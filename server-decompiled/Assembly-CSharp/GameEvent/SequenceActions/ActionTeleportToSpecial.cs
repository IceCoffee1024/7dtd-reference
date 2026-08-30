using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019A7 RID: 6567
	[Preserve]
	public class ActionTeleportToSpecial : ActionBaseClientAction
	{
		// Token: 0x0600C848 RID: 51272 RVA: 0x0049A45C File Offset: 0x0049865C
		public override void OnClientPerform(Entity target)
		{
			EntityPlayer entityPlayer = target as EntityPlayer;
			if (entityPlayer != null)
			{
				World world = GameManager.Instance.World;
				this.position = Vector3.zero;
				switch (this.pointType)
				{
				case ActionTeleportToSpecial.SpecialPointTypes.Bedroll:
					if (entityPlayer.SpawnPoints.Count == 0)
					{
						return;
					}
					this.position = entityPlayer.SpawnPoints[0];
					break;
				case ActionTeleportToSpecial.SpecialPointTypes.Landclaim:
				{
					PersistentPlayerData playerDataFromEntityID = GameManager.Instance.persistentPlayers.GetPlayerDataFromEntityID(entityPlayer.entityId);
					if (playerDataFromEntityID.LPBlocks == null || playerDataFromEntityID.LPBlocks.Count == 0)
					{
						return;
					}
					this.position = playerDataFromEntityID.LPBlocks[0];
					break;
				}
				case ActionTeleportToSpecial.SpecialPointTypes.Backpack:
				{
					Vector3i lastDroppedBackpackPosition = entityPlayer.GetLastDroppedBackpackPosition();
					if (lastDroppedBackpackPosition == Vector3i.zero)
					{
						return;
					}
					this.position = lastDroppedBackpackPosition;
					break;
				}
				}
				GameManager.Instance.StartCoroutine(this.handleTeleport(entityPlayer));
			}
		}

		// Token: 0x0600C849 RID: 51273 RVA: 0x0049A549 File Offset: 0x00498749
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator handleTeleport(EntityPlayer player)
		{
			yield return new WaitForSeconds(this.teleportDelay);
			if (this.position.y > 0f)
			{
				this.position += Vector3.up * 2f;
				if (player.isEntityRemote)
				{
					SingletonMonoBehaviour<ConnectionManager>.Instance.Clients.ForEntityId(player.entityId).SendPackage(NetPackageManager.GetPackage<NetPackageTeleportPlayer>().Setup(this.position, null, false));
				}
				else
				{
					((EntityPlayerLocal)player).PlayerUI.windowManager.CloseAllOpenModalWindows(null, false);
					((EntityPlayerLocal)player).TeleportToPosition(this.position, false, null);
				}
			}
			yield break;
		}

		// Token: 0x0600C84A RID: 51274 RVA: 0x0049A55F File Offset: 0x0049875F
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseEnum<ActionTeleportToSpecial.SpecialPointTypes>(ActionTeleportToSpecial.PropSpecialType, ref this.pointType);
			properties.ParseFloat(ActionTeleportToSpecial.PropTeleportDelay, ref this.teleportDelay);
		}

		// Token: 0x0600C84B RID: 51275 RVA: 0x0049A58A File Offset: 0x0049878A
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionTeleportToSpecial
			{
				targetGroup = this.targetGroup,
				pointType = this.pointType,
				teleportDelay = this.teleportDelay
			};
		}

		// Token: 0x040097E6 RID: 38886
		[PublicizedFrom(EAccessModifier.Private)]
		public Vector3 position;

		// Token: 0x040097E7 RID: 38887
		[PublicizedFrom(EAccessModifier.Protected)]
		public float teleportDelay = 0.1f;

		// Token: 0x040097E8 RID: 38888
		[PublicizedFrom(EAccessModifier.Protected)]
		public ActionTeleportToSpecial.SpecialPointTypes pointType;

		// Token: 0x040097E9 RID: 38889
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropSpecialType = "special_type";

		// Token: 0x040097EA RID: 38890
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTeleportDelay = "teleport_delay";

		// Token: 0x020019A8 RID: 6568
		[PublicizedFrom(EAccessModifier.Protected)]
		public enum SpecialPointTypes
		{
			// Token: 0x040097EC RID: 38892
			Bedroll,
			// Token: 0x040097ED RID: 38893
			Landclaim,
			// Token: 0x040097EE RID: 38894
			Backpack
		}
	}
}
