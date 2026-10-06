using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019F9 RID: 6649
	[Preserve]
	public class ActionTeleportToSpecial : ActionBaseClientAction
	{
		// Token: 0x0600CB27 RID: 52007 RVA: 0x004A4C58 File Offset: 0x004A2E58
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

		// Token: 0x0600CB28 RID: 52008 RVA: 0x004A4D45 File Offset: 0x004A2F45
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

		// Token: 0x0600CB29 RID: 52009 RVA: 0x004A4D5B File Offset: 0x004A2F5B
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseEnum<ActionTeleportToSpecial.SpecialPointTypes>(ActionTeleportToSpecial.PropSpecialType, ref this.pointType);
			properties.ParseFloat(ActionTeleportToSpecial.PropTeleportDelay, ref this.teleportDelay);
		}

		// Token: 0x0600CB2A RID: 52010 RVA: 0x004A4D86 File Offset: 0x004A2F86
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

		// Token: 0x040099D5 RID: 39381
		[PublicizedFrom(EAccessModifier.Private)]
		public Vector3 position;

		// Token: 0x040099D6 RID: 39382
		[PublicizedFrom(EAccessModifier.Protected)]
		public float teleportDelay = 0.1f;

		// Token: 0x040099D7 RID: 39383
		[PublicizedFrom(EAccessModifier.Protected)]
		public ActionTeleportToSpecial.SpecialPointTypes pointType;

		// Token: 0x040099D8 RID: 39384
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropSpecialType = "special_type";

		// Token: 0x040099D9 RID: 39385
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTeleportDelay = "teleport_delay";

		// Token: 0x020019FA RID: 6650
		[PublicizedFrom(EAccessModifier.Protected)]
		public enum SpecialPointTypes
		{
			// Token: 0x040099DB RID: 39387
			Bedroll,
			// Token: 0x040099DC RID: 39388
			Landclaim,
			// Token: 0x040099DD RID: 39389
			Backpack
		}
	}
}
