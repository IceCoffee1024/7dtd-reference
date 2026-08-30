using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine.Scripting;
using Utf8Json;

namespace Webserver.WebAPI.APIs.Permissions
{
	// Token: 0x02001ABA RID: 6842
	[Preserve]
	public class Blacklist : AbsRestApi
	{
		// Token: 0x17001908 RID: 6408
		// (get) Token: 0x0600CD85 RID: 52613 RVA: 0x004AFF08 File Offset: 0x004AE108
		public static AdminBlacklist BlacklistInstance
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return GameManager.Instance.adminTools.Blacklist;
			}
		}

		// Token: 0x0600CD86 RID: 52614 RVA: 0x004AFF1C File Offset: 0x004AE11C
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

		// Token: 0x0600CD87 RID: 52615 RVA: 0x004AFFD4 File Offset: 0x004AE1D4
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

		// Token: 0x0600CD88 RID: 52616 RVA: 0x004B004C File Offset: 0x004AE24C
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

		// Token: 0x0600CD89 RID: 52617 RVA: 0x004B00BC File Offset: 0x004AE2BC
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

		// Token: 0x0600CD8A RID: 52618 RVA: 0x004B00FC File Offset: 0x004AE2FC
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

		// Token: 0x17001909 RID: 6409
		// (get) Token: 0x0600CD8B RID: 52619 RVA: 0x0001FF5D File Offset: 0x0001E15D
		public override bool AllowPostWithId
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return true;
			}
		}

		// Token: 0x0600CD8C RID: 52620 RVA: 0x004B0145 File Offset: 0x004AE345
		public override int[] DefaultMethodPermissionLevels()
		{
			return new int[] { -2147483647, int.MinValue, int.MinValue, -2147483647, int.MinValue };
		}

		// Token: 0x0600CD8D RID: 52621 RVA: 0x004A91E8 File Offset: 0x004A73E8
		public Blacklist()
			: base(null)
		{
		}

		// Token: 0x04009BF5 RID: 39925
		[PublicizedFrom(EAccessModifier.Private)]
		public const string propertyName = "name";

		// Token: 0x04009BF6 RID: 39926
		[PublicizedFrom(EAccessModifier.Private)]
		public const string propertyUserId = "userId";

		// Token: 0x04009BF7 RID: 39927
		[PublicizedFrom(EAccessModifier.Private)]
		public const string propertyBannedUntil = "bannedUntil";

		// Token: 0x04009BF8 RID: 39928
		[PublicizedFrom(EAccessModifier.Private)]
		public const string propertyBanReason = "banReason";

		// Token: 0x04009BF9 RID: 39929
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyName = JsonWriter.GetEncodedPropertyNameWithBeginObject("name");

		// Token: 0x04009BFA RID: 39930
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyUserId = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("userId");

		// Token: 0x04009BFB RID: 39931
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyBannedUntil = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("bannedUntil");

		// Token: 0x04009BFC RID: 39932
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyBanReason = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("banReason");
	}
}
