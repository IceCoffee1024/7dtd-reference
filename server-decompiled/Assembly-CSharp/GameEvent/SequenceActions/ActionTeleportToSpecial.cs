using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001A2A RID: 6698
	[Preserve]
	public class ActionTeleportToSpecial : ActionBaseClientAction
	{
		// Token: 0x0600CC3B RID: 52283 RVA: 0x004A8754 File Offset: 0x004A6954
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

		// Token: 0x0600CC3C RID: 52284 RVA: 0x004A8841 File Offset: 0x004A6A41
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

		// Token: 0x0600CC3D RID: 52285 RVA: 0x004A8857 File Offset: 0x004A6A57
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseEnum<ActionTeleportToSpecial.SpecialPointTypes>(ActionTeleportToSpecial.PropSpecialType, ref this.pointType);
			properties.ParseFloat(ActionTeleportToSpecial.PropTeleportDelay, ref this.teleportDelay);
		}

		// Token: 0x0600CC3E RID: 52286 RVA: 0x004A8882 File Offset: 0x004A6A82
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

		// Token: 0x04009ABE RID: 39614
		[PublicizedFrom(EAccessModifier.Private)]
		public Vector3 position;

		// Token: 0x04009ABF RID: 39615
		[PublicizedFrom(EAccessModifier.Protected)]
		public float teleportDelay = 0.1f;

		// Token: 0x04009AC0 RID: 39616
		[PublicizedFrom(EAccessModifier.Protected)]
		public ActionTeleportToSpecial.SpecialPointTypes pointType;

		// Token: 0x04009AC1 RID: 39617
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropSpecialType = "special_type";

		// Token: 0x04009AC2 RID: 39618
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTeleportDelay = "teleport_delay";

		// Token: 0x02001A2B RID: 6699
		[PublicizedFrom(EAccessModifier.Protected)]
		public enum SpecialPointTypes
		{
			// Token: 0x04009AC4 RID: 39620
			Bedroll,
			// Token: 0x04009AC5 RID: 39621
			Landclaim,
			// Token: 0x04009AC6 RID: 39622
			Backpack
		}
	}
}
