using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000466 RID: 1126
public class TokenManager
{
	// Token: 0x060021FE RID: 8702 RVA: 0x000CDF00 File Offset: 0x000CC100
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

	// Token: 0x060021FF RID: 8703 RVA: 0x000CE0D0 File Offset: 0x000CC2D0
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

	// Token: 0x06002200 RID: 8704 RVA: 0x000CE188 File Offset: 0x000CC388
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

	// Token: 0x170003DD RID: 989
	// (get) Token: 0x06002201 RID: 8705 RVA: 0x000CE214 File Offset: 0x000CC414
	// (set) Token: 0x06002202 RID: 8706 RVA: 0x000CE21B File Offset: 0x000CC41B
	public static TokenManager Instance
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x06002203 RID: 8707 RVA: 0x000CE223 File Offset: 0x000CC423
	public static void Init()
	{
		TokenManager.Instance = new TokenManager();
		TokenManager.Instance.Load();
	}

	// Token: 0x06002204 RID: 8708 RVA: 0x000CE239 File Offset: 0x000CC439
	public static void Cleanup()
	{
		TokenManager instance = TokenManager.Instance;
		if (instance != null)
		{
			instance.EntityTokenStates.Clear();
		}
		TokenManager.Instance = null;
	}

	// Token: 0x06002205 RID: 8709 RVA: 0x000027FC File Offset: 0x000009FC
	public void Clear()
	{
	}

	// Token: 0x06002206 RID: 8710 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void Load()
	{
	}

	// Token: 0x06002207 RID: 8711 RVA: 0x000CE258 File Offset: 0x000CC458
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

	// Token: 0x04001798 RID: 6040
	[PublicizedFrom(EAccessModifier.Private)]
	public static Stack<EntityAlive> deadEntities = new Stack<EntityAlive>();

	// Token: 0x04001799 RID: 6041
	[PublicizedFrom(EAccessModifier.Private)]
	public static List<TokenManager.TokenRequester> deadRequesters = new List<TokenManager.TokenRequester>();

	// Token: 0x0400179B RID: 6043
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<EntityAlive, TokenManager.EntityTokenState> EntityTokenStates = new Dictionary<EntityAlive, TokenManager.EntityTokenState>();

	// Token: 0x02000467 RID: 1127
	public class TokenRequester
	{
		// Token: 0x0400179C RID: 6044
		public EntityAlive Entity;

		// Token: 0x0400179D RID: 6045
		public double WorldTimeExpireRequest;

		// Token: 0x0400179E RID: 6046
		public double WorldTimeExpireClaim;

		// Token: 0x0400179F RID: 6047
		public double WorldTimeLastToken;
	}

	// Token: 0x02000468 RID: 1128
	public struct TokenState
	{
		// Token: 0x0600220B RID: 8715 RVA: 0x000CE461 File Offset: 0x000CC661
		public TokenState(int maxClaims)
		{
			this.MaxClaims = maxClaims;
			this.Claims = new HashSet<TokenManager.TokenRequester>();
			this.Requesters = new Queue<TokenManager.TokenRequester>();
		}

		// Token: 0x040017A0 RID: 6048
		public readonly int MaxClaims;

		// Token: 0x040017A1 RID: 6049
		public readonly HashSet<TokenManager.TokenRequester> Claims;

		// Token: 0x040017A2 RID: 6050
		public readonly Queue<TokenManager.TokenRequester> Requesters;
	}

	// Token: 0x02000469 RID: 1129
	[PublicizedFrom(EAccessModifier.Private)]
	public class EntityTokenState
	{
		// Token: 0x0600220C RID: 8716 RVA: 0x000CE480 File Offset: 0x000CC680
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

		// Token: 0x040017A3 RID: 6051
		public readonly EntityAlive Entity;

		// Token: 0x040017A4 RID: 6052
		public readonly TokenManager.TokenState[] TokenStates = new TokenManager.TokenState[Enum.GetValues(typeof(AITokenType)).Length];
	}
}
