using System;
using System.Collections.Generic;

namespace Platform.EOS
{
	// Token: 0x02001D00 RID: 7424
	public class UserDetailsServiceEos : IUserDetailsService
	{
		// Token: 0x0600DB89 RID: 56201 RVA: 0x004EB1FD File Offset: 0x004E93FD
		public void Init(IPlatform owner)
		{
			this.idMapper = (EosUserIdMapper)owner.IdMappingService;
			this.user = (User)owner.User;
		}

		// Token: 0x0600DB8A RID: 56202 RVA: 0x004EB224 File Offset: 0x004E9424
		public void RequestUserDetailsUpdate(IReadOnlyList<UserDetailsRequest> requestedUsers, UserDetailsRequestCompleteHandler onComplete)
		{
			UserDetailsServiceEos.<>c__DisplayClass3_0 CS$<>8__locals1 = new UserDetailsServiceEos.<>c__DisplayClass3_0();
			CS$<>8__locals1.requestedUsers = requestedUsers;
			CS$<>8__locals1.onComplete = onComplete;
			List<MappedAccountRequest> list = null;
			CS$<>8__locals1.requestIndices = null;
			int i = 0;
			while (i < CS$<>8__locals1.requestedUsers.Count)
			{
				UserDetailsRequest userDetailsRequest = CS$<>8__locals1.requestedUsers[i];
				if (!userDetailsRequest.Id.Equals(this.user.PlatformUserId))
				{
					goto IL_007B;
				}
				string @string = GamePrefs.GetString(EnumGamePrefs.PlayerName);
				if (string.IsNullOrEmpty(@string))
				{
					Log.Error("[EOS] RequestUserDetailsUpdate: PlayerName not set yet, requesting details for local player");
					goto IL_007B;
				}
				userDetailsRequest.details.name = @string;
				userDetailsRequest.IsSuccess = true;
				IL_00CD:
				i++;
				continue;
				IL_007B:
				if (this.idMapper.CanQuery(userDetailsRequest.Id))
				{
					if (list == null)
					{
						list = new List<MappedAccountRequest>();
					}
					if (CS$<>8__locals1.requestIndices == null)
					{
						CS$<>8__locals1.requestIndices = new List<int>();
					}
					list.Add(new MappedAccountRequest(userDetailsRequest.Id, userDetailsRequest.NativePlatform));
					CS$<>8__locals1.requestIndices.Add(i);
					goto IL_00CD;
				}
				goto IL_00CD;
			}
			if (list == null)
			{
				CS$<>8__locals1.onComplete(CS$<>8__locals1.requestedUsers);
				return;
			}
			this.idMapper.QueryMappedAccountsDetails(list, new MappedAccountsQueryCallback(CS$<>8__locals1.<RequestUserDetailsUpdate>g__OnAccountsMapped|0));
		}

		// Token: 0x0400A5E1 RID: 42465
		[PublicizedFrom(EAccessModifier.Private)]
		public EosUserIdMapper idMapper;

		// Token: 0x0400A5E2 RID: 42466
		[PublicizedFrom(EAccessModifier.Private)]
		public User user;
	}
}
