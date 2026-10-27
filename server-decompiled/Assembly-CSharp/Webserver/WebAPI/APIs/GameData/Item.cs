using System;
using System.Net;
using UnityEngine.Scripting;
using Utf8Json;

namespace Webserver.WebAPI.APIs.GameData
{
	// Token: 0x02001B48 RID: 6984
	[Preserve]
	[PublicizedFrom(EAccessModifier.Internal)]
	public class Item : AbsRestApi
	{
		// Token: 0x0600D1C7 RID: 53703 RVA: 0x004BFD30 File Offset: 0x004BDF30
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

		// Token: 0x0600D1C8 RID: 53704 RVA: 0x004BFDF8 File Offset: 0x004BDFF8
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void HandleRestGet(RequestContext _context)
		{
			JsonWriter jsonWriter;
			AbsRestApi.PrepareEnvelopedResult(out jsonWriter);
			jsonWriter.WriteRaw(this.allItemsSerialized);
			AbsRestApi.SendEnvelopedResult(_context, ref jsonWriter, HttpStatusCode.OK, null, null, null);
		}

		// Token: 0x0600D1C9 RID: 53705 RVA: 0x004BCF21 File Offset: 0x004BB121
		public override int DefaultPermissionLevel()
		{
			return 2000;
		}

		// Token: 0x04009F18 RID: 40728
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyName = JsonWriter.GetEncodedPropertyNameWithBeginObject("name");

		// Token: 0x04009F19 RID: 40729
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyLocalizedName = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("localizedName");

		// Token: 0x04009F1A RID: 40730
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyIsBlock = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("isBlock");

		// Token: 0x04009F1B RID: 40731
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly byte[] allItemsSerialized;
	}
}
