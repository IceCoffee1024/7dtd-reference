using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine.Scripting;
using Utf8Json;
using Webserver.LiveData;

namespace Webserver.WebAPI.APIs.WorldState
{
	// Token: 0x02001AD7 RID: 6871
	[Preserve]
	[PublicizedFrom(EAccessModifier.Internal)]
	public class Hostile : AbsRestApi
	{
		// Token: 0x0600CEE5 RID: 52965 RVA: 0x004B51F8 File Offset: 0x004B33F8
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void HandleRestGet(RequestContext _context)
		{
			JsonWriter jsonWriter;
			AbsRestApi.PrepareEnvelopedResult(out jsonWriter);
			jsonWriter.WriteBeginArray();
			List<EntityEnemy> list = this.entities;
			lock (list)
			{
				Hostiles.Instance.Get(this.entities);
				for (int i = 0; i < this.entities.Count; i++)
				{
					if (i > 0)
					{
						jsonWriter.WriteValueSeparator();
					}
					EntityAlive entityAlive = this.entities[i];
					Vector3i vector3i = new Vector3i(entityAlive.GetPosition());
					jsonWriter.WriteRaw(Hostile.jsonKeyId);
					jsonWriter.WriteInt32(entityAlive.entityId);
					jsonWriter.WriteRaw(Hostile.jsonKeyName);
					jsonWriter.WriteString((!string.IsNullOrEmpty(entityAlive.EntityName)) ? entityAlive.EntityName : string.Format("enemy class #{0}", entityAlive.entityClass));
					jsonWriter.WriteRaw(Hostile.jsonKeyPosition);
					JsonCommons.WriteVector3I(ref jsonWriter, vector3i);
					jsonWriter.WriteEndObject();
				}
			}
			jsonWriter.WriteEndArray();
			AbsRestApi.SendEnvelopedResult(_context, ref jsonWriter, HttpStatusCode.OK, null, null, null);
		}

		// Token: 0x0600CEE6 RID: 52966 RVA: 0x004B5320 File Offset: 0x004B3520
		public Hostile()
			: base(null)
		{
		}

		// Token: 0x04009CEF RID: 40175
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly List<EntityEnemy> entities = new List<EntityEnemy>();

		// Token: 0x04009CF0 RID: 40176
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyId = JsonWriter.GetEncodedPropertyNameWithBeginObject("id");

		// Token: 0x04009CF1 RID: 40177
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyName = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("name");

		// Token: 0x04009CF2 RID: 40178
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyPosition = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("position");
	}
}
