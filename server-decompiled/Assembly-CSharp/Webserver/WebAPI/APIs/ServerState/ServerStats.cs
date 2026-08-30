using System;
using System.Net;
using UnityEngine.Scripting;
using Utf8Json;
using Webserver.LiveData;

namespace Webserver.WebAPI.APIs.ServerState
{
	// Token: 0x02001AE0 RID: 6880
	[Preserve]
	public class ServerStats : AbsRestApi
	{
		// Token: 0x0600CF0A RID: 53002 RVA: 0x004B61F4 File Offset: 0x004B43F4
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

		// Token: 0x0600CF0B RID: 53003 RVA: 0x004B4F59 File Offset: 0x004B3159
		public override int DefaultPermissionLevel()
		{
			return 2000;
		}

		// Token: 0x0600CF0C RID: 53004 RVA: 0x004AF5DC File Offset: 0x004AD7DC
		public ServerStats()
			: base(null)
		{
		}

		// Token: 0x04009D22 RID: 40226
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyGameTime = JsonWriter.GetEncodedPropertyNameWithBeginObject("gameTime");

		// Token: 0x04009D23 RID: 40227
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyPlayers = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("players");

		// Token: 0x04009D24 RID: 40228
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyHostiles = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("hostiles");

		// Token: 0x04009D25 RID: 40229
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyAnimals = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("animals");
	}
}
