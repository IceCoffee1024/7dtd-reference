using System;
using System.Net;
using UnityEngine.Scripting;
using Utf8Json;
using Webserver.LiveData;

namespace Webserver.WebAPI.APIs.ServerState
{
	// Token: 0x02001B3D RID: 6973
	[Preserve]
	public class ServerStats : AbsRestApi
	{
		// Token: 0x0600D176 RID: 53622 RVA: 0x004BE1BC File Offset: 0x004BC3BC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void HandleRestGet(RequestContext _context)
		{
			JsonWriter jsonWriter;
			AbsRestApi.PrepareEnvelopedResult(out jsonWriter);
			jsonWriter.WriteRaw(ServerStats.jsonKeyGameTime);
			ValueTuple<int, int, int> valueTuple = GameUtils.WorldTimeToElements(GameManager.Instance.World.worldTime);
			int item = valueTuple.Item1;
			int item2 = valueTuple.Item2;
			int item3 = valueTuple.Item3;
			JsonCommons.WriteGameTimeObject(ref jsonWriter, item, item2, item3);
			jsonWriter.WriteRaw(ServerStats.jsonKeyPlayers);
			jsonWriter.WriteInt32(GameManager.Instance.World.Players.Count);
			jsonWriter.WriteRaw(ServerStats.jsonKeyHostiles);
			jsonWriter.WriteInt32(Hostiles.Instance.GetCount());
			jsonWriter.WriteRaw(ServerStats.jsonKeyAnimals);
			jsonWriter.WriteInt32(Animals.Instance.GetCount());
			jsonWriter.WriteEndObject();
			AbsRestApi.SendEnvelopedResult(_context, ref jsonWriter, HttpStatusCode.OK, null, null, null);
		}

		// Token: 0x0600D177 RID: 53623 RVA: 0x004BCF21 File Offset: 0x004BB121
		public override int DefaultPermissionLevel()
		{
			return 2000;
		}

		// Token: 0x0600D178 RID: 53624 RVA: 0x004B75A4 File Offset: 0x004B57A4
		public ServerStats()
			: base(null)
		{
		}

		// Token: 0x04009ED1 RID: 40657
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyGameTime = JsonWriter.GetEncodedPropertyNameWithBeginObject("gameTime");

		// Token: 0x04009ED2 RID: 40658
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyPlayers = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("players");

		// Token: 0x04009ED3 RID: 40659
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyHostiles = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("hostiles");

		// Token: 0x04009ED4 RID: 40660
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyAnimals = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("animals");
	}
}
