using System;
using System.Net;
using UnityEngine.Scripting;
using Utf8Json;
using Webserver;
using Webserver.WebAPI;

namespace MapRendering.Api
{
	// Token: 0x02001AB7 RID: 6839
	[Preserve]
	public class Map : AbsRestApi
	{
		// Token: 0x0600CE44 RID: 52804 RVA: 0x004AF4F0 File Offset: 0x004AD6F0
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

		// Token: 0x0600CE45 RID: 52805 RVA: 0x004AF544 File Offset: 0x004AD744
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

		// Token: 0x0600CE46 RID: 52806 RVA: 0x004AF5C9 File Offset: 0x004AD7C9
		public override int[] DefaultMethodPermissionLevels()
		{
			return new int[] { -2147483647, 2000, int.MinValue, int.MinValue, int.MinValue };
		}

		// Token: 0x0600CE47 RID: 52807 RVA: 0x004AF5DC File Offset: 0x004AD7DC
		public Map()
			: base(null)
		{
		}

		// Token: 0x04009C33 RID: 39987
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyEnabled = JsonWriter.GetEncodedPropertyNameWithBeginObject("enabled");

		// Token: 0x04009C34 RID: 39988
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyMapBlockSize = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("mapBlockSize");

		// Token: 0x04009C35 RID: 39989
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyMaxZoom = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("maxZoom");

		// Token: 0x04009C36 RID: 39990
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyMapSize = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("mapSize");
	}
}
