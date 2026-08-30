using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200045E RID: 1118
public class TokenManager
{
	// Token: 0x060021ED RID: 8685 RVA: 0x000CD304 File Offset: 0x000CB504
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

	// Token: 0x060021EE RID: 8686 RVA: 0x000CD4D4 File Offset: 0x000CB6D4
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

	// Token: 0x060021EF RID: 8687 RVA: 0x000CD58C File Offset: 0x000CB78C
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

	// Token: 0x170003E5 RID: 997
	// (get) Token: 0x060021F0 RID: 8688 RVA: 0x000CD618 File Offset: 0x000CB818
	// (set) Token: 0x060021F1 RID: 8689 RVA: 0x000CD61F File Offset: 0x000CB81F
	public static TokenManager Instance
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x060021F2 RID: 8690 RVA: 0x000CD627 File Offset: 0x000CB827
	public static void Init()
	{
		TokenManager.Instance = new TokenManager();
		TokenManager.Instance.Load();
	}

	// Token: 0x060021F3 RID: 8691 RVA: 0x000CD63D File Offset: 0x000CB83D
	public static void Cleanup()
	{
		TokenManager instance = TokenManager.Instance;
		if (instance != null)
		{
			instance.EntityTokenStates.Clear();
		}
		TokenManager.Instance = null;
	}

	// Token: 0x060021F4 RID: 8692 RVA: 0x000027FC File Offset: 0x000009FC
	public void Clear()
	{
	}

	// Token: 0x060021F5 RID: 8693 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void Load()
	{
	}

	// Token: 0x060021F6 RID: 8694 RVA: 0x000CD65C File Offset: 0x000CB85C
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

	// Token: 0x0400177B RID: 6011
	[PublicizedFrom(EAccessModifier.Private)]
	public static Stack<EntityAlive> deadEntities = new Stack<EntityAlive>();

	// Token: 0x0400177C RID: 6012
	[PublicizedFrom(EAccessModifier.Private)]
	public static List<TokenManager.TokenRequester> deadRequesters = new List<TokenManager.TokenRequester>();

	// Token: 0x0400177E RID: 6014
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<EntityAlive, TokenManager.EntityTokenState> EntityTokenStates = new Dictionary<EntityAlive, TokenManager.EntityTokenState>();

	// Token: 0x0200045F RID: 1119
	public class TokenRequester
	{
		// Token: 0x0400177F RID: 6015
		public EntityAlive Entity;

		// Token: 0x04001780 RID: 6016
		public double WorldTimeExpireRequest;

		// Token: 0x04001781 RID: 6017
		public double WorldTimeExpireClaim;

		// Token: 0x04001782 RID: 6018
		public double WorldTimeLastToken;
	}

	// Token: 0x02000460 RID: 1120
	public struct TokenState
	{
		// Token: 0x060021FA RID: 8698 RVA: 0x000CD865 File Offset: 0x000CBA65
		public TokenState(int maxClaims)
		{
			this.MaxClaims = maxClaims;
			this.Claims = new HashSet<TokenManager.TokenRequester>();
			this.Requesters = new Queue<TokenManager.TokenRequester>();
		}

		// Token: 0x04001783 RID: 6019
		public readonly int MaxClaims;

		// Token: 0x04001784 RID: 6020
		public readonly HashSet<TokenManager.TokenRequester> Claims;

		// Token: 0x04001785 RID: 6021
		public readonly Queue<TokenManager.TokenRequester> Requesters;
	}

	// Token: 0x02000461 RID: 1121
	[PublicizedFrom(EAccessModifier.Private)]
	public class EntityTokenState
	{
		// Token: 0x060021FB RID: 8699 RVA: 0x000CD884 File Offset: 0x000CBA84
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

		// Token: 0x04001786 RID: 6022
		public readonly EntityAlive Entity;

		// Token: 0x04001787 RID: 6023
		public readonly TokenManager.TokenState[] TokenStates = new TokenManager.TokenState[Enum.GetValues(typeof(AITokenType)).Length];
	}
}
