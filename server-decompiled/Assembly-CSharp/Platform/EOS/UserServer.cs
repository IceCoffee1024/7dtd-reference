using System;
using Epic.OnlineServices;
using Epic.OnlineServices.Connect;

namespace Platform.EOS
{
	// Token: 0x02001D2C RID: 7468
	public class UserServer : UserBase
	{
		// Token: 0x17001B9B RID: 7067
		// (get) Token: 0x0600DD43 RID: 56643 RVA: 0x001B24E4 File Offset: 0x001B06E4
		public override EUserPerms Permissions
		{
			get
			{
				return EUserPerms.All;
			}
		}

		// Token: 0x0600DD44 RID: 56644 RVA: 0x004F20C8 File Offset: 0x004F02C8
		public override void Login(LoginUserCallback _delegate)
		{
			if (base.UserStatus == EUserStatus.LoggedIn)
			{
				Log.Out("[EOS] Login already done.");
				this.eosLoginDone(_delegate);
				return;
			}
			Log.Out("[EOS] Login");
			EosHelpers.TestEosConnection(delegate(bool _success)
			{
				if (_success)
				{
					this.startLogin(_delegate, false);
					return;
				}
				this.UserStatus = EUserStatus.OfflineMode;
				_delegate(this.Owner, EApiStatusReason.Other, "No connection to EOS backend");
			});
		}

		// Token: 0x0600DD45 RID: 56645 RVA: 0x004F2123 File Offset: 0x004F0323
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void startLogin(LoginUserCallback _delegate, bool _refreshing = false)
		{
			this.fetchDeviceId(_delegate, _refreshing);
		}

		// Token: 0x0600DD46 RID: 56646 RVA: 0x004F2130 File Offset: 0x004F0330
		[PublicizedFrom(EAccessModifier.Private)]
		public void fetchDeviceId(LoginUserCallback _delegate, bool _refreshing)
		{
			Log.Out(string.Format("[EOS] Fetching DeviceId from native platform, with delegate={0}, refreshing={1}", _delegate != null, _refreshing));
			CreateDeviceIdOptions createDeviceIdOptions = new CreateDeviceIdOptions
			{
				DeviceModel = "DedicatedServer"
			};
			base.ConnectInterface.CreateDeviceId(ref createDeviceIdOptions, null, delegate(ref CreateDeviceIdCallbackInfo _result)
			{
				if (_result.ResultCode == Result.Success || _result.ResultCode == Result.DuplicateNotAllowed)
				{
					this.connectLogin(null, null, ExternalCredentialType.DeviceidAccessToken, _delegate, _refreshing, "DedicatedServer");
					return;
				}
				Log.Error("[EOS] Failed creating DeviceId: " + _result.ResultCode.ToStringCached<Result>());
				this.UserStatus = EUserStatus.TemporaryError;
				LoginUserCallback @delegate = _delegate;
				if (@delegate == null)
				{
					return;
				}
				@delegate(this.Owner, EApiStatusReason.NoLoginTicket, null);
			});
		}

		// Token: 0x0600DD47 RID: 56647 RVA: 0x00087DB8 File Offset: 0x00085FB8
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void connectCreateUser(ContinuanceToken _continuanceToken, LoginUserCallback _callback)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600DD48 RID: 56648 RVA: 0x004F21B5 File Offset: 0x004F03B5
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void eosLoggedIn(ProductUserId _puid, LoginUserCallback _callback)
		{
			base.eosLoggedIn(_puid, _callback);
			this.eosLoginDone(_callback);
		}
	}
}
