using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019AC RID: 6572
	[Preserve]
	public class ActionResetMap : ActionBaseClientAction
	{
		// Token: 0x0600C939 RID: 51513 RVA: 0x0049DA08 File Offset: 0x0049BC08
		public override void OnClientPerform(Entity target)
		{
			EntityPlayerLocal entityPlayerLocal = target as EntityPlayerLocal;
			if (entityPlayerLocal != null)
			{
				if (this.removeDiscovery)
				{
					entityPlayerLocal.ChunkObserver.mapDatabase.Clear();
				}
				if (this.removeWaypoints)
				{
					for (int i = 0; i < entityPlayerLocal.Waypoints.Collection.list.Count; i++)
					{
						Waypoint waypoint = entityPlayerLocal.Waypoints.Collection.list[i];
						if (waypoint.navObject != null)
						{
							NavObjectManager.Instance.UnRegisterNavObject(waypoint.navObject);
						}
					}
					entityPlayerLocal.WaypointInvites.Clear();
					entityPlayerLocal.Waypoints.Collection.Clear();
					entityPlayerLocal.markerPosition = Vector3i.zero;
				}
			}
		}

		// Token: 0x0600C93A RID: 51514 RVA: 0x0049DAB7 File Offset: 0x0049BCB7
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseBool(ActionResetMap.PropRemoveDiscovery, ref this.removeDiscovery);
			properties.ParseBool(ActionResetMap.PropRemoveWaypoints, ref this.removeWaypoints);
		}

		// Token: 0x0600C93B RID: 51515 RVA: 0x0049DAE2 File Offset: 0x0049BCE2
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionResetMap
			{
				removeDiscovery = this.removeDiscovery,
				removeWaypoints = this.removeWaypoints
			};
		}

		// Token: 0x0400986F RID: 39023
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool removeDiscovery;

		// Token: 0x04009870 RID: 39024
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool removeWaypoints;

		// Token: 0x04009871 RID: 39025
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropRemoveDiscovery = "remove_discovery";

		// Token: 0x04009872 RID: 39026
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropRemoveWaypoints = "remove_waypoints";
	}
}
