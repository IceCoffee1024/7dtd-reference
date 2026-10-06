using System;
using Epic.OnlineServices;
using Epic.OnlineServices.Connect;

namespace Platform.EOS
{
	// Token: 0x02001D5E RID: 7518
	public class UserServer : UserBase
	{
		// Token: 0x17001C41 RID: 7233
		// (get) Token: 0x0600DEBE RID: 57022 RVA: 0x001B2938 File Offset: 0x001B0B38
		public override EUserPerms Permissions
		{
			get
			{
				return EUserPerms.All;
			}
		}

		// Token: 0x0600DEBF RID: 57023 RVA: 0x004F6D10 File Offset: 0x004F4F10
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

		// Token: 0x0600DEC0 RID: 57024 RVA: 0x004F6D6B File Offset: 0x004F4F6B
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void startLogin(LoginUserCallback _delegate, bool _refreshing = false)
		{
			this.fetchDeviceId(_delegate, _refreshing);
		}

		// Token: 0x0600DEC1 RID: 57025 RVA: 0x004F6D78 File Offset: 0x004F4F78
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

		// Token: 0x0600DEC2 RID: 57026 RVA: 0x0008806C File Offset: 0x0008626C
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void connectCreateUser(ContinuanceToken _continuanceToken, LoginUserCallback _callback)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600DEC3 RID: 57027 RVA: 0x004F6DFD File Offset: 0x004F4FFD
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void eosLoggedIn(ProductUserId _puid, LoginUserCallback _callback)
		{
			base.eosLoggedIn(_puid, _callback);
			this.eosLoginDone(_callback);
		}
	}
}
