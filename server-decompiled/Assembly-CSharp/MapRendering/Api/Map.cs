using System;
using System.Net;
using UnityEngine.Scripting;
using Utf8Json;
using Webserver;
using Webserver.WebAPI;

namespace MapRendering.Api
{
	// Token: 0x02001B14 RID: 6932
	[Preserve]
	public class Map : AbsRestApi
	{
		// Token: 0x0600D0B0 RID: 53424 RVA: 0x004B74B8 File Offset: 0x004B56B8
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

		// Token: 0x0600D0B1 RID: 53425 RVA: 0x004B750C File Offset: 0x004B570C
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

		// Token: 0x0600D0B2 RID: 53426 RVA: 0x004B7591 File Offset: 0x004B5791
		public override int[] DefaultMethodPermissionLevels()
		{
			return new int[] { -2147483647, 2000, int.MinValue, int.MinValue, int.MinValue };
		}

		// Token: 0x0600D0B3 RID: 53427 RVA: 0x004B75A4 File Offset: 0x004B57A4
		public Map()
			: base(null)
		{
		}

		// Token: 0x04009DE2 RID: 40418
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyEnabled = JsonWriter.GetEncodedPropertyNameWithBeginObject("enabled");

		// Token: 0x04009DE3 RID: 40419
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyMapBlockSize = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("mapBlockSize");

		// Token: 0x04009DE4 RID: 40420
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyMaxZoom = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("maxZoom");

		// Token: 0x04009DE5 RID: 40421
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyMapSize = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("mapSize");
	}
}
