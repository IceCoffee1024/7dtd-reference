using System;
using System.Net;
using UnityEngine.Scripting;
using Utf8Json;
using Webserver.LiveData;

namespace Webserver.WebAPI.APIs.ServerState
{
	// Token: 0x02001B0C RID: 6924
	[Preserve]
	public class ServerStats : AbsRestApi
	{
		// Token: 0x0600D062 RID: 53346 RVA: 0x004BA6C0 File Offset: 0x004B88C0
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

		// Token: 0x0600D063 RID: 53347 RVA: 0x004B9425 File Offset: 0x004B7625
		public override int DefaultPermissionLevel()
		{
			return 2000;
		}

		// Token: 0x0600D064 RID: 53348 RVA: 0x004B3AA8 File Offset: 0x004B1CA8
		public ServerStats()
			: base(null)
		{
		}

		// Token: 0x04009DE8 RID: 40424
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyGameTime = JsonWriter.GetEncodedPropertyNameWithBeginObject("gameTime");

		// Token: 0x04009DE9 RID: 40425
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyPlayers = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("players");

		// Token: 0x04009DEA RID: 40426
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyHostiles = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("hostiles");

		// Token: 0x04009DEB RID: 40427
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyAnimals = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("animals");
	}
}
