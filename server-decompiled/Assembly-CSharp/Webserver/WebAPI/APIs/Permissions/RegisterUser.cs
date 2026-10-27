using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine.Scripting;
using Utf8Json;
using Webserver.Permissions;
using Webserver.UrlHandlers;

namespace Webserver.WebAPI.APIs.Permissions
{
	// Token: 0x02001B41 RID: 6977
	[Preserve]
	public class RegisterUser : AbsRestApi
	{
		// Token: 0x0600D18E RID: 53646 RVA: 0x004BCEB4 File Offset: 0x004BB0B4
		public RegisterUser(Web _parentWeb)
			: base(_parentWeb, null)
		{
		}

		// Token: 0x0600D18F RID: 53647 RVA: 0x004BE8AC File Offset: 0x004BCAAC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void HandleRestGet(RequestContext _context)
		{
			string requestPath = _context.RequestPath;
			if (string.IsNullOrEmpty(requestPath))
			{
				AbsRestApi.SendEmptyResponse(_context, HttpStatusCode.BadRequest, null, EApiErrorCode.MISSING_TOKEN, null);
				return;
			}
			UserRegistrationTokens.RegistrationData registrationData;
			if (!UserRegistrationTokens.TryValidate(requestPath, out registrationData))
			{
				AbsRestApi.SendEmptyResponse(_context, HttpStatusCode.NotFound, null, EApiErrorCode.INVALID_OR_EXPIRED_TOKEN, null);
				return;
			}
			JsonWriter jsonWriter;
			AbsRestApi.PrepareEnvelopedResult(out jsonWriter);
			jsonWriter.WriteRaw(RegisterUser.jsonPlayerNameKey);
			jsonWriter.WriteString(registrationData.PlayerName);
			jsonWriter.WriteRaw(RegisterUser.jsonExpirationKey);
			jsonWriter.WriteDouble((registrationData.ExpiryTime - DateTime.Now).TotalSeconds);
			jsonWriter.WriteEndObject();
			AbsRestApi.SendEnvelopedResult(_context, ref jsonWriter, HttpStatusCode.OK, null, null, null);
		}

		// Token: 0x0600D190 RID: 53648 RVA: 0x004BE954 File Offset: 0x004BCB54
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void HandleRestPost(RequestContext _context, IDictionary<string, object> _jsonInput, byte[] _jsonInputData)
		{
			string text;
			if (!JsonCommons.TryGetJsonField(_jsonInput, "token", out text))
			{
				AbsRestApi.SendEmptyResponse(_context, HttpStatusCode.BadRequest, _jsonInputData, EApiErrorCode.MISSING_TOKEN, null);
				return;
			}
			string text2;
			if (!JsonCommons.TryGetJsonField(_jsonInput, "username", out text2))
			{
				AbsRestApi.SendEmptyResponse(_context, HttpStatusCode.BadRequest, _jsonInputData, EApiErrorCode.MISSING_USERNAME, null);
				return;
			}
			string text3;
			if (!JsonCommons.TryGetJsonField(_jsonInput, "password", out text3))
			{
				AbsRestApi.SendEmptyResponse(_context, HttpStatusCode.BadRequest, _jsonInputData, EApiErrorCode.MISSING_PASSWORD, null);
				return;
			}
			UserRegistrationTokens.RegistrationData registrationData;
			if (!UserRegistrationTokens.TryValidate(text, out registrationData))
			{
				AbsRestApi.SendEmptyResponse(_context, HttpStatusCode.Unauthorized, null, EApiErrorCode.INVALID_OR_EXPIRED_TOKEN, null);
				return;
			}
			if (!RegisterUser.userValidationRegex.IsMatch(text2))
			{
				AbsRestApi.SendEmptyResponse(_context, HttpStatusCode.Unauthorized, _jsonInputData, EApiErrorCode.INVALID_USERNAME, null);
				return;
			}
			if (!RegisterUser.passValidationRegex.IsMatch(text3))
			{
				AbsRestApi.SendEmptyResponse(_context, HttpStatusCode.Unauthorized, _jsonInputData, EApiErrorCode.INVALID_PASSWORD, null);
				return;
			}
			AdminWebUsers.WebUser webUser;
			if (AdminWebUsers.Instance.GetUsers().TryGetValue(text2, out webUser) && (!object.Equals(webUser.PlatformUser, registrationData.PlatformUserId) || !object.Equals(webUser.CrossPlatformUser, registrationData.CrossPlatformUserId)))
			{
				AbsRestApi.SendEmptyResponse(_context, HttpStatusCode.Unauthorized, _jsonInputData, EApiErrorCode.DUPLICATE_USERNAME, null);
				return;
			}
			string text4 = ((registrationData.CrossPlatformUserId == null) ? "" : (", crossplatform ID " + registrationData.CrossPlatformUserId.CombinedString));
			Log.Out(string.Concat(new string[]
			{
				"[Web] User registered: Username '",
				text2,
				"' for platform ID ",
				registrationData.PlatformUserId.CombinedString,
				text4
			}));
			AdminWebUsers.WebUser webUser2;
			if (AdminWebUsers.Instance.HasUser(registrationData.PlatformUserId, registrationData.CrossPlatformUserId, out webUser2))
			{
				Log.Out("[Web] Re-registration, replacing existing username '" + webUser2.Name + "'");
				AdminWebUsers.Instance.RemoveUser(webUser2.Name);
			}
			AdminWebUsers.Instance.AddUser(text2, text3, registrationData.PlatformUserId, registrationData.CrossPlatformUserId);
			string text5 = _context.Request.RemoteEndPoint.ToString();
			SessionHandler.HandleUserIdLogin(this.ParentWeb.ConnectionHandler, _context, text5, "User/pass", text2, registrationData.PlatformUserId, registrationData.CrossPlatformUserId);
			_context.Response.StatusCode = 201;
			_context.Response.ContentType = "text/plain";
			_context.Response.ContentEncoding = Encoding.UTF8;
			_context.Response.ContentLength64 = 0L;
		}

		// Token: 0x0600D191 RID: 53649 RVA: 0x004BCF21 File Offset: 0x004BB121
		public override int DefaultPermissionLevel()
		{
			return 2000;
		}

		// Token: 0x04009EE3 RID: 40675
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonPlayerNameKey = JsonWriter.GetEncodedPropertyNameWithBeginObject("playerName");

		// Token: 0x04009EE4 RID: 40676
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonExpirationKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("expirationSeconds");

		// Token: 0x04009EE5 RID: 40677
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly Regex userValidationRegex = new Regex("^\\w{4,16}$", RegexOptions.Compiled | RegexOptions.ECMAScript);

		// Token: 0x04009EE6 RID: 40678
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly Regex passValidationRegex = new Regex("^\\w{4,16}$", RegexOptions.Compiled | RegexOptions.ECMAScript);
	}
}
