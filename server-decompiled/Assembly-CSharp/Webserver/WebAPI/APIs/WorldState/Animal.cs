using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine.Scripting;
using Utf8Json;
using Webserver.LiveData;

namespace Webserver.WebAPI.APIs.WorldState
{
	// Token: 0x02001B01 RID: 6913
	[Preserve]
	[PublicizedFrom(EAccessModifier.Internal)]
	public class Animal : AbsRestApi
	{
		// Token: 0x0600D037 RID: 53303 RVA: 0x004B942C File Offset: 0x004B762C
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void HandleRestGet(RequestContext _context)
		{
			JsonWriter jsonWriter;
			AbsRestApi.PrepareEnvelopedResult(out jsonWriter);
			jsonWriter.WriteBeginArray();
			List<EntityAnimal> list = this.entities;
			lock (list)
			{
				Animals.Instance.Get(this.entities);
				for (int i = 0; i < this.entities.Count; i++)
				{
					if (i > 0)
					{
						jsonWriter.WriteValueSeparator();
					}
					EntityAlive entityAlive = this.entities[i];
					Vector3i vector3i = new Vector3i(entityAlive.GetPosition());
					jsonWriter.WriteRaw(Animal.jsonKeyId);
					jsonWriter.WriteInt32(entityAlive.entityId);
					jsonWriter.WriteRaw(Animal.jsonKeyName);
					jsonWriter.WriteString((!string.IsNullOrEmpty(entityAlive.EntityName)) ? entityAlive.EntityName : string.Format("animal class #{0}", entityAlive.entityClass));
					jsonWriter.WriteRaw(Animal.jsonKeyPosition);
					JsonCommons.WriteVector3I(ref jsonWriter, vector3i);
					jsonWriter.WriteEndObject();
				}
			}
			jsonWriter.WriteEndArray();
			AbsRestApi.SendEnvelopedResult(_context, ref jsonWriter, HttpStatusCode.OK, null, null, null);
		}

		// Token: 0x0600D038 RID: 53304 RVA: 0x004B9554 File Offset: 0x004B7754
		public Animal()
			: base(null)
		{
		}

		// Token: 0x04009DAD RID: 40365
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly List<EntityAnimal> entities = new List<EntityAnimal>();

		// Token: 0x04009DAE RID: 40366
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyId = JsonWriter.GetEncodedPropertyNameWithBeginObject("id");

		// Token: 0x04009DAF RID: 40367
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyName = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("name");

		// Token: 0x04009DB0 RID: 40368
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyPosition = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("position");
	}
}
