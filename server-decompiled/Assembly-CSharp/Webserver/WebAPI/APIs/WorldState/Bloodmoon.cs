using System;
using System.Net;
using UnityEngine.Scripting;
using Utf8Json;

namespace Webserver.WebAPI.APIs.WorldState
{
	// Token: 0x02001B02 RID: 6914
	[Preserve]
	public class Bloodmoon : AbsRestApi
	{
		// Token: 0x0600D03A RID: 53306 RVA: 0x004B9598 File Offset: 0x004B7798
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void HandleRestGet(RequestContext _context)
		{
			ulong worldTime = GameManager.Instance.World.worldTime;
			ValueTuple<int, int, int> valueTuple = GameUtils.WorldTimeToElements(worldTime);
			int item = valueTuple.Item1;
			int item2 = valueTuple.Item2;
			int item3 = valueTuple.Item3;
			int @int = GameStats.GetInt(EnumUtils.Parse<EnumGameStats>("BloodMoonDay", false));
			ValueTuple<int, int> valueTuple2 = GameUtils.CalcDuskDawnHours(GamePrefs.GetInt(EnumUtils.Parse<EnumGamePrefs>("DayLightLength", false)));
			JsonWriter jsonWriter;
			AbsRestApi.PrepareEnvelopedResult(out jsonWriter);
			jsonWriter.WriteRaw(Bloodmoon.jsonKeyGameTime);
			JsonCommons.WriteGameTimeObject(ref jsonWriter, item, item2, item3);
			jsonWriter.WriteRaw(Bloodmoon.jsonKeyBloodmoonActive);
			jsonWriter.WriteBoolean(GameUtils.IsBloodMoonTime(worldTime, valueTuple2, @int));
			jsonWriter.WriteRaw(Bloodmoon.jsonKeyNextBloodmoon);
			JsonCommons.WriteGameTimeObject(ref jsonWriter, @int, valueTuple2.Item1, 0);
			jsonWriter.WriteRaw(Bloodmoon.jsonKeyNextBloodmoonEnd);
			JsonCommons.WriteGameTimeObject(ref jsonWriter, @int + 1, valueTuple2.Item2, 0);
			jsonWriter.WriteEndObject();
			AbsRestApi.SendEnvelopedResult(_context, ref jsonWriter, HttpStatusCode.OK, null, null, null);
		}

		// Token: 0x0600D03B RID: 53307 RVA: 0x004B3AA8 File Offset: 0x004B1CA8
		public Bloodmoon()
			: base(null)
		{
		}

		// Token: 0x04009DB1 RID: 40369
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyGameTime = JsonWriter.GetEncodedPropertyNameWithBeginObject("gameTime");

		// Token: 0x04009DB2 RID: 40370
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyBloodmoonActive = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("bloodmoonActive");

		// Token: 0x04009DB3 RID: 40371
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyNextBloodmoon = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("nextBloodmoon");

		// Token: 0x04009DB4 RID: 40372
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyNextBloodmoonEnd = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("nextBloodmoonEnd");
	}
}
