using System;
using System.Text.RegularExpressions;
using Epic.OnlineServices;
using UnityEngine;
using UnityEngine.Scripting;

namespace Platform.EOS
{
	// Token: 0x02001D5C RID: 7516
	[Preserve]
	[DoNotTouchSerializableFlags]
	[Serializable]
	public class UserIdentifierEos : PlatformUserIdentifierAbs
	{
		// Token: 0x17001C3A RID: 7226
		// (get) Token: 0x0600DEAA RID: 57002 RVA: 0x0002F260 File Offset: 0x0002D460
		public override EPlatformIdentifier PlatformIdentifier
		{
			get
			{
				return EPlatformIdentifier.EOS;
			}
		}

		// Token: 0x17001C3B RID: 7227
		// (get) Token: 0x0600DEAB RID: 57003 RVA: 0x004F6A75 File Offset: 0x004F4C75
		public override string PlatformIdentifierString { get; } = PlatformManager.PlatformStringFromEnum(EPlatformIdentifier.EOS);

		// Token: 0x17001C3C RID: 7228
		// (get) Token: 0x0600DEAC RID: 57004 RVA: 0x004F6A7D File Offset: 0x004F4C7D
		public override string ReadablePlatformUserIdentifier
		{
			get
			{
				return this.ProductUserIdString;
			}
		}

		// Token: 0x17001C3D RID: 7229
		// (get) Token: 0x0600DEAD RID: 57005 RVA: 0x004F6A85 File Offset: 0x004F4C85
		public override string CombinedString { get; }

		// Token: 0x0600DEAE RID: 57006 RVA: 0x004F6A8D File Offset: 0x004F4C8D
		public static string CreateCombinedString(string _puidString)
		{
			return PlatformManager.PlatformStringFromEnum(EPlatformIdentifier.EOS) + "_" + _puidString;
		}

		// Token: 0x0600DEAF RID: 57007 RVA: 0x004F6AA0 File Offset: 0x004F4CA0
		public static string CreateCombinedString(ProductUserId _puid)
		{
			return PlatformManager.PlatformStringFromEnum(EPlatformIdentifier.EOS) + "_" + UserIdentifierEos.CreateStringFromPuid(_puid);
		}

		// Token: 0x17001C3E RID: 7230
		// (get) Token: 0x0600DEB0 RID: 57008 RVA: 0x004F6AB8 File Offset: 0x004F4CB8
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

		// Token: 0x17001C3F RID: 7231
		// (get) Token: 0x0600DEB1 RID: 57009 RVA: 0x004F6AE4 File Offset: 0x004F4CE4
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

		// Token: 0x17001C40 RID: 7232
		// (get) Token: 0x0600DEB2 RID: 57010 RVA: 0x004F6B0F File Offset: 0x004F4D0F
		// (set) Token: 0x0600DEB3 RID: 57011 RVA: 0x004F6B17 File Offset: 0x004F4D17
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

		// Token: 0x0600DEB4 RID: 57012 RVA: 0x004F6B20 File Offset: 0x004F4D20
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

		// Token: 0x0600DEB5 RID: 57013 RVA: 0x004F6BB0 File Offset: 0x004F4DB0
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

		// Token: 0x0600DEB6 RID: 57014 RVA: 0x004F6C20 File Offset: 0x004F4E20
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

		// Token: 0x0600DEB7 RID: 57015 RVA: 0x004F6C77 File Offset: 0x004F4E77
		public static ProductUserId CreatePuidFromString(string _puidString)
		{
			if (_puidString == null)
			{
				Log.Error("CreatePuidFromString with null PUID string! From:\n" + StackTraceUtility.ExtractStackTrace() + "\n");
				return null;
			}
			return ProductUserId.FromString(_puidString);
		}

		// Token: 0x0600DEB8 RID: 57016 RVA: 0x004F6CA2 File Offset: 0x004F4EA2
		public override bool DecodeTicket(string _ticket)
		{
			if (string.IsNullOrEmpty(_ticket))
			{
				return false;
			}
			this.Ticket = _ticket;
			return true;
		}

		// Token: 0x0600DEB9 RID: 57017 RVA: 0x004F6CB8 File Offset: 0x004F4EB8
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

		// Token: 0x0600DEBA RID: 57018 RVA: 0x004F6CEE File Offset: 0x004F4EEE
		public override int GetHashCode()
		{
			return this.hashcode;
		}

		// Token: 0x0400A7F2 RID: 42994
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly Regex puidMatcher = new Regex("^[0-9a-fA-F]{8,32}$", RegexOptions.Compiled);

		// Token: 0x0400A7F5 RID: 42997
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public string ticket;

		// Token: 0x0400A7F6 RID: 42998
		[PublicizedFrom(EAccessModifier.Private)]
		public string productUserIdString;

		// Token: 0x0400A7F7 RID: 42999
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public ProductUserId productUserId;

		// Token: 0x0400A7F8 RID: 43000
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly int hashcode;
	}
}
