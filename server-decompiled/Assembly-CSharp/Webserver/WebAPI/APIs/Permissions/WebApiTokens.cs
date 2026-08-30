using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine.Scripting;
using Utf8Json;
using Webserver.Permissions;

namespace Webserver.WebAPI.APIs.Permissions
{
	// Token: 0x02001AE6 RID: 6886
	[Preserve]
	public class WebApiTokens : AbsRestApi
	{
		// Token: 0x17001980 RID: 6528
		// (get) Token: 0x0600CF31 RID: 53041 RVA: 0x004B7001 File Offset: 0x004B5201
		public static AdminApiTokens ApiTokensInstance
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return AdminApiTokens.Instance;
			}
		}

		// Token: 0x0600CF32 RID: 53042 RVA: 0x004B7008 File Offset: 0x004B5208
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

		// Token: 0x0600CF33 RID: 53043 RVA: 0x004B70D0 File Offset: 0x004B52D0
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

		// Token: 0x0600CF34 RID: 53044 RVA: 0x004B7128 File Offset: 0x004B5328
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

		// Token: 0x0600CF35 RID: 53045 RVA: 0x004B71AC File Offset: 0x004B53AC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void HandleRestDelete(RequestContext _context)
		{
			string requestPath = _context.RequestPath;
			AbsRestApi.SendEmptyResponse(_context, WebApiTokens.ApiTokensInstance.RemoveToken(requestPath) ? HttpStatusCode.NoContent : HttpStatusCode.NotFound, null, null, null);
		}

		// Token: 0x17001981 RID: 6529
		// (get) Token: 0x0600CF36 RID: 53046 RVA: 0x00020021 File Offset: 0x0001E221
		public override bool AllowPostWithId
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return true;
			}
		}

		// Token: 0x0600CF37 RID: 53047 RVA: 0x004B6539 File Offset: 0x004B4739
		public override int[] DefaultMethodPermissionLevels()
		{
			return new int[] { -2147483647, int.MinValue, int.MinValue, -2147483647, int.MinValue };
		}

		// Token: 0x0600CF38 RID: 53048 RVA: 0x004AF5DC File Offset: 0x004AD7DC
		public WebApiTokens()
			: base(null)
		{
		}

		// Token: 0x04009D46 RID: 40262
		[PublicizedFrom(EAccessModifier.Private)]
		public const string propertyName = "name";

		// Token: 0x04009D47 RID: 40263
		[PublicizedFrom(EAccessModifier.Private)]
		public const string propertySecret = "secret";

		// Token: 0x04009D48 RID: 40264
		[PublicizedFrom(EAccessModifier.Private)]
		public const string propertyPermissionLevel = "permissionLevel";

		// Token: 0x04009D49 RID: 40265
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyName = JsonWriter.GetEncodedPropertyNameWithBeginObject("name");

		// Token: 0x04009D4A RID: 40266
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeySecret = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("secret");

		// Token: 0x04009D4B RID: 40267
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyPermissionLevel = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("permissionLevel");
	}
}
