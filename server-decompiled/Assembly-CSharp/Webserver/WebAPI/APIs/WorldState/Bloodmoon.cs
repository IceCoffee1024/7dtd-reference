using System;
using System.Net;
using UnityEngine.Scripting;
using Utf8Json;

namespace Webserver.WebAPI.APIs.WorldState
{
	// Token: 0x02001AD6 RID: 6870
	[Preserve]
	public class Bloodmoon : AbsRestApi
	{
		// Token: 0x0600CEE2 RID: 52962 RVA: 0x004B50CC File Offset: 0x004B32CC
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

		// Token: 0x0600CEE3 RID: 52963 RVA: 0x004AF5DC File Offset: 0x004AD7DC
		public Bloodmoon()
			: base(null)
		{
		}

		// Token: 0x04009CEB RID: 40171
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyGameTime = JsonWriter.GetEncodedPropertyNameWithBeginObject("gameTime");

		// Token: 0x04009CEC RID: 40172
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyBloodmoonActive = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("bloodmoonActive");

		// Token: 0x04009CED RID: 40173
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyNextBloodmoon = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("nextBloodmoon");

		// Token: 0x04009CEE RID: 40174
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyNextBloodmoonEnd = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("nextBloodmoonEnd");
	}
}
