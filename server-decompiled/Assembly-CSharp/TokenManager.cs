using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000464 RID: 1124
public class TokenManager
{
	// Token: 0x060021F9 RID: 8697 RVA: 0x000CDB54 File Offset: 0x000CBD54
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

	// Token: 0x060021FA RID: 8698 RVA: 0x000CDD24 File Offset: 0x000CBF24
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

	// Token: 0x060021FB RID: 8699 RVA: 0x000CDDDC File Offset: 0x000CBFDC
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
	// (get) Token: 0x060021FC RID: 8700 RVA: 0x000CDE68 File Offset: 0x000CC068
	// (set) Token: 0x060021FD RID: 8701 RVA: 0x000CDE6F File Offset: 0x000CC06F
	public static TokenManager Instance
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x060021FE RID: 8702 RVA: 0x000CDE77 File Offset: 0x000CC077
	public static void Init()
	{
		TokenManager.Instance = new TokenManager();
		TokenManager.Instance.Load();
	}

	// Token: 0x060021FF RID: 8703 RVA: 0x000CDE8D File Offset: 0x000CC08D
	public static void Cleanup()
	{
		TokenManager instance = TokenManager.Instance;
		if (instance != null)
		{
			instance.EntityTokenStates.Clear();
		}
		TokenManager.Instance = null;
	}

	// Token: 0x06002200 RID: 8704 RVA: 0x000027FC File Offset: 0x000009FC
	public void Clear()
	{
	}

	// Token: 0x06002201 RID: 8705 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void Load()
	{
	}

	// Token: 0x06002202 RID: 8706 RVA: 0x000CDEAC File Offset: 0x000CC0AC
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

	// Token: 0x04001788 RID: 6024
	[PublicizedFrom(EAccessModifier.Private)]
	public static Stack<EntityAlive> deadEntities = new Stack<EntityAlive>();

	// Token: 0x04001789 RID: 6025
	[PublicizedFrom(EAccessModifier.Private)]
	public static List<TokenManager.TokenRequester> deadRequesters = new List<TokenManager.TokenRequester>();

	// Token: 0x0400178B RID: 6027
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<EntityAlive, TokenManager.EntityTokenState> EntityTokenStates = new Dictionary<EntityAlive, TokenManager.EntityTokenState>();

	// Token: 0x02000465 RID: 1125
	public class TokenRequester
	{
		// Token: 0x0400178C RID: 6028
		public EntityAlive Entity;

		// Token: 0x0400178D RID: 6029
		public double WorldTimeExpireRequest;

		// Token: 0x0400178E RID: 6030
		public double WorldTimeExpireClaim;

		// Token: 0x0400178F RID: 6031
		public double WorldTimeLastToken;
	}

	// Token: 0x02000466 RID: 1126
	public struct TokenState
	{
		// Token: 0x06002206 RID: 8710 RVA: 0x000CE0B5 File Offset: 0x000CC2B5
		public TokenState(int maxClaims)
		{
			this.MaxClaims = maxClaims;
			this.Claims = new HashSet<TokenManager.TokenRequester>();
			this.Requesters = new Queue<TokenManager.TokenRequester>();
		}

		// Token: 0x04001790 RID: 6032
		public readonly int MaxClaims;

		// Token: 0x04001791 RID: 6033
		public readonly HashSet<TokenManager.TokenRequester> Claims;

		// Token: 0x04001792 RID: 6034
		public readonly Queue<TokenManager.TokenRequester> Requesters;
	}

	// Token: 0x02000467 RID: 1127
	[PublicizedFrom(EAccessModifier.Private)]
	public class EntityTokenState
	{
		// Token: 0x06002207 RID: 8711 RVA: 0x000CE0D4 File Offset: 0x000CC2D4
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

		// Token: 0x04001793 RID: 6035
		public readonly EntityAlive Entity;

		// Token: 0x04001794 RID: 6036
		public readonly TokenManager.TokenState[] TokenStates = new TokenManager.TokenState[Enum.GetValues(typeof(AITokenType)).Length];
	}
}
