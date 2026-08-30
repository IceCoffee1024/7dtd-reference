using System;
using System.Text.RegularExpressions;
using Epic.OnlineServices;
using UnityEngine;
using UnityEngine.Scripting;

namespace Platform.EOS
{
	// Token: 0x02001D2A RID: 7466
	[Preserve]
	[DoNotTouchSerializableFlags]
	[Serializable]
	public class UserIdentifierEos : PlatformUserIdentifierAbs
	{
		// Token: 0x17001B94 RID: 7060
		// (get) Token: 0x0600DD2F RID: 56623 RVA: 0x0002F168 File Offset: 0x0002D368
		public override EPlatformIdentifier PlatformIdentifier
		{
			get
			{
				return EPlatformIdentifier.EOS;
			}
		}

		// Token: 0x17001B95 RID: 7061
		// (get) Token: 0x0600DD30 RID: 56624 RVA: 0x004F1E2D File Offset: 0x004F002D
		public override string PlatformIdentifierString { get; } = PlatformManager.PlatformStringFromEnum(EPlatformIdentifier.EOS);

		// Token: 0x17001B96 RID: 7062
		// (get) Token: 0x0600DD31 RID: 56625 RVA: 0x004F1E35 File Offset: 0x004F0035
		public override string ReadablePlatformUserIdentifier
		{
			get
			{
				return this.ProductUserIdString;
			}
		}

		// Token: 0x17001B97 RID: 7063
		// (get) Token: 0x0600DD32 RID: 56626 RVA: 0x004F1E3D File Offset: 0x004F003D
		public override string CombinedString { get; }

		// Token: 0x0600DD33 RID: 56627 RVA: 0x004F1E45 File Offset: 0x004F0045
		public static string CreateCombinedString(string _puidString)
		{
			return PlatformManager.PlatformStringFromEnum(EPlatformIdentifier.EOS) + "_" + _puidString;
		}

		// Token: 0x0600DD34 RID: 56628 RVA: 0x004F1E58 File Offset: 0x004F0058
		public static string CreateCombinedString(ProductUserId _puid)
		{
			return PlatformManager.PlatformStringFromEnum(EPlatformIdentifier.EOS) + "_" + UserIdentifierEos.CreateStringFromPuid(_puid);
		}

		// Token: 0x17001B98 RID: 7064
		// (get) Token: 0x0600DD35 RID: 56629 RVA: 0x004F1E70 File Offset: 0x004F0070
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

		// Token: 0x17001B99 RID: 7065
		// (get) Token: 0x0600DD36 RID: 56630 RVA: 0x004F1E9C File Offset: 0x004F009C
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

		// Token: 0x17001B9A RID: 7066
		// (get) Token: 0x0600DD37 RID: 56631 RVA: 0x004F1EC7 File Offset: 0x004F00C7
		// (set) Token: 0x0600DD38 RID: 56632 RVA: 0x004F1ECF File Offset: 0x004F00CF
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

		// Token: 0x0600DD39 RID: 56633 RVA: 0x004F1ED8 File Offset: 0x004F00D8
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

		// Token: 0x0600DD3A RID: 56634 RVA: 0x004F1F68 File Offset: 0x004F0168
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

		// Token: 0x0600DD3B RID: 56635 RVA: 0x004F1FD8 File Offset: 0x004F01D8
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

		// Token: 0x0600DD3C RID: 56636 RVA: 0x004F202F File Offset: 0x004F022F
		public static ProductUserId CreatePuidFromString(string _puidString)
		{
			if (_puidString == null)
			{
				Log.Error("CreatePuidFromString with null PUID string! From:\n" + StackTraceUtility.ExtractStackTrace() + "\n");
				return null;
			}
			return ProductUserId.FromString(_puidString);
		}

		// Token: 0x0600DD3D RID: 56637 RVA: 0x004F205A File Offset: 0x004F025A
		public override bool DecodeTicket(string _ticket)
		{
			if (string.IsNullOrEmpty(_ticket))
			{
				return false;
			}
			this.Ticket = _ticket;
			return true;
		}

		// Token: 0x0600DD3E RID: 56638 RVA: 0x004F2070 File Offset: 0x004F0270
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

		// Token: 0x0600DD3F RID: 56639 RVA: 0x004F20A6 File Offset: 0x004F02A6
		public override int GetHashCode()
		{
			return this.hashcode;
		}

		// Token: 0x0400A71B RID: 42779
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly Regex puidMatcher = new Regex("^[0-9a-fA-F]{8,32}$", RegexOptions.Compiled);

		// Token: 0x0400A71E RID: 42782
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public string ticket;

		// Token: 0x0400A71F RID: 42783
		[PublicizedFrom(EAccessModifier.Private)]
		public string productUserIdString;

		// Token: 0x0400A720 RID: 42784
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public ProductUserId productUserId;

		// Token: 0x0400A721 RID: 42785
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly int hashcode;
	}
}
