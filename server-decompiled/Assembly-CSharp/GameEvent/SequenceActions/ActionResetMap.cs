using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001A09 RID: 6665
	[Preserve]
	public class ActionResetMap : ActionBaseClientAction
	{
		// Token: 0x0600CBA5 RID: 52133 RVA: 0x004A59F8 File Offset: 0x004A3BF8
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

		// Token: 0x0600CBA6 RID: 52134 RVA: 0x004A5AA7 File Offset: 0x004A3CA7
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseBool(ActionResetMap.PropRemoveDiscovery, ref this.removeDiscovery);
			properties.ParseBool(ActionResetMap.PropRemoveWaypoints, ref this.removeWaypoints);
		}

		// Token: 0x0600CBA7 RID: 52135 RVA: 0x004A5AD2 File Offset: 0x004A3CD2
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionResetMap
			{
				removeDiscovery = this.removeDiscovery,
				removeWaypoints = this.removeWaypoints
			};
		}

		// Token: 0x04009A1E RID: 39454
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool removeDiscovery;

		// Token: 0x04009A1F RID: 39455
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool removeWaypoints;

		// Token: 0x04009A20 RID: 39456
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropRemoveDiscovery = "remove_discovery";

		// Token: 0x04009A21 RID: 39457
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropRemoveWaypoints = "remove_waypoints";
	}
}
