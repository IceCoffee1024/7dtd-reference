using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine.Scripting;
using Utf8Json;

namespace Webserver.WebAPI.APIs.GameData
{
	// Token: 0x02001B47 RID: 6983
	[Preserve]
	[PublicizedFrom(EAccessModifier.Internal)]
	public class EntityClass : AbsRestApi
	{
		// Token: 0x0600D1C3 RID: 53699 RVA: 0x004BFBA0 File Offset: 0x004BDDA0
		public EntityClass(Web _parent)
			: base(null)
		{
			JsonWriter jsonWriter = default(JsonWriter);
			jsonWriter.WriteBeginArray();
			int num = 0;
			int num2 = 0;
			foreach (KeyValuePair<int, EntityClass> keyValuePair in EntityClass.list.Dict)
			{
				int num3;
				EntityClass entityClass;
				keyValuePair.Deconstruct(out num3, out entityClass);
				int num4 = num3;
				EntityClass entityClass2 = entityClass;
				if (num > 0)
				{
					jsonWriter.WriteValueSeparator();
				}
				num++;
				string entityClassName = entityClass2.entityClassName;
				EntityClass.UserSpawnType userSpawnType = entityClass2.userSpawnType;
				jsonWriter.WriteRaw(EntityClass.jsonKeyName);
				jsonWriter.WriteString(entityClassName);
				jsonWriter.WriteRaw(EntityClass.jsonKeyId);
				jsonWriter.WriteInt32(num4);
				if (entityClass2.userSpawnType != EntityClass.UserSpawnType.None)
				{
					num2++;
					jsonWriter.WriteRaw(EntityClass.jsonKeyCommandIndex);
					jsonWriter.WriteInt32(num2);
				}
				jsonWriter.WriteRaw(EntityClass.jsonKeyManualSpawnType);
				jsonWriter.WriteString(userSpawnType.ToStringCached<EntityClass.UserSpawnType>());
				jsonWriter.WriteEndObject();
			}
			jsonWriter.WriteEndArray();
			this.allClassesSerialized = jsonWriter.ToUtf8ByteArray();
		}

		// Token: 0x0600D1C4 RID: 53700 RVA: 0x004BFCC0 File Offset: 0x004BDEC0
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void HandleRestGet(RequestContext _context)
		{
			JsonWriter jsonWriter;
			AbsRestApi.PrepareEnvelopedResult(out jsonWriter);
			jsonWriter.WriteRaw(this.allClassesSerialized);
			AbsRestApi.SendEnvelopedResult(_context, ref jsonWriter, HttpStatusCode.OK, null, null, null);
		}

		// Token: 0x0600D1C5 RID: 53701 RVA: 0x004BCF21 File Offset: 0x004BB121
		public override int DefaultPermissionLevel()
		{
			return 2000;
		}

		// Token: 0x04009F13 RID: 40723
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyName = JsonWriter.GetEncodedPropertyNameWithBeginObject("name");

		// Token: 0x04009F14 RID: 40724
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyId = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("id");

		// Token: 0x04009F15 RID: 40725
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyCommandIndex = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("commandId");

		// Token: 0x04009F16 RID: 40726
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyManualSpawnType = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("manualSpawnType");

		// Token: 0x04009F17 RID: 40727
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly byte[] allClassesSerialized;
	}
}
