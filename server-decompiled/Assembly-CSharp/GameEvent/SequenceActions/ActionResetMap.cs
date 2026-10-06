using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019D8 RID: 6616
	[Preserve]
	public class ActionResetMap : ActionBaseClientAction
	{
		// Token: 0x0600CA91 RID: 51857 RVA: 0x004A1EF8 File Offset: 0x004A00F8
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

		// Token: 0x0600CA92 RID: 51858 RVA: 0x004A1FA7 File Offset: 0x004A01A7
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseBool(ActionResetMap.PropRemoveDiscovery, ref this.removeDiscovery);
			properties.ParseBool(ActionResetMap.PropRemoveWaypoints, ref this.removeWaypoints);
		}

		// Token: 0x0600CA93 RID: 51859 RVA: 0x004A1FD2 File Offset: 0x004A01D2
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionResetMap
			{
				removeDiscovery = this.removeDiscovery,
				removeWaypoints = this.removeWaypoints
			};
		}

		// Token: 0x04009935 RID: 39221
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool removeDiscovery;

		// Token: 0x04009936 RID: 39222
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool removeWaypoints;

		// Token: 0x04009937 RID: 39223
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropRemoveDiscovery = "remove_discovery";

		// Token: 0x04009938 RID: 39224
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropRemoveWaypoints = "remove_waypoints";
	}
}
