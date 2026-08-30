using System;
using System.Text.RegularExpressions;
using Epic.OnlineServices;
using UnityEngine;
using UnityEngine.Scripting;

namespace Platform.EOS
{
	// Token: 0x02001D02 RID: 7426
	[Preserve]
	[DoNotTouchSerializableFlags]
	[Serializable]
	public class UserIdentifierEos : PlatformUserIdentifierAbs
	{
		// Token: 0x17001B22 RID: 6946
		// (get) Token: 0x0600DB8E RID: 56206 RVA: 0x0002EEAC File Offset: 0x0002D0AC
		public override EPlatformIdentifier PlatformIdentifier
		{
			get
			{
				return EPlatformIdentifier.EOS;
			}
		}

		// Token: 0x17001B23 RID: 6947
		// (get) Token: 0x0600DB8F RID: 56207 RVA: 0x004EB3B9 File Offset: 0x004E95B9
		public override string PlatformIdentifierString { get; } = PlatformManager.PlatformStringFromEnum(EPlatformIdentifier.EOS);

		// Token: 0x17001B24 RID: 6948
		// (get) Token: 0x0600DB90 RID: 56208 RVA: 0x004EB3C1 File Offset: 0x004E95C1
		public override string ReadablePlatformUserIdentifier
		{
			get
			{
				return this.ProductUserIdString;
			}
		}

		// Token: 0x17001B25 RID: 6949
		// (get) Token: 0x0600DB91 RID: 56209 RVA: 0x004EB3C9 File Offset: 0x004E95C9
		public override string CombinedString { get; }

		// Token: 0x0600DB92 RID: 56210 RVA: 0x004EB3D1 File Offset: 0x004E95D1
		public static string CreateCombinedString(string _puidString)
		{
			return PlatformManager.PlatformStringFromEnum(EPlatformIdentifier.EOS) + "_" + _puidString;
		}

		// Token: 0x0600DB93 RID: 56211 RVA: 0x004EB3E4 File Offset: 0x004E95E4
		public static string CreateCombinedString(ProductUserId _puid)
		{
			return PlatformManager.PlatformStringFromEnum(EPlatformIdentifier.EOS) + "_" + UserIdentifierEos.CreateStringFromPuid(_puid);
		}

		// Token: 0x17001B26 RID: 6950
		// (get) Token: 0x0600DB94 RID: 56212 RVA: 0x004EB3FC File Offset: 0x004E95FC
		public string ProductUserIdString
		{
			get
			{
				string text;
				if ((text = this.productUserIdString) == null)
				{
					text = (this.productUserIdString = UserIdentifierEos.CreateStringFromPuid(this.productUserId));
				}
				return text;
			}
		}

		// Token: 0x17001B27 RID: 6951
		// (get) Token: 0x0600DB95 RID: 56213 RVA: 0x004EB428 File Offset: 0x004E9628
		public ProductUserId ProductUserId
		{
			get
			{
				ProductUserId productUserId;
				if ((productUserId = this.productUserId) == null)
				{
					productUserId = (this.productUserId = UserIdentifierEos.CreatePuidFromString(this.productUserIdString));
				}
				return productUserId;
			}
		}

		// Token: 0x17001B28 RID: 6952
		// (get) Token: 0x0600DB96 RID: 56214 RVA: 0x004EB453 File Offset: 0x004E9653
		// (set) Token: 0x0600DB97 RID: 56215 RVA: 0x004EB45B File Offset: 0x004E965B
		public string Ticket
		{
			get
			{
				return this.ticket;
			}
			[PublicizedFrom(EAccessModifier.Private)]
			set
			{
				this.ticket = value;
			}
		}

		// Token: 0x0600DB98 RID: 56216 RVA: 0x004EB464 File Offset: 0x004E9664
		public UserIdentifierEos(string _puid)
		{
			if (string.IsNullOrEmpty(_puid))
			{
				throw new ArgumentException("Empty or null PUID", "_puid");
			}
			if (!UserIdentifierEos.puidMatcher.IsMatch(_puid))
			{
				throw new ArgumentException("Invalid PUID '" + _puid + "'", "_puid");
			}
			this.productUserIdString = _puid;
			this.CombinedString = UserIdentifierEos.CreateCombinedString(_puid);
			this.hashcode = this.ProductUserIdString.GetHashCode() ^ ((int)this.PlatformIdentifier * 397);
		}

		// Token: 0x0600DB99 RID: 56217 RVA: 0x004EB4F4 File Offset: 0x004E96F4
		public UserIdentifierEos(ProductUserId _puid)
		{
			if (_puid == null)
			{
				throw new ArgumentException("Null PUID", "_puid");
			}
			this.productUserId = _puid;
			this.CombinedString = UserIdentifierEos.CreateCombinedString(this.ProductUserIdString);
			this.hashcode = this.ProductUserIdString.GetHashCode() ^ ((int)this.PlatformIdentifier * 397);
		}

		// Token: 0x0600DB9A RID: 56218 RVA: 0x004EB564 File Offset: 0x004E9764
		public static string CreateStringFromPuid(ProductUserId _puid)
		{
			if (!ThreadManager.IsMainThread())
			{
				Log.Warning("CreateStringFromPuid NOT ON MAIN THREAD! From:\n" + StackTraceUtility.ExtractStackTrace() + "\n");
			}
			if (_puid == null)
			{
				Log.Error("CreateStringFromPuid with null PUID! From:\n" + StackTraceUtility.ExtractStackTrace() + "\n");
				return null;
			}
			return _puid.ToString();
		}

		// Token: 0x0600DB9B RID: 56219 RVA: 0x004EB5BB File Offset: 0x004E97BB
		public static ProductUserId CreatePuidFromString(string _puidString)
		{
			if (_puidString == null)
			{
				Log.Error("CreatePuidFromString with null PUID string! From:\n" + StackTraceUtility.ExtractStackTrace() + "\n");
				return null;
			}
			return ProductUserId.FromString(_puidString);
		}

		// Token: 0x0600DB9C RID: 56220 RVA: 0x004EB5E6 File Offset: 0x004E97E6
		public override bool DecodeTicket(string _ticket)
		{
			if (string.IsNullOrEmpty(_ticket))
			{
				return false;
			}
			this.Ticket = _ticket;
			return true;
		}

		// Token: 0x0600DB9D RID: 56221 RVA: 0x004EB5FC File Offset: 0x004E97FC
		public override bool Equals(PlatformUserIdentifierAbs _other)
		{
			if (_other == null)
			{
				return false;
			}
			if (this == _other)
			{
				return true;
			}
			UserIdentifierEos userIdentifierEos = _other as UserIdentifierEos;
			return userIdentifierEos != null && string.Equals(userIdentifierEos.ProductUserIdString, this.ProductUserIdString, StringComparison.Ordinal);
		}

		// Token: 0x0600DB9E RID: 56222 RVA: 0x004EB632 File Offset: 0x004E9832
		public override int GetHashCode()
		{
			return this.hashcode;
		}

		// Token: 0x0400A5E6 RID: 42470
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly Regex puidMatcher = new Regex("^[0-9a-fA-F]{8,32}$", RegexOptions.Compiled);

		// Token: 0x0400A5E9 RID: 42473
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public string ticket;

		// Token: 0x0400A5EA RID: 42474
		[PublicizedFrom(EAccessModifier.Private)]
		public string productUserIdString;

		// Token: 0x0400A5EB RID: 42475
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public ProductUserId productUserId;

		// Token: 0x0400A5EC RID: 42476
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly int hashcode;
	}
}
