using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine.Scripting;
using Utf8Json;
using Webserver.Permissions;

namespace Webserver.WebAPI.APIs.Permissions
{
	// Token: 0x02001B45 RID: 6981
	[Preserve]
	public class WebUsers : AbsRestApi
	{
		// Token: 0x17001A6E RID: 6766
		// (get) Token: 0x0600D1AF RID: 53679 RVA: 0x004BF620 File Offset: 0x004BD820
		public static AdminWebUsers WebUsersInstance
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return AdminWebUsers.Instance;
			}
		}

		// Token: 0x0600D1B0 RID: 53680 RVA: 0x004BF628 File Offset: 0x004BD828
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
				foreach (KeyValuePair<string, AdminWebUsers.WebUser> keyValuePair in WebUsers.WebUsersInstance.GetUsers())
				{
					string text;
					AdminWebUsers.WebUser webUser;
					keyValuePair.Deconstruct(out text, out webUser);
					AdminWebUsers.WebUser webUser2 = webUser;
					if (!flag)
					{
						jsonWriter.WriteValueSeparator();
					}
					flag = false;
					this.writeUserJson(ref jsonWriter, webUser2);
				}
				jsonWriter.WriteEndArray();
				AbsRestApi.SendEnvelopedResult(_context, ref jsonWriter, HttpStatusCode.OK, null, null, null);
				return;
			}
			jsonWriter.WriteRaw(WebUtils.JsonEmptyData);
			AbsRestApi.SendEnvelopedResult(_context, ref jsonWriter, HttpStatusCode.BadRequest, null, null, null);
		}

		// Token: 0x0600D1B1 RID: 53681 RVA: 0x004BF6F0 File Offset: 0x004BD8F0
		[PublicizedFrom(EAccessModifier.Private)]
		public void writeUserJson(ref JsonWriter _writer, AdminWebUsers.WebUser _user)
		{
			_writer.WriteRaw(WebUsers.jsonKeyName);
			_writer.WriteString(_user.Name ?? "");
			_writer.WriteRaw(WebUsers.jsonKeyPlatformUserId);
			JsonCommons.WritePlatformUserIdentifier(ref _writer, _user.PlatformUser);
			_writer.WriteRaw(WebUsers.jsonKeyCrossplatformUserId);
			JsonCommons.WritePlatformUserIdentifier(ref _writer, _user.CrossPlatformUser);
			_writer.WriteEndObject();
		}

		// Token: 0x0600D1B2 RID: 53682 RVA: 0x004BF754 File Offset: 0x004BD954
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void HandleRestPost(RequestContext _context, IDictionary<string, object> _jsonInput, byte[] _jsonInputData)
		{
			string text;
			if (!this.TryParseName(_context, _jsonInputData, out text))
			{
				return;
			}
			string text2;
			if (!JsonCommons.TryGetJsonField(_jsonInput, "password", out text2))
			{
				AbsRestApi.SendEmptyResponse(_context, HttpStatusCode.BadRequest, _jsonInputData, EApiErrorCode.NO_OR_INVALID_PASSWORD, null);
				return;
			}
			IDictionary<string, object> dictionary;
			if (!JsonCommons.TryGetJsonField(_jsonInput, "platformUserId", out dictionary))
			{
				AbsRestApi.SendEmptyResponse(_context, HttpStatusCode.BadRequest, _jsonInputData, EApiErrorCode.NO_PLATFORM_USER_ID, null);
				return;
			}
			PlatformUserIdentifierAbs platformUserIdentifierAbs;
			if (!JsonCommons.TryReadPlatformUserIdentifier(dictionary, out platformUserIdentifierAbs))
			{
				AbsRestApi.SendEmptyResponse(_context, HttpStatusCode.BadRequest, _jsonInputData, EApiErrorCode.INVALID_PLATFORM_USER_ID, null);
				return;
			}
			PlatformUserIdentifierAbs platformUserIdentifierAbs2 = null;
			if (JsonCommons.TryGetJsonField(_jsonInput, "crossplatformUserId", out dictionary) && !JsonCommons.TryReadPlatformUserIdentifier(dictionary, out platformUserIdentifierAbs2))
			{
				AbsRestApi.SendEmptyResponse(_context, HttpStatusCode.BadRequest, _jsonInputData, EApiErrorCode.INVALID_CROSSPLATFORM_USER_ID, null);
				return;
			}
			WebUsers.WebUsersInstance.AddUser(text, text2, platformUserIdentifierAbs, platformUserIdentifierAbs2);
			AbsRestApi.SendEmptyResponse(_context, HttpStatusCode.Created, null, null, null);
		}

		// Token: 0x0600D1B3 RID: 53683 RVA: 0x004BF810 File Offset: 0x004BDA10
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void HandleRestDelete(RequestContext _context)
		{
			string text;
			if (!this.TryParseName(_context, null, out text))
			{
				return;
			}
			AbsRestApi.SendEmptyResponse(_context, WebUsers.WebUsersInstance.RemoveUser(text) ? HttpStatusCode.NoContent : HttpStatusCode.NotFound, null, null, null);
		}

		// Token: 0x0600D1B4 RID: 53684 RVA: 0x004BF850 File Offset: 0x004BDA50
		[PublicizedFrom(EAccessModifier.Private)]
		public bool TryParseName(RequestContext _context, byte[] _jsonInputData, out string _userName)
		{
			string requestPath = _context.RequestPath;
			_userName = null;
			if (string.IsNullOrEmpty(requestPath))
			{
				AbsRestApi.SendEmptyResponse(_context, HttpStatusCode.BadRequest, _jsonInputData, EApiErrorCode.NO_USERNAME, null);
				return false;
			}
			_userName = requestPath;
			return true;
		}

		// Token: 0x17001A6F RID: 6767
		// (get) Token: 0x0600D1B5 RID: 53685 RVA: 0x00020119 File Offset: 0x0001E319
		public override bool AllowPostWithId
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return true;
			}
		}

		// Token: 0x0600D1B6 RID: 53686 RVA: 0x004BE501 File Offset: 0x004BC701
		public override int[] DefaultMethodPermissionLevels()
		{
			return new int[] { -2147483647, int.MinValue, int.MinValue, -2147483647, int.MinValue };
		}

		// Token: 0x0600D1B7 RID: 53687 RVA: 0x004B75A4 File Offset: 0x004B57A4
		public WebUsers()
			: base(null)
		{
		}

		// Token: 0x04009F04 RID: 40708
		[PublicizedFrom(EAccessModifier.Private)]
		public const string propertyName = "name";

		// Token: 0x04009F05 RID: 40709
		[PublicizedFrom(EAccessModifier.Private)]
		public const string propertyPassword = "password";

		// Token: 0x04009F06 RID: 40710
		[PublicizedFrom(EAccessModifier.Private)]
		public const string propertyPlatformUserId = "platformUserId";

		// Token: 0x04009F07 RID: 40711
		[PublicizedFrom(EAccessModifier.Private)]
		public const string propertyCrossplatformUserId = "crossplatformUserId";

		// Token: 0x04009F08 RID: 40712
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyName = JsonWriter.GetEncodedPropertyNameWithBeginObject("name");

		// Token: 0x04009F09 RID: 40713
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyPlatformUserId = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("platformUserId");

		// Token: 0x04009F0A RID: 40714
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyCrossplatformUserId = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("crossplatformUserId");
	}
}
