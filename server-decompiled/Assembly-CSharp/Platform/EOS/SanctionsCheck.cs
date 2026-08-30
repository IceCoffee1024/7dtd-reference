using System;
using System.Collections;
using System.Collections.Generic;
using Epic.OnlineServices;
using Epic.OnlineServices.Sanctions;
using JetBrains.Annotations;
using Pathfinding.Util;

namespace Platform.EOS
{
	// Token: 0x02001CE0 RID: 7392
	public class SanctionsCheck
	{
		// Token: 0x0600DAC7 RID: 56007 RVA: 0x004E6586 File Offset: 0x004E4786
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator WaitUntilSanctionsCheck(CoroutineCancellationToken _cancellationToken = null)
		{
			while (this.queryInProgress)
			{
				if (_cancellationToken != null && _cancellationToken.IsCancelled())
				{
					yield break;
				}
				yield return null;
			}
			yield break;
		}

		// Token: 0x0600DAC8 RID: 56008 RVA: 0x004E659C File Offset: 0x004E479C
		[PublicizedFrom(EAccessModifier.Internal)]
		public IEnumerator CheckSanctionsEnumerator(SanctionsInterface _sanctionsInterface, ProductUserId _productUserId, [CanBeNull] ProductUserId _localUser, Action<SanctionsCheckResult> callback, CoroutineCancellationToken _cancellationToken = null)
		{
			if (this.queryInProgress)
			{
				yield return this.WaitUntilSanctionsCheck(_cancellationToken);
			}
			CoroutineCancellationToken cancellationToken = _cancellationToken;
			if (cancellationToken != null && cancellationToken.IsCancelled())
			{
				yield break;
			}
			this.queryInProgress = true;
			this.sanctionsInterface = _sanctionsInterface;
			this.productUserId = _productUserId;
			QueryActivePlayerSanctionsOptions queryActivePlayerSanctionsOptions = new QueryActivePlayerSanctionsOptions
			{
				TargetUserId = this.productUserId,
				LocalUserId = _localUser
			};
			object lockObject = AntiCheatCommon.LockObject;
			lock (lockObject)
			{
				this.sanctionsInterface.QueryActivePlayerSanctions(ref queryActivePlayerSanctionsOptions, null, delegate(ref QueryActivePlayerSanctionsCallbackInfo data)
				{
					CoroutineCancellationToken cancellationToken2 = _cancellationToken;
					if (cancellationToken2 != null && cancellationToken2.IsCancelled())
					{
						return;
					}
					this.OnSanctionsQueryResolveAndGatherSanctions(ref data, callback);
				});
			}
			yield return this.WaitUntilSanctionsCheck(_cancellationToken);
			yield break;
		}

		// Token: 0x0600DAC9 RID: 56009 RVA: 0x004E65D0 File Offset: 0x004E47D0
		[PublicizedFrom(EAccessModifier.Internal)]
		public void CheckSanctions(SanctionsInterface _sanctionsInterface, ProductUserId _productUserId, [CanBeNull] ProductUserId _localUser, Action<SanctionsCheckResult> callback)
		{
			if (this.queryInProgress)
			{
				ThreadManager.StartCoroutine(this.CheckSanctionsEnumerator(_sanctionsInterface, _productUserId, _localUser, callback, null));
				return;
			}
			this.queryInProgress = true;
			this.sanctionsInterface = _sanctionsInterface;
			this.productUserId = _productUserId;
			QueryActivePlayerSanctionsOptions queryActivePlayerSanctionsOptions = new QueryActivePlayerSanctionsOptions
			{
				TargetUserId = this.productUserId,
				LocalUserId = _localUser
			};
			object lockObject = AntiCheatCommon.LockObject;
			lock (lockObject)
			{
				this.sanctionsInterface.QueryActivePlayerSanctions(ref queryActivePlayerSanctionsOptions, null, delegate(ref QueryActivePlayerSanctionsCallbackInfo data)
				{
					this.OnSanctionsQueryResolveAndGatherSanctions(ref data, callback);
				});
			}
		}

		// Token: 0x0600DACA RID: 56010 RVA: 0x004E6690 File Offset: 0x004E4890
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnSanctionsQueryResolveAndGatherSanctions(ref QueryActivePlayerSanctionsCallbackInfo data, Action<SanctionsCheckResult> callback)
		{
			if (data.ResultCode == Result.NotFound)
			{
				Log.Out("[EOS] Player has no sanctions");
				this.queryInProgress = false;
				List<EOSSanction> currentCheckSanctions = this.CurrentCheckSanctions;
				if (currentCheckSanctions != null)
				{
					currentCheckSanctions.ClearFast<EOSSanction>();
				}
				if (callback != null)
				{
					callback(new SanctionsCheckResult(null));
				}
				return;
			}
			if (data.ResultCode != Result.Success)
			{
				Log.Out(string.Format("[EOS] Sanctions query failed {0}", data.ResultCode));
				if (data.ResultCode == Result.OperationWillRetry)
				{
					return;
				}
				this.queryInProgress = false;
				List<EOSSanction> currentCheckSanctions2 = this.CurrentCheckSanctions;
				if (currentCheckSanctions2 != null)
				{
					currentCheckSanctions2.ClearFast<EOSSanction>();
				}
				if (callback != null)
				{
					callback(new SanctionsCheckResult(default(DateTime), GameUtils.EKickReason.CrossPlatformAuthenticationFailed, 5, data.ResultCode.ToStringCached<Result>()));
				}
			}
			else
			{
				Log.Out("[EOS] Player may have active sanctions");
				GetPlayerSanctionCountOptions getPlayerSanctionCountOptions = new GetPlayerSanctionCountOptions
				{
					TargetUserId = this.productUserId
				};
				object obj = AntiCheatCommon.LockObject;
				uint playerSanctionCount;
				lock (obj)
				{
					playerSanctionCount = this.sanctionsInterface.GetPlayerSanctionCount(ref getPlayerSanctionCountOptions);
				}
				this.CurrentCheckSanctions = new List<EOSSanction>();
				for (uint num = 0U; num < playerSanctionCount; num += 1U)
				{
					CopyPlayerSanctionByIndexOptions copyPlayerSanctionByIndexOptions = new CopyPlayerSanctionByIndexOptions
					{
						SanctionIndex = num,
						TargetUserId = this.productUserId
					};
					obj = AntiCheatCommon.LockObject;
					lock (obj)
					{
						PlayerSanction? playerSanction;
						if (this.sanctionsInterface.CopyPlayerSanctionByIndex(ref copyPlayerSanctionByIndexOptions, out playerSanction) == Result.Success)
						{
							Log.Error("[EOS] Sanction found: " + (((playerSanction != null) ? playerSanction.GetValueOrDefault().Action : null) ?? "Empty Action").ToString());
							if (playerSanction != null && playerSanction.GetValueOrDefault().Action.ToString().Equals("RESTRICT_MATCHMAKING"))
							{
								Log.Error("[EOS] Sanction in place");
								if (playerSanction.Value.TimeExpires == 0L)
								{
									Log.Out("[EOS] Sanctioned Until: Forever");
									this.CurrentCheckSanctions.Add(new EOSSanction(new DateTime?(DateTime.MaxValue), playerSanction.Value.ReferenceId));
									break;
								}
								DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
								dateTime = dateTime.AddSeconds((double)playerSanction.Value.TimeExpires).ToLocalTime();
								Log.Out("[EOS] Sanctioned Until: " + dateTime.ToLongDateString());
								this.CurrentCheckSanctions.Add(new EOSSanction(new DateTime?(dateTime), playerSanction.Value.ReferenceId));
							}
						}
					}
				}
			}
			string text = "[EOS] SanctionsQuery finished with: {0} sanctions";
			List<EOSSanction> currentCheckSanctions3 = this.CurrentCheckSanctions;
			Log.Out(string.Format(text, (currentCheckSanctions3 != null) ? currentCheckSanctions3.Count : 0));
			this.queryInProgress = false;
			if (callback != null)
			{
				callback(new SanctionsCheckResult(this.CurrentCheckSanctions));
			}
		}

		// Token: 0x0400A55D RID: 42333
		[PublicizedFrom(EAccessModifier.Private)]
		public ProductUserId productUserId;

		// Token: 0x0400A55E RID: 42334
		[PublicizedFrom(EAccessModifier.Private)]
		public SanctionsInterface sanctionsInterface;

		// Token: 0x0400A55F RID: 42335
		[PublicizedFrom(EAccessModifier.Private)]
		public bool queryInProgress;

		// Token: 0x0400A560 RID: 42336
		[PublicizedFrom(EAccessModifier.Private)]
		public List<EOSSanction> CurrentCheckSanctions;
	}
}
