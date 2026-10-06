using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine.Scripting;
using Utf8Json;

namespace Webserver.WebAPI.APIs.Permissions
{
	// Token: 0x02001B0D RID: 6925
	[Preserve]
	public class Blacklist : AbsRestApi
	{
		// Token: 0x17001A1C RID: 6684
		// (get) Token: 0x0600D066 RID: 53350 RVA: 0x004BA7C8 File Offset: 0x004B89C8
		public static AdminBlacklist BlacklistInstance
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return GameManager.Instance.adminTools.Blacklist;
			}
		}

		// Token: 0x0600D067 RID: 53351 RVA: 0x004BA7DC File Offset: 0x004B89DC
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

		// Token: 0x0600D068 RID: 53352 RVA: 0x004BA894 File Offset: 0x004B8A94
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

		// Token: 0x0600D069 RID: 53353 RVA: 0x004BA90C File Offset: 0x004B8B0C
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

		// Token: 0x0600D06A RID: 53354 RVA: 0x004BA97C File Offset: 0x004B8B7C
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

		// Token: 0x0600D06B RID: 53355 RVA: 0x004BA9BC File Offset: 0x004B8BBC
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

		// Token: 0x17001A1D RID: 6685
		// (get) Token: 0x0600D06C RID: 53356 RVA: 0x00020119 File Offset: 0x0001E319
		public override bool AllowPostWithId
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return true;
			}
		}

		// Token: 0x0600D06D RID: 53357 RVA: 0x004BAA05 File Offset: 0x004B8C05
		public override int[] DefaultMethodPermissionLevels()
		{
			return new int[] { -2147483647, int.MinValue, int.MinValue, -2147483647, int.MinValue };
		}

		// Token: 0x0600D06E RID: 53358 RVA: 0x004B3AA8 File Offset: 0x004B1CA8
		public Blacklist()
			: base(null)
		{
		}

		// Token: 0x04009DEC RID: 40428
		[PublicizedFrom(EAccessModifier.Private)]
		public const string propertyName = "name";

		// Token: 0x04009DED RID: 40429
		[PublicizedFrom(EAccessModifier.Private)]
		public const string propertyUserId = "userId";

		// Token: 0x04009DEE RID: 40430
		[PublicizedFrom(EAccessModifier.Private)]
		public const string propertyBannedUntil = "bannedUntil";

		// Token: 0x04009DEF RID: 40431
		[PublicizedFrom(EAccessModifier.Private)]
		public const string propertyBanReason = "banReason";

		// Token: 0x04009DF0 RID: 40432
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyName = JsonWriter.GetEncodedPropertyNameWithBeginObject("name");

		// Token: 0x04009DF1 RID: 40433
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyUserId = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("userId");

		// Token: 0x04009DF2 RID: 40434
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyBannedUntil = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("bannedUntil");

		// Token: 0x04009DF3 RID: 40435
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyBanReason = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("banReason");
	}
}
