using System;
using System.Text.RegularExpressions;
using Epic.OnlineServices;
using UnityEngine;
using UnityEngine.Scripting;

namespace Platform.EOS
{
	// Token: 0x02001D8D RID: 7565
	[Preserve]
	[DoNotTouchSerializableFlags]
	[Serializable]
	public class UserIdentifierEos : PlatformUserIdentifierAbs
	{
		// Token: 0x17001C82 RID: 7298
		// (get) Token: 0x0600DFBE RID: 57278 RVA: 0x0002F260 File Offset: 0x0002D460
		public override EPlatformIdentifier PlatformIdentifier
		{
			get
			{
				return EPlatformIdentifier.EOS;
			}
		}

		// Token: 0x17001C83 RID: 7299
		// (get) Token: 0x0600DFBF RID: 57279 RVA: 0x004FA615 File Offset: 0x004F8815
		public override string PlatformIdentifierString { get; } = PlatformManager.PlatformStringFromEnum(EPlatformIdentifier.EOS);

		// Token: 0x17001C84 RID: 7300
		// (get) Token: 0x0600DFC0 RID: 57280 RVA: 0x004FA61D File Offset: 0x004F881D
		public override string ReadablePlatformUserIdentifier
		{
			get
			{
				return this.ProductUserIdString;
			}
		}

		// Token: 0x17001C85 RID: 7301
		// (get) Token: 0x0600DFC1 RID: 57281 RVA: 0x004FA625 File Offset: 0x004F8825
		public override string CombinedString { get; }

		// Token: 0x0600DFC2 RID: 57282 RVA: 0x004FA62D File Offset: 0x004F882D
		public static string CreateCombinedString(string _puidString)
		{
			return PlatformManager.PlatformStringFromEnum(EPlatformIdentifier.EOS) + "_" + _puidString;
		}

		// Token: 0x0600DFC3 RID: 57283 RVA: 0x004FA640 File Offset: 0x004F8840
		public static string CreateCombinedString(ProductUserId _puid)
		{
			return PlatformManager.PlatformStringFromEnum(EPlatformIdentifier.EOS) + "_" + UserIdentifierEos.CreateStringFromPuid(_puid);
		}

		// Token: 0x17001C86 RID: 7302
		// (get) Token: 0x0600DFC4 RID: 57284 RVA: 0x004FA658 File Offset: 0x004F8858
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

		// Token: 0x17001C87 RID: 7303
		// (get) Token: 0x0600DFC5 RID: 57285 RVA: 0x004FA684 File Offset: 0x004F8884
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

		// Token: 0x17001C88 RID: 7304
		// (get) Token: 0x0600DFC6 RID: 57286 RVA: 0x004FA6AF File Offset: 0x004F88AF
		// (set) Token: 0x0600DFC7 RID: 57287 RVA: 0x004FA6B7 File Offset: 0x004F88B7
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

		// Token: 0x0600DFC8 RID: 57288 RVA: 0x004FA6C0 File Offset: 0x004F88C0
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

		// Token: 0x0600DFC9 RID: 57289 RVA: 0x004FA750 File Offset: 0x004F8950
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

		// Token: 0x0600DFCA RID: 57290 RVA: 0x004FA7C0 File Offset: 0x004F89C0
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

		// Token: 0x0600DFCB RID: 57291 RVA: 0x004FA817 File Offset: 0x004F8A17
		public static ProductUserId CreatePuidFromString(string _puidString)
		{
			if (_puidString == null)
			{
				Log.Error("CreatePuidFromString with null PUID string! From:\n" + StackTraceUtility.ExtractStackTrace() + "\n");
				return null;
			}
			return ProductUserId.FromString(_puidString);
		}

		// Token: 0x0600DFCC RID: 57292 RVA: 0x004FA842 File Offset: 0x004F8A42
		public override bool DecodeTicket(string _ticket)
		{
			if (string.IsNullOrEmpty(_ticket))
			{
				return false;
			}
			this.Ticket = _ticket;
			return true;
		}

		// Token: 0x0600DFCD RID: 57293 RVA: 0x004FA858 File Offset: 0x004F8A58
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

		// Token: 0x0600DFCE RID: 57294 RVA: 0x004FA88E File Offset: 0x004F8A8E
		public override int GetHashCode()
		{
			return this.hashcode;
		}

		// Token: 0x0400A8DB RID: 43227
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly Regex puidMatcher = new Regex("^[0-9a-fA-F]{8,32}$", RegexOptions.Compiled);

		// Token: 0x0400A8DE RID: 43230
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public string ticket;

		// Token: 0x0400A8DF RID: 43231
		[PublicizedFrom(EAccessModifier.Private)]
		public string productUserIdString;

		// Token: 0x0400A8E0 RID: 43232
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public ProductUserId productUserId;

		// Token: 0x0400A8E1 RID: 43233
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly int hashcode;
	}
}
