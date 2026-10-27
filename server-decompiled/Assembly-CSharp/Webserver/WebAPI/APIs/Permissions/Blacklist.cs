using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine.Scripting;
using Utf8Json;

namespace Webserver.WebAPI.APIs.Permissions
{
	// Token: 0x02001B3E RID: 6974
	[Preserve]
	public class Blacklist : AbsRestApi
	{
		// Token: 0x17001A64 RID: 6756
		// (get) Token: 0x0600D17A RID: 53626 RVA: 0x004BE2C4 File Offset: 0x004BC4C4
		public static AdminBlacklist BlacklistInstance
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return GameManager.Instance.adminTools.Blacklist;
			}
		}

		// Token: 0x0600D17B RID: 53627 RVA: 0x004BE2D8 File Offset: 0x004BC4D8
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

		// Token: 0x0600D17C RID: 53628 RVA: 0x004BE390 File Offset: 0x004BC590
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

		// Token: 0x0600D17D RID: 53629 RVA: 0x004BE408 File Offset: 0x004BC608
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

		// Token: 0x0600D17E RID: 53630 RVA: 0x004BE478 File Offset: 0x004BC678
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

		// Token: 0x0600D17F RID: 53631 RVA: 0x004BE4B8 File Offset: 0x004BC6B8
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

		// Token: 0x17001A65 RID: 6757
		// (get) Token: 0x0600D180 RID: 53632 RVA: 0x00020119 File Offset: 0x0001E319
		public override bool AllowPostWithId
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return true;
			}
		}

		// Token: 0x0600D181 RID: 53633 RVA: 0x004BE501 File Offset: 0x004BC701
		public override int[] DefaultMethodPermissionLevels()
		{
			return new int[] { -2147483647, int.MinValue, int.MinValue, -2147483647, int.MinValue };
		}

		// Token: 0x0600D182 RID: 53634 RVA: 0x004B75A4 File Offset: 0x004B57A4
		public Blacklist()
			: base(null)
		{
		}

		// Token: 0x04009ED5 RID: 40661
		[PublicizedFrom(EAccessModifier.Private)]
		public const string propertyName = "name";

		// Token: 0x04009ED6 RID: 40662
		[PublicizedFrom(EAccessModifier.Private)]
		public const string propertyUserId = "userId";

		// Token: 0x04009ED7 RID: 40663
		[PublicizedFrom(EAccessModifier.Private)]
		public const string propertyBannedUntil = "bannedUntil";

		// Token: 0x04009ED8 RID: 40664
		[PublicizedFrom(EAccessModifier.Private)]
		public const string propertyBanReason = "banReason";

		// Token: 0x04009ED9 RID: 40665
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyName = JsonWriter.GetEncodedPropertyNameWithBeginObject("name");

		// Token: 0x04009EDA RID: 40666
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyUserId = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("userId");

		// Token: 0x04009EDB RID: 40667
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyBannedUntil = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("bannedUntil");

		// Token: 0x04009EDC RID: 40668
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyBanReason = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("banReason");
	}
}
