using System;
using Epic.OnlineServices;
using Epic.OnlineServices.Connect;

namespace Platform.EOS
{
	// Token: 0x02001D04 RID: 7428
	public class UserServer : UserBase
	{
		// Token: 0x17001B29 RID: 6953
		// (get) Token: 0x0600DBA2 RID: 56226 RVA: 0x001AFF0C File Offset: 0x001AE10C
		public override EUserPerms Permissions
		{
			get
			{
				return EUserPerms.All;
			}
		}

		// Token: 0x0600DBA3 RID: 56227 RVA: 0x004EB654 File Offset: 0x004E9854
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

		// Token: 0x0600DBA4 RID: 56228 RVA: 0x004EB6AF File Offset: 0x004E98AF
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void startLogin(LoginUserCallback _delegate, bool _refreshing = false)
		{
			this.fetchDeviceId(_delegate, _refreshing);
		}

		// Token: 0x0600DBA5 RID: 56229 RVA: 0x004EB6BC File Offset: 0x004E98BC
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

		// Token: 0x0600DBA6 RID: 56230 RVA: 0x000876FD File Offset: 0x000858FD
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void connectCreateUser(ContinuanceToken _continuanceToken, LoginUserCallback _callback)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600DBA7 RID: 56231 RVA: 0x004EB741 File Offset: 0x004E9941
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void eosLoggedIn(ProductUserId _puid, LoginUserCallback _callback)
		{
			base.eosLoggedIn(_puid, _callback);
			this.eosLoginDone(_callback);
		}
	}
}
