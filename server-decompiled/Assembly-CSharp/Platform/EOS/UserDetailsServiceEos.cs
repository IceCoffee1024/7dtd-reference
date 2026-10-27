using System;
using System.Collections.Generic;

namespace Platform.EOS
{
	// Token: 0x02001D8B RID: 7563
	public class UserDetailsServiceEos : IUserDetailsService
	{
		// Token: 0x0600DFB9 RID: 57273 RVA: 0x004FA459 File Offset: 0x004F8659
		public void Init(IPlatform owner)
		{
			this.idMapper = (EosUserIdMapper)owner.IdMappingService;
			this.user = (User)owner.User;
		}

		// Token: 0x0600DFBA RID: 57274 RVA: 0x004FA480 File Offset: 0x004F8680
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

		// Token: 0x0400A8D6 RID: 43222
		[PublicizedFrom(EAccessModifier.Private)]
		public EosUserIdMapper idMapper;

		// Token: 0x0400A8D7 RID: 43223
		[PublicizedFrom(EAccessModifier.Private)]
		public User user;
	}
}
