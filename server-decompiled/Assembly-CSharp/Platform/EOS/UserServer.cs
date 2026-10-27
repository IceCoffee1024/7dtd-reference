using System;
using Epic.OnlineServices;
using Epic.OnlineServices.Connect;

namespace Platform.EOS
{
	// Token: 0x02001D8F RID: 7567
	public class UserServer : UserBase
	{
		// Token: 0x17001C89 RID: 7305
		// (get) Token: 0x0600DFD2 RID: 57298 RVA: 0x001B39BC File Offset: 0x001B1BBC
		public override EUserPerms Permissions
		{
			get
			{
				return EUserPerms.All;
			}
		}

		// Token: 0x0600DFD3 RID: 57299 RVA: 0x004FA8B0 File Offset: 0x004F8AB0
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

		// Token: 0x0600DFD4 RID: 57300 RVA: 0x004FA90B File Offset: 0x004F8B0B
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void startLogin(LoginUserCallback _delegate, bool _refreshing = false)
		{
			this.fetchDeviceId(_delegate, _refreshing);
		}

		// Token: 0x0600DFD5 RID: 57301 RVA: 0x004FA918 File Offset: 0x004F8B18
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

		// Token: 0x0600DFD6 RID: 57302 RVA: 0x000882A0 File Offset: 0x000864A0
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void connectCreateUser(ContinuanceToken _continuanceToken, LoginUserCallback _callback)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600DFD7 RID: 57303 RVA: 0x004FA99D File Offset: 0x004F8B9D
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void eosLoggedIn(ProductUserId _puid, LoginUserCallback _callback)
		{
			base.eosLoggedIn(_puid, _callback);
			this.eosLoginDone(_callback);
		}
	}
}
