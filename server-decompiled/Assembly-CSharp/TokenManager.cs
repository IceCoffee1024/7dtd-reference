using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000462 RID: 1122
public class TokenManager
{
	// Token: 0x0600220A RID: 8714 RVA: 0x000CDAA0 File Offset: 0x000CBCA0
	public AITokenClaim TryClaimToken(EntityAlive claimer, AITokenType tokenType, EntityAlive target, float claimTimeout, AIClaimTokenFlags claimTokenFlags = AIClaimTokenFlags.None)
	{
		if (claimer == null || target == null)
		{
			return AITokenClaim.NoneAvailable;
		}
		TokenManager.EntityTokenState entityTokenState;
		if (!this.EntityTokenStates.TryGetValue(target, out entityTokenState))
		{
			entityTokenState = new TokenManager.EntityTokenState(target);
			this.EntityTokenStates[target] = entityTokenState;
		}
		double num = GameManager.Instance.World.worldTime / 20.0;
		double num2 = 0.0;
		int num3 = 0;
		int num4 = 0;
		ref TokenManager.TokenState ptr = ref entityTokenState.TokenStates[(int)tokenType];
		bool flag = false;
		TokenManager.TokenRequester tokenRequester = null;
		int count = ptr.Requesters.Count;
		for (int i = 0; i < count; i++)
		{
			TokenManager.TokenRequester tokenRequester2 = ptr.Requesters.Dequeue();
			if (!(tokenRequester2.Entity == null))
			{
				if (tokenRequester2.WorldTimeExpireRequest > num)
				{
					num3++;
					num2 += num - tokenRequester2.WorldTimeLastToken;
				}
				bool flag2 = ptr.Claims.Contains(tokenRequester2);
				if (flag2)
				{
					num4++;
				}
				if (tokenRequester == null && tokenRequester2.Entity == claimer)
				{
					flag = flag2;
					if ((claimTokenFlags & AIClaimTokenFlags.UpdateTimeout) != AIClaimTokenFlags.None)
					{
						tokenRequester2.WorldTimeExpireRequest = num + 0.5;
					}
					tokenRequester = tokenRequester2;
					ptr.Requesters.Enqueue(tokenRequester2);
				}
				else
				{
					ptr.Requesters.Enqueue(tokenRequester2);
				}
			}
		}
		if (tokenRequester == null)
		{
			tokenRequester = new TokenManager.TokenRequester
			{
				Entity = claimer,
				WorldTimeExpireRequest = num + 0.5
			};
			ptr.Requesters.Enqueue(tokenRequester);
		}
		if (flag)
		{
			return AITokenClaim.AlreadyClaimed;
		}
		if (num4 < ptr.MaxClaims)
		{
			double num5 = (num - tokenRequester.WorldTimeLastToken) / num2;
			if ((double)global::UnityEngine.Random.value <= num5)
			{
				tokenRequester.Entity = claimer;
				tokenRequester.WorldTimeExpireClaim = num + (double)claimTimeout;
				ptr.Claims.Add(tokenRequester);
				return AITokenClaim.Claimed;
			}
		}
		return AITokenClaim.NoneAvailable;
	}

	// Token: 0x0600220B RID: 8715 RVA: 0x000CDC70 File Offset: 0x000CBE70
	public void ReleaseClaimToken(AITokenType tokenType, EntityAlive claimer, EntityAlive target)
	{
		TokenManager.EntityTokenState entityTokenState;
		if (this.EntityTokenStates.TryGetValue(target, out entityTokenState))
		{
			ref TokenManager.TokenState ptr = ref entityTokenState.TokenStates[(int)tokenType];
			foreach (TokenManager.TokenRequester tokenRequester in ptr.Requesters)
			{
				if (tokenRequester.Entity == claimer)
				{
					tokenRequester.WorldTimeExpireClaim = 0.0;
					tokenRequester.WorldTimeLastToken = GameManager.Instance.World.worldTime / 20.0;
					ptr.Claims.Remove(tokenRequester);
				}
			}
		}
	}

	// Token: 0x0600220C RID: 8716 RVA: 0x000CDD28 File Offset: 0x000CBF28
	public bool HasClaimToken(AITokenType tokenType, EntityAlive claimer, EntityAlive target)
	{
		TokenManager.EntityTokenState entityTokenState;
		if (this.EntityTokenStates.TryGetValue(target, out entityTokenState))
		{
			ref TokenManager.TokenState ptr = ref entityTokenState.TokenStates[(int)tokenType];
			foreach (TokenManager.TokenRequester tokenRequester in ptr.Requesters)
			{
				if (tokenRequester.Entity == claimer)
				{
					return ptr.Claims.Contains(tokenRequester);
				}
			}
			return false;
		}
		return false;
	}

	// Token: 0x170003E9 RID: 1001
	// (get) Token: 0x0600220D RID: 8717 RVA: 0x000CDDB4 File Offset: 0x000CBFB4
	// (set) Token: 0x0600220E RID: 8718 RVA: 0x000CDDBB File Offset: 0x000CBFBB
	public static TokenManager Instance
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x0600220F RID: 8719 RVA: 0x000CDDC3 File Offset: 0x000CBFC3
	public static void Init()
	{
		TokenManager.Instance = new TokenManager();
		TokenManager.Instance.Load();
	}

	// Token: 0x06002210 RID: 8720 RVA: 0x000CDDD9 File Offset: 0x000CBFD9
	public static void Cleanup()
	{
		TokenManager instance = TokenManager.Instance;
		if (instance != null)
		{
			instance.EntityTokenStates.Clear();
		}
		TokenManager.Instance = null;
	}

	// Token: 0x06002211 RID: 8721 RVA: 0x000027FC File Offset: 0x000009FC
	public void Clear()
	{
	}

	// Token: 0x06002212 RID: 8722 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void Load()
	{
	}

	// Token: 0x06002213 RID: 8723 RVA: 0x000CDDF8 File Offset: 0x000CBFF8
	public void Update()
	{
		using (new ProfilerScope("TokenManager"))
		{
			double num = GameManager.Instance.World.worldTime / 20.0;
			TokenManager.deadEntities.Clear();
			foreach (KeyValuePair<EntityAlive, TokenManager.EntityTokenState> keyValuePair in this.EntityTokenStates)
			{
				if (keyValuePair.Key == null)
				{
					TokenManager.deadEntities.Push(keyValuePair.Key);
				}
				else
				{
					for (int i = 0; i < keyValuePair.Value.TokenStates.Length; i++)
					{
						ref TokenManager.TokenState ptr = ref keyValuePair.Value.TokenStates[i];
						TokenManager.deadRequesters.Clear();
						foreach (TokenManager.TokenRequester tokenRequester in ptr.Claims)
						{
							if (tokenRequester.WorldTimeExpireClaim < num)
							{
								TokenManager.deadRequesters.Add(tokenRequester);
							}
							else
							{
								tokenRequester.WorldTimeLastToken = num;
							}
						}
						if (TokenManager.deadRequesters.Count > 0)
						{
							ptr.Claims.RemoveWhere((TokenManager.TokenRequester x) => TokenManager.deadRequesters.Contains(x));
						}
					}
				}
			}
			EntityAlive entityAlive;
			while (TokenManager.deadEntities.TryPop(out entityAlive))
			{
				this.EntityTokenStates.Remove(entityAlive);
			}
		}
	}

	// Token: 0x04001791 RID: 6033
	[PublicizedFrom(EAccessModifier.Private)]
	public static Stack<EntityAlive> deadEntities = new Stack<EntityAlive>();

	// Token: 0x04001792 RID: 6034
	[PublicizedFrom(EAccessModifier.Private)]
	public static List<TokenManager.TokenRequester> deadRequesters = new List<TokenManager.TokenRequester>();

	// Token: 0x04001794 RID: 6036
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<EntityAlive, TokenManager.EntityTokenState> EntityTokenStates = new Dictionary<EntityAlive, TokenManager.EntityTokenState>();

	// Token: 0x02000463 RID: 1123
	public class TokenRequester
	{
		// Token: 0x04001795 RID: 6037
		public EntityAlive Entity;

		// Token: 0x04001796 RID: 6038
		public double WorldTimeExpireRequest;

		// Token: 0x04001797 RID: 6039
		public double WorldTimeExpireClaim;

		// Token: 0x04001798 RID: 6040
		public double WorldTimeLastToken;
	}

	// Token: 0x02000464 RID: 1124
	public struct TokenState
	{
		// Token: 0x06002217 RID: 8727 RVA: 0x000CE001 File Offset: 0x000CC201
		public TokenState(int maxClaims)
		{
			this.MaxClaims = maxClaims;
			this.Claims = new HashSet<TokenManager.TokenRequester>();
			this.Requesters = new Queue<TokenManager.TokenRequester>();
		}

		// Token: 0x04001799 RID: 6041
		public readonly int MaxClaims;

		// Token: 0x0400179A RID: 6042
		public readonly HashSet<TokenManager.TokenRequester> Claims;

		// Token: 0x0400179B RID: 6043
		public readonly Queue<TokenManager.TokenRequester> Requesters;
	}

	// Token: 0x02000465 RID: 1125
	[PublicizedFrom(EAccessModifier.Private)]
	public class EntityTokenState
	{
		// Token: 0x06002218 RID: 8728 RVA: 0x000CE020 File Offset: 0x000CC220
		public EntityTokenState(EntityAlive entity)
		{
			this.Entity = entity;
			Dictionary<AITokenType, AITokenConfig> tokenManagerConfig = this.Entity.EntityClass.TokenManagerConfig;
			for (int i = 0; i < this.TokenStates.Length; i++)
			{
				AITokenConfig aitokenConfig;
				if (tokenManagerConfig != null && tokenManagerConfig.TryGetValue((AITokenType)i, out aitokenConfig))
				{
					this.TokenStates[i] = new TokenManager.TokenState(aitokenConfig.MaxClaims);
				}
				else
				{
					this.TokenStates[i] = new TokenManager.TokenState(0);
				}
			}
		}

		// Token: 0x0400179C RID: 6044
		public readonly EntityAlive Entity;

		// Token: 0x0400179D RID: 6045
		public readonly TokenManager.TokenState[] TokenStates = new TokenManager.TokenState[Enum.GetValues(typeof(AITokenType)).Length];
	}
}
