using System;
using System.Net;
using UnityEngine.Scripting;
using Utf8Json;
using Webserver;
using Webserver.WebAPI;

namespace MapRendering.Api
{
	// Token: 0x02001AE3 RID: 6883
	[Preserve]
	public class Map : AbsRestApi
	{
		// Token: 0x0600CF9C RID: 53148 RVA: 0x004B39BC File Offset: 0x004B1BBC
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

		// Token: 0x0600CF9D RID: 53149 RVA: 0x004B3A10 File Offset: 0x004B1C10
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

		// Token: 0x0600CF9E RID: 53150 RVA: 0x004B3A95 File Offset: 0x004B1C95
		public override int[] DefaultMethodPermissionLevels()
		{
			return new int[] { -2147483647, 2000, int.MinValue, int.MinValue, int.MinValue };
		}

		// Token: 0x0600CF9F RID: 53151 RVA: 0x004B3AA8 File Offset: 0x004B1CA8
		public Map()
			: base(null)
		{
		}

		// Token: 0x04009CF9 RID: 40185
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyEnabled = JsonWriter.GetEncodedPropertyNameWithBeginObject("enabled");

		// Token: 0x04009CFA RID: 40186
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyMapBlockSize = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("mapBlockSize");

		// Token: 0x04009CFB RID: 40187
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyMaxZoom = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("maxZoom");

		// Token: 0x04009CFC RID: 40188
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyMapSize = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("mapSize");
	}
}
