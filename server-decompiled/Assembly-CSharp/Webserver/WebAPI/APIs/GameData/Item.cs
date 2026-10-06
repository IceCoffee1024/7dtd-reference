using System;
using System.Net;
using UnityEngine.Scripting;
using Utf8Json;

namespace Webserver.WebAPI.APIs.GameData
{
	// Token: 0x02001B17 RID: 6935
	[Preserve]
	[PublicizedFrom(EAccessModifier.Internal)]
	public class Item : AbsRestApi
	{
		// Token: 0x0600D0B3 RID: 53427 RVA: 0x004BC234 File Offset: 0x004BA434
		public Item(Web _parent)
			: base(null)
		{
			JsonWriter jsonWriter = default(JsonWriter);
			jsonWriter.WriteBeginArray();
			int num = 0;
			for (int i = 0; i < ItemClass.list.Length; i++)
			{
				ItemClass itemClass = ItemClass.list[i];
				if (itemClass != null)
				{
					if (num > 0)
					{
						jsonWriter.WriteValueSeparator();
					}
					num++;
					string name = itemClass.Name;
					string localizedItemName = itemClass.GetLocalizedItemName();
					bool flag = itemClass.IsBlock();
					jsonWriter.WriteRaw(Item.jsonKeyName);
					jsonWriter.WriteString(name);
					jsonWriter.WriteRaw(Item.jsonKeyLocalizedName);
					jsonWriter.WriteString(localizedItemName);
					jsonWriter.WriteRaw(Item.jsonKeyIsBlock);
					jsonWriter.WriteBoolean(flag);
					jsonWriter.WriteEndObject();
				}
			}
			jsonWriter.WriteEndArray();
			this.allItemsSerialized = jsonWriter.ToUtf8ByteArray();
		}

		// Token: 0x0600D0B4 RID: 53428 RVA: 0x004BC2FC File Offset: 0x004BA4FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void HandleRestGet(RequestContext _context)
		{
			JsonWriter jsonWriter;
			AbsRestApi.PrepareEnvelopedResult(out jsonWriter);
			jsonWriter.WriteRaw(this.allItemsSerialized);
			AbsRestApi.SendEnvelopedResult(_context, ref jsonWriter, HttpStatusCode.OK, null, null, null);
		}

		// Token: 0x0600D0B5 RID: 53429 RVA: 0x004B9425 File Offset: 0x004B7625
		public override int DefaultPermissionLevel()
		{
			return 2000;
		}

		// Token: 0x04009E2F RID: 40495
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyName = JsonWriter.GetEncodedPropertyNameWithBeginObject("name");

		// Token: 0x04009E30 RID: 40496
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyLocalizedName = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("localizedName");

		// Token: 0x04009E31 RID: 40497
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyIsBlock = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("isBlock");

		// Token: 0x04009E32 RID: 40498
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly byte[] allItemsSerialized;
	}
}
