using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine.Scripting;
using Utf8Json;
using Webserver.Permissions;

namespace Webserver.WebAPI.APIs.Permissions
{
	// Token: 0x02001B12 RID: 6930
	[Preserve]
	public class WebApiTokens : AbsRestApi
	{
		// Token: 0x17001A22 RID: 6690
		// (get) Token: 0x0600D089 RID: 53385 RVA: 0x004BB4CD File Offset: 0x004B96CD
		public static AdminApiTokens ApiTokensInstance
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return AdminApiTokens.Instance;
			}
		}

		// Token: 0x0600D08A RID: 53386 RVA: 0x004BB4D4 File Offset: 0x004B96D4
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void HandleRestGet(RequestContext _context)
		{
			string requestPath = _context.RequestPath;
			JsonWriter jsonWriter;
			AbsRestApi.PrepareEnvelopedResult(out jsonWriter);
			if (string.IsNullOrEmpty(requestPath))
			{
				jsonWriter.WriteBeginArray();
				bool flag = true;
				foreach (KeyValuePair<string, AdminApiTokens.ApiToken> keyValuePair in WebApiTokens.ApiTokensInstance.GetTokens())
				{
					string text;
					AdminApiTokens.ApiToken apiToken;
					keyValuePair.Deconstruct(out text, out apiToken);
					AdminApiTokens.ApiToken apiToken2 = apiToken;
					if (!flag)
					{
						jsonWriter.WriteValueSeparator();
					}
					flag = false;
					this.writeTokenJson(ref jsonWriter, apiToken2);
				}
				jsonWriter.WriteEndArray();
				AbsRestApi.SendEnvelopedResult(_context, ref jsonWriter, HttpStatusCode.OK, null, null, null);
				return;
			}
			jsonWriter.WriteRaw(WebUtils.JsonEmptyData);
			AbsRestApi.SendEnvelopedResult(_context, ref jsonWriter, HttpStatusCode.BadRequest, null, null, null);
		}

		// Token: 0x0600D08B RID: 53387 RVA: 0x004BB59C File Offset: 0x004B979C
		[PublicizedFrom(EAccessModifier.Private)]
		public void writeTokenJson(ref JsonWriter _writer, AdminApiTokens.ApiToken _token)
		{
			_writer.WriteRaw(WebApiTokens.jsonKeyName);
			_writer.WriteString(_token.Name);
			_writer.WriteRaw(WebApiTokens.jsonKeySecret);
			_writer.WriteString(_token.Secret);
			_writer.WriteRaw(WebApiTokens.jsonKeyPermissionLevel);
			_writer.WriteInt32(_token.PermissionLevel);
			_writer.WriteEndObject();
		}

		// Token: 0x0600D08C RID: 53388 RVA: 0x004BB5F4 File Offset: 0x004B97F4
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void HandleRestPost(RequestContext _context, IDictionary<string, object> _jsonInput, byte[] _jsonInputData)
		{
			string requestPath = _context.RequestPath;
			if (string.IsNullOrEmpty(requestPath))
			{
				AbsRestApi.SendEmptyResponse(_context, HttpStatusCode.BadRequest, _jsonInputData, EApiErrorCode.NO_NAME, null);
				return;
			}
			string text;
			if (!JsonCommons.TryGetJsonField(_jsonInput, "secret", out text))
			{
				AbsRestApi.SendEmptyResponse(_context, HttpStatusCode.BadRequest, _jsonInputData, EApiErrorCode.NO_OR_INVALID_SECRET, null);
				return;
			}
			int num;
			if (!JsonCommons.TryGetJsonField(_jsonInput, "permissionLevel", out num))
			{
				AbsRestApi.SendEmptyResponse(_context, HttpStatusCode.BadRequest, _jsonInputData, EApiErrorCode.NO_OR_INVALID_PERMISSION_LEVEL, null);
				return;
			}
			WebApiTokens.ApiTokensInstance.AddToken(requestPath, text, num);
			AbsRestApi.SendEmptyResponse(_context, HttpStatusCode.Created, null, null, null);
		}

		// Token: 0x0600D08D RID: 53389 RVA: 0x004BB678 File Offset: 0x004B9878
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void HandleRestDelete(RequestContext _context)
		{
			string requestPath = _context.RequestPath;
			AbsRestApi.SendEmptyResponse(_context, WebApiTokens.ApiTokensInstance.RemoveToken(requestPath) ? HttpStatusCode.NoContent : HttpStatusCode.NotFound, null, null, null);
		}

		// Token: 0x17001A23 RID: 6691
		// (get) Token: 0x0600D08E RID: 53390 RVA: 0x00020119 File Offset: 0x0001E319
		public override bool AllowPostWithId
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return true;
			}
		}

		// Token: 0x0600D08F RID: 53391 RVA: 0x004BAA05 File Offset: 0x004B8C05
		public override int[] DefaultMethodPermissionLevels()
		{
			return new int[] { -2147483647, int.MinValue, int.MinValue, -2147483647, int.MinValue };
		}

		// Token: 0x0600D090 RID: 53392 RVA: 0x004B3AA8 File Offset: 0x004B1CA8
		public WebApiTokens()
			: base(null)
		{
		}

		// Token: 0x04009E0C RID: 40460
		[PublicizedFrom(EAccessModifier.Private)]
		public const string propertyName = "name";

		// Token: 0x04009E0D RID: 40461
		[PublicizedFrom(EAccessModifier.Private)]
		public const string propertySecret = "secret";

		// Token: 0x04009E0E RID: 40462
		[PublicizedFrom(EAccessModifier.Private)]
		public const string propertyPermissionLevel = "permissionLevel";

		// Token: 0x04009E0F RID: 40463
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyName = JsonWriter.GetEncodedPropertyNameWithBeginObject("name");

		// Token: 0x04009E10 RID: 40464
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeySecret = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("secret");

		// Token: 0x04009E11 RID: 40465
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyPermissionLevel = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("permissionLevel");
	}
}
