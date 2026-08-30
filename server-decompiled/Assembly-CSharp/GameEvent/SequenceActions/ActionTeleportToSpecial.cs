using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019CD RID: 6605
	[Preserve]
	public class ActionTeleportToSpecial : ActionBaseClientAction
	{
		// Token: 0x0600C9CF RID: 51663 RVA: 0x004A07B4 File Offset: 0x0049E9B4
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

		// Token: 0x0600C9D0 RID: 51664 RVA: 0x004A08A1 File Offset: 0x0049EAA1
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

		// Token: 0x0600C9D1 RID: 51665 RVA: 0x004A08B7 File Offset: 0x0049EAB7
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseEnum<ActionTeleportToSpecial.SpecialPointTypes>(ActionTeleportToSpecial.PropSpecialType, ref this.pointType);
			properties.ParseFloat(ActionTeleportToSpecial.PropTeleportDelay, ref this.teleportDelay);
		}

		// Token: 0x0600C9D2 RID: 51666 RVA: 0x004A08E2 File Offset: 0x0049EAE2
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

		// Token: 0x0400990F RID: 39183
		[PublicizedFrom(EAccessModifier.Private)]
		public Vector3 position;

		// Token: 0x04009910 RID: 39184
		[PublicizedFrom(EAccessModifier.Protected)]
		public float teleportDelay = 0.1f;

		// Token: 0x04009911 RID: 39185
		[PublicizedFrom(EAccessModifier.Protected)]
		public ActionTeleportToSpecial.SpecialPointTypes pointType;

		// Token: 0x04009912 RID: 39186
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropSpecialType = "special_type";

		// Token: 0x04009913 RID: 39187
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTeleportDelay = "teleport_delay";

		// Token: 0x020019CE RID: 6606
		[PublicizedFrom(EAccessModifier.Protected)]
		public enum SpecialPointTypes
		{
			// Token: 0x04009915 RID: 39189
			Bedroll,
			// Token: 0x04009916 RID: 39190
			Landclaim,
			// Token: 0x04009917 RID: 39191
			Backpack
		}
	}
}
