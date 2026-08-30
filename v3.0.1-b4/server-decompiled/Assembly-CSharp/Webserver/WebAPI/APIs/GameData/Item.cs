using System;
using System.Net;
using UnityEngine.Scripting;
using Utf8Json;

namespace Webserver.WebAPI.APIs.GameData
{
	// Token: 0x02001AC4 RID: 6852
	[Preserve]
	[PublicizedFrom(EAccessModifier.Internal)]
	public class Item : AbsRestApi
	{
		// Token: 0x0600CDD2 RID: 52690 RVA: 0x004B1974 File Offset: 0x004AFB74
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

		// Token: 0x0600CDD3 RID: 52691 RVA: 0x004B1A3C File Offset: 0x004AFC3C
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void HandleRestGet(RequestContext _context)
		{
			JsonWriter jsonWriter;
			AbsRestApi.PrepareEnvelopedResult(out jsonWriter);
			jsonWriter.WriteRaw(this.allItemsSerialized);
			AbsRestApi.SendEnvelopedResult(_context, ref jsonWriter, HttpStatusCode.OK, null, null, null);
		}

		// Token: 0x0600CDD4 RID: 52692 RVA: 0x004AEB65 File Offset: 0x004ACD65
		public override int DefaultPermissionLevel()
		{
			return 2000;
		}

		// Token: 0x04009C38 RID: 39992
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyName = JsonWriter.GetEncodedPropertyNameWithBeginObject("name");

		// Token: 0x04009C39 RID: 39993
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyLocalizedName = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("localizedName");

		// Token: 0x04009C3A RID: 39994
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyIsBlock = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("isBlock");

		// Token: 0x04009C3B RID: 39995
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly byte[] allItemsSerialized;
	}
}
