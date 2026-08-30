using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001986 RID: 6534
	[Preserve]
	public class ActionResetMap : ActionBaseClientAction
	{
		// Token: 0x0600C7B2 RID: 51122 RVA: 0x004976C4 File Offset: 0x004958C4
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

		// Token: 0x0600C7B3 RID: 51123 RVA: 0x00497773 File Offset: 0x00495973
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseBool(ActionResetMap.PropRemoveDiscovery, ref this.removeDiscovery);
			properties.ParseBool(ActionResetMap.PropRemoveWaypoints, ref this.removeWaypoints);
		}

		// Token: 0x0600C7B4 RID: 51124 RVA: 0x0049779E File Offset: 0x0049599E
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionResetMap
			{
				removeDiscovery = this.removeDiscovery,
				removeWaypoints = this.removeWaypoints
			};
		}

		// Token: 0x04009746 RID: 38726
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool removeDiscovery;

		// Token: 0x04009747 RID: 38727
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool removeWaypoints;

		// Token: 0x04009748 RID: 38728
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropRemoveDiscovery = "remove_discovery";

		// Token: 0x04009749 RID: 38729
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropRemoveWaypoints = "remove_waypoints";
	}
}
