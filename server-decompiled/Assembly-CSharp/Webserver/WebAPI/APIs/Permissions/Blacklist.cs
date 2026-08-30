using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine.Scripting;
using Utf8Json;

namespace Webserver.WebAPI.APIs.Permissions
{
	// Token: 0x02001AE1 RID: 6881
	[Preserve]
	public class Blacklist : AbsRestApi
	{
		// Token: 0x1700197A RID: 6522
		// (get) Token: 0x0600CF0E RID: 53006 RVA: 0x004B62FC File Offset: 0x004B44FC
		public static AdminBlacklist BlacklistInstance
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return GameManager.Instance.adminTools.Blacklist;
			}
		}

		// Token: 0x0600CF0F RID: 53007 RVA: 0x004B6310 File Offset: 0x004B4510
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
				foreach (AdminBlacklist.BannedUser bannedUser in Blacklist.BlacklistInstance.GetBanned())
				{
					if (!flag)
					{
						jsonWriter.WriteValueSeparator();
					}
					flag = false;
					this.writeBan(ref jsonWriter, bannedUser);
				}
				jsonWriter.WriteEndArray();
				AbsRestApi.SendEnvelopedResult(_context, ref jsonWriter, HttpStatusCode.OK, null, null, null);
				return;
			}
			jsonWriter.WriteRaw(WebUtils.JsonEmptyData);
			AbsRestApi.SendEnvelopedResult(_context, ref jsonWriter, HttpStatusCode.BadRequest, null, null, null);
		}

		// Token: 0x0600CF10 RID: 53008 RVA: 0x004B63C8 File Offset: 0x004B45C8
		[PublicizedFrom(EAccessModifier.Private)]
		public void writeBan(ref JsonWriter _writer, AdminBlacklist.BannedUser _ban)
		{
			_writer.WriteRaw(Blacklist.jsonKeyName);
			_writer.WriteString(_ban.Name ?? "");
			_writer.WriteRaw(Blacklist.jsonKeyUserId);
			JsonCommons.WritePlatformUserIdentifier(ref _writer, _ban.UserIdentifier);
			_writer.WriteRaw(Blacklist.jsonKeyBannedUntil);
			JsonCommons.WriteDateTime(ref _writer, _ban.BannedUntil);
			_writer.WriteRaw(Blacklist.jsonKeyBanReason);
			_writer.WriteString(_ban.BanReason);
			_writer.WriteEndObject();
		}

		// Token: 0x0600CF11 RID: 53009 RVA: 0x004B6440 File Offset: 0x004B4640
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void HandleRestPost(RequestContext _context, IDictionary<string, object> _jsonInput, byte[] _jsonInputData)
		{
			PlatformUserIdentifierAbs platformUserIdentifierAbs;
			if (!this.TryParseId(_context, _jsonInputData, out platformUserIdentifierAbs))
			{
				return;
			}
			DateTime dateTime;
			if (!JsonCommons.TryReadDateTime(_jsonInput, "bannedUntil", out dateTime))
			{
				AbsRestApi.SendEmptyResponse(_context, HttpStatusCode.BadRequest, _jsonInputData, EApiErrorCode.NO_OR_INVALID_BANNED_UNTIL, null);
				return;
			}
			string text;
			JsonCommons.TryGetJsonField(_jsonInput, "banReason", out text);
			string text2;
			JsonCommons.TryGetJsonField(_jsonInput, "name", out text2);
			Blacklist.BlacklistInstance.AddBan(text2, platformUserIdentifierAbs, dateTime, text);
			AbsRestApi.SendEmptyResponse(_context, HttpStatusCode.Created, null, null, null);
		}

		// Token: 0x0600CF12 RID: 53010 RVA: 0x004B64B0 File Offset: 0x004B46B0
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void HandleRestDelete(RequestContext _context)
		{
			PlatformUserIdentifierAbs platformUserIdentifierAbs;
			if (!this.TryParseId(_context, null, out platformUserIdentifierAbs))
			{
				return;
			}
			AbsRestApi.SendEmptyResponse(_context, Blacklist.BlacklistInstance.RemoveBan(platformUserIdentifierAbs) ? HttpStatusCode.NoContent : HttpStatusCode.NotFound, null, null, null);
		}

		// Token: 0x0600CF13 RID: 53011 RVA: 0x004B64F0 File Offset: 0x004B46F0
		[PublicizedFrom(EAccessModifier.Private)]
		public bool TryParseId(RequestContext _context, byte[] _jsonInputData, out PlatformUserIdentifierAbs _userId)
		{
			string requestPath = _context.RequestPath;
			_userId = null;
			if (string.IsNullOrEmpty(requestPath))
			{
				AbsRestApi.SendEmptyResponse(_context, HttpStatusCode.BadRequest, _jsonInputData, EApiErrorCode.NO_USER, null);
				return false;
			}
			if (!PlatformUserIdentifierAbs.TryFromCombinedString(requestPath, out _userId))
			{
				AbsRestApi.SendEmptyResponse(_context, HttpStatusCode.BadRequest, _jsonInputData, EApiErrorCode.INVALID_USER, null);
				return false;
			}
			return true;
		}

		// Token: 0x1700197B RID: 6523
		// (get) Token: 0x0600CF14 RID: 53012 RVA: 0x00020021 File Offset: 0x0001E221
		public override bool AllowPostWithId
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return true;
			}
		}

		// Token: 0x0600CF15 RID: 53013 RVA: 0x004B6539 File Offset: 0x004B4739
		public override int[] DefaultMethodPermissionLevels()
		{
			return new int[] { -2147483647, int.MinValue, int.MinValue, -2147483647, int.MinValue };
		}

		// Token: 0x0600CF16 RID: 53014 RVA: 0x004AF5DC File Offset: 0x004AD7DC
		public Blacklist()
			: base(null)
		{
		}

		// Token: 0x04009D26 RID: 40230
		[PublicizedFrom(EAccessModifier.Private)]
		public const string propertyName = "name";

		// Token: 0x04009D27 RID: 40231
		[PublicizedFrom(EAccessModifier.Private)]
		public const string propertyUserId = "userId";

		// Token: 0x04009D28 RID: 40232
		[PublicizedFrom(EAccessModifier.Private)]
		public const string propertyBannedUntil = "bannedUntil";

		// Token: 0x04009D29 RID: 40233
		[PublicizedFrom(EAccessModifier.Private)]
		public const string propertyBanReason = "banReason";

		// Token: 0x04009D2A RID: 40234
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyName = JsonWriter.GetEncodedPropertyNameWithBeginObject("name");

		// Token: 0x04009D2B RID: 40235
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyUserId = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("userId");

		// Token: 0x04009D2C RID: 40236
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyBannedUntil = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("bannedUntil");

		// Token: 0x04009D2D RID: 40237
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyBanReason = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("banReason");
	}
}
