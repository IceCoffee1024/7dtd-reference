using System;
using System.Net;
using UnityEngine.Scripting;
using Utf8Json;
using Webserver;
using Webserver.WebAPI;

namespace MapRendering.Api
{
	// Token: 0x02001A90 RID: 6800
	[Preserve]
	public class Map : AbsRestApi
	{
		// Token: 0x0600CCBB RID: 52411 RVA: 0x004A90FC File Offset: 0x004A72FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void HandleRestGet(RequestContext _context)
		{
			string requestPath = _context.RequestPath;
			JsonWriter jsonWriter;
			AbsRestApi.PrepareEnvelopedResult(out jsonWriter);
			if (requestPath == "config")
			{
				this.writeConfig(ref jsonWriter);
				AbsRestApi.SendEnvelopedResult(_context, ref jsonWriter, HttpStatusCode.OK, null, null, null);
				return;
			}
			AbsRestApi.SendEmptyResponse(_context, HttpStatusCode.NotImplemented, null, "INVALID_ID", null);
		}

		// Token: 0x0600CCBC RID: 52412 RVA: 0x004A9150 File Offset: 0x004A7350
		[PublicizedFrom(EAccessModifier.Private)]
		public void writeConfig(ref JsonWriter writer)
		{
			writer.WriteRaw(Map.jsonKeyEnabled);
			writer.WriteBoolean(MapRenderer.Enabled);
			writer.WriteRaw(Map.jsonKeyMapBlockSize);
			writer.WriteInt32(Constants.MapBlockSize);
			writer.WriteRaw(Map.jsonKeyMaxZoom);
			writer.WriteInt32(Constants.Zoomlevels - 1);
			Vector3i vector3i;
			Vector3i vector3i2;
			GameManager.Instance.World.GetWorldExtent(out vector3i, out vector3i2);
			Vector3i vector3i3 = vector3i2 - vector3i;
			writer.WriteRaw(Map.jsonKeyMapSize);
			JsonCommons.WriteVector3I(ref writer, vector3i3);
			writer.WriteEndObject();
		}

		// Token: 0x0600CCBD RID: 52413 RVA: 0x004A91D5 File Offset: 0x004A73D5
		public override int[] DefaultMethodPermissionLevels()
		{
			return new int[] { -2147483647, 2000, int.MinValue, int.MinValue, int.MinValue };
		}

		// Token: 0x0600CCBE RID: 52414 RVA: 0x004A91E8 File Offset: 0x004A73E8
		public Map()
			: base(null)
		{
		}

		// Token: 0x04009B02 RID: 39682
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyEnabled = JsonWriter.GetEncodedPropertyNameWithBeginObject("enabled");

		// Token: 0x04009B03 RID: 39683
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyMapBlockSize = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("mapBlockSize");

		// Token: 0x04009B04 RID: 39684
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyMaxZoom = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("maxZoom");

		// Token: 0x04009B05 RID: 39685
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyMapSize = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("mapSize");
	}
}
