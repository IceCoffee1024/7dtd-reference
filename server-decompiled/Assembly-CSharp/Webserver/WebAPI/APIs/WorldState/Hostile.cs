using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine.Scripting;
using Utf8Json;
using Webserver.LiveData;

namespace Webserver.WebAPI.APIs.WorldState
{
	// Token: 0x02001B34 RID: 6964
	[Preserve]
	[PublicizedFrom(EAccessModifier.Internal)]
	public class Hostile : AbsRestApi
	{
		// Token: 0x0600D151 RID: 53585 RVA: 0x004BD1C0 File Offset: 0x004BB3C0
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

		// Token: 0x0600D152 RID: 53586 RVA: 0x004BD2E8 File Offset: 0x004BB4E8
		public Hostile()
			: base(null)
		{
		}

		// Token: 0x04009E9E RID: 40606
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly List<EntityEnemy> entities = new List<EntityEnemy>();

		// Token: 0x04009E9F RID: 40607
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyId = JsonWriter.GetEncodedPropertyNameWithBeginObject("id");

		// Token: 0x04009EA0 RID: 40608
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyName = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("name");

		// Token: 0x04009EA1 RID: 40609
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyPosition = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("position");
	}
}
