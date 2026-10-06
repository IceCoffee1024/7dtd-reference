using System;
using System.Collections;
using System.Collections.Generic;
using DynamicMusic.Factories;
using MusicUtils.Enums;
using UniLinq;
using UnityEngine.Scripting;

namespace DynamicMusic
{
	// Token: 0x02001A63 RID: 6755
	[Preserve]
	public class Conductor : IUpdatable, ICleanable
	{
		// Token: 0x17001986 RID: 6534
		// (get) Token: 0x0600CD03 RID: 52483 RVA: 0x004AB501 File Offset: 0x004A9701
		// (set) Token: 0x0600CD04 RID: 52484 RVA: 0x004AB509 File Offset: 0x004A9709
		public SectionType CurrentSectionType
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x0600CD05 RID: 52485 RVA: 0x004AB512 File Offset: 0x004A9712
		public Conductor()
		{
			if (GameManager.IsDedicatedServer)
			{
				return;
			}
			MixerController.Instance.Init();
		}

		// Token: 0x0600CD06 RID: 52486 RVA: 0x004AB52C File Offset: 0x004A972C
		public void Init(bool ReadyImmediate = false)
		{
			this.world = GameManager.Instance.World;
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer && this.PlayerEligibleForBloodmoonCache == null)
			{
				this.PlayerEligibleForBloodmoonCache = new Dictionary<int, bool>();
				ConnectionManager.OnClientDisconnected += this.OnClientDisconnected;
				Log.Out("Dynamic Music Initialized on Server");
			}
			if (GameManager.IsDedicatedServer)
			{
				Log.Out("Dynamic Music Initialized on Dedi");
				return;
			}
			LayeredContent.ReadyQueuesImmediate();
			this.sections = new EnumDictionary<SectionType, ISection>
			{
				{
					SectionType.Exploration,
					Factory.CreateSection<Adventure>(SectionType.Exploration)
				},
				{
					SectionType.Suspense,
					Factory.CreateSection<Adventure>(SectionType.Suspense)
				},
				{
					SectionType.Combat,
					Factory.CreateSection<Combat>(SectionType.Combat)
				},
				{
					SectionType.Bloodmoon,
					Factory.CreateSection<Bloodmoon>(SectionType.Bloodmoon)
				},
				{
					SectionType.HomeDay,
					Factory.CreateSection<Song>(SectionType.HomeDay)
				},
				{
					SectionType.HomeNight,
					Factory.CreateSection<Song>(SectionType.HomeNight)
				},
				{
					SectionType.TraderBob,
					Factory.CreateSection<Song>(SectionType.TraderBob)
				},
				{
					SectionType.TraderHugh,
					Factory.CreateSection<Song>(SectionType.TraderHugh)
				},
				{
					SectionType.TraderJen,
					Factory.CreateSection<Song>(SectionType.TraderJen)
				},
				{
					SectionType.TraderJoel,
					Factory.CreateSection<Song>(SectionType.TraderJoel)
				},
				{
					SectionType.TraderRekt,
					Factory.CreateSection<Song>(SectionType.TraderRekt)
				}
			};
			this.sectionSelector = Factory.CreateSectionSelector();
			this.wasMusicPlaying = false;
			Log.Out("Dynamic Music Initialized on Client");
		}

		// Token: 0x17001987 RID: 6535
		// (get) Token: 0x0600CD07 RID: 52487 RVA: 0x004AB658 File Offset: 0x004A9858
		public bool IsMusicPlaying
		{
			get
			{
				if (this.sections != null)
				{
					using (Dictionary<SectionType, ISection>.ValueCollection.Enumerator enumerator = this.sections.Values.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (enumerator.Current.IsPlaying)
							{
								return true;
							}
						}
					}
					return false;
				}
				return false;
			}
		}

		// Token: 0x0600CD08 RID: 52488 RVA: 0x004AB6C0 File Offset: 0x004A98C0
		public IEnumerator PreloadRoutine()
		{
			Log.Out("Begin DMS Conductor Preload Routine");
			foreach (KeyValuePair<SectionType, ISection> keyValuePair in this.sections)
			{
				Section section = keyValuePair.Value as Section;
				if (section != null)
				{
					yield return section.PreloadRoutine();
				}
			}
			Dictionary<SectionType, ISection>.Enumerator enumerator = default(Dictionary<SectionType, ISection>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x0600CD09 RID: 52489 RVA: 0x004AB6D0 File Offset: 0x004A98D0
		public void Update()
		{
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
			{
				Dictionary<int, bool> playerEligibleForBloodmoonCache = this.PlayerEligibleForBloodmoonCache;
				lock (playerEligibleForBloodmoonCache)
				{
					foreach (EntityPlayer entityPlayer in this.world.Players.list)
					{
						bool flag4;
						if (entityPlayer.bloodMoonParty != null && entityPlayer.bloodMoonParty.partySpawner != null)
						{
							bool flag2 = entityPlayer.bloodMoonParty.partySpawner.partyMembers.Max<EntityPlayer>((EntityPlayer p) => p.Progression.GetLevel()) > 1 && (!entityPlayer.bloodMoonParty.partySpawner.IsDone || entityPlayer.bloodMoonParty.BloodmoonZombiesRemain);
							if (entityPlayer is EntityPlayerLocal)
							{
								this.IsBloodmoonMusicEligible = flag2;
							}
							else
							{
								bool flag3;
								if (!this.PlayerEligibleForBloodmoonCache.TryGetValue(entityPlayer.entityId, out flag3))
								{
									this.PlayerEligibleForBloodmoonCache.Add(entityPlayer.entityId, flag2);
								}
								if (flag3 != flag2)
								{
									this.PlayerEligibleForBloodmoonCache[entityPlayer.entityId] = flag2;
									NetPackageBloodmoonMusic netPackageBloodmoonMusic = NetPackageManager.GetPackage<NetPackageBloodmoonMusic>().Setup(flag2);
									SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(netPackageBloodmoonMusic, false, entityPlayer.entityId, -1, -1, null, 192, false);
								}
							}
						}
						else if (entityPlayer is EntityPlayerLocal)
						{
							this.IsBloodmoonMusicEligible = false;
						}
						else if (this.PlayerEligibleForBloodmoonCache.TryGetValue(entityPlayer.entityId, out flag4) && flag4)
						{
							this.PlayerEligibleForBloodmoonCache[entityPlayer.entityId] = false;
							NetPackageBloodmoonMusic netPackageBloodmoonMusic2 = NetPackageManager.GetPackage<NetPackageBloodmoonMusic>().Setup(false);
							SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(netPackageBloodmoonMusic2, false, entityPlayer.entityId, -1, -1, null, 192, false);
						}
						else if (!this.PlayerEligibleForBloodmoonCache.ContainsKey(entityPlayer.entityId))
						{
							this.PlayerEligibleForBloodmoonCache.Add(entityPlayer.entityId, false);
							NetPackageBloodmoonMusic netPackageBloodmoonMusic3 = NetPackageManager.GetPackage<NetPackageBloodmoonMusic>().Setup(false);
							SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(netPackageBloodmoonMusic3, false, entityPlayer.entityId, -1, -1, null, 192, false);
						}
					}
				}
			}
			if (GameManager.IsDedicatedServer)
			{
				return;
			}
			bool flag5 = this.wasMusicPlaying;
			bool flag = (this.wasMusicPlaying = this.IsMusicPlaying);
			if (flag5 & !flag)
			{
				this.sectionSelector.Notify(MusicActionType.Stop);
				Log.Out("Notified SectionSelector that music stopped");
			}
			SectionType sectionType = this.sectionSelector.Select();
			if (this.CurrentSectionType != sectionType)
			{
				Log.Out(string.Format("SectionType change from {0} to {1}", this.CurrentSectionType, sectionType));
				if (this.CurrentSectionType == SectionType.None)
				{
					ISection section;
					if (this.sections.TryGetValue(sectionType, out section))
					{
						section.FadeIn();
						this.CurrentSection = section;
						this.sectionSelector.Notify(MusicActionType.Play);
						Log.Out("Notified SectionSelector that music played");
					}
				}
				else
				{
					if (this.CurrentSection != null && this.CurrentSection.IsPlaying && !this.CurrentSection.IsPaused)
					{
						this.CurrentSection.FadeOut();
					}
					ISection section2;
					if (this.sections.TryGetValue(sectionType, out section2))
					{
						section2.FadeIn();
						this.sectionSelector.Notify(MusicActionType.FadeIn);
						this.CurrentSection = section2;
					}
					else
					{
						this.CurrentSection = null;
					}
				}
			}
			this.CurrentSectionType = sectionType;
			MixerController.Instance.Update();
		}

		// Token: 0x0600CD0A RID: 52490 RVA: 0x004ABA84 File Offset: 0x004A9C84
		public void OnPauseGame()
		{
			if (GameManager.IsDedicatedServer)
			{
				return;
			}
			if (this.CurrentSection != null)
			{
				this.CurrentSection.Pause();
			}
			if (this.sectionSelector != null)
			{
				this.sectionSelector.Notify(MusicActionType.Pause);
			}
		}

		// Token: 0x0600CD0B RID: 52491 RVA: 0x004ABAB5 File Offset: 0x004A9CB5
		public void OnUnPauseGame()
		{
			if (GameManager.IsDedicatedServer)
			{
				return;
			}
			if (this.CurrentSection != null)
			{
				this.CurrentSection.UnPause();
			}
			if (this.sectionSelector != null)
			{
				this.sectionSelector.Notify(MusicActionType.UnPause);
			}
		}

		// Token: 0x0600CD0C RID: 52492 RVA: 0x004ABAE8 File Offset: 0x004A9CE8
		public void CleanUp()
		{
			if (GameManager.IsDedicatedServer)
			{
				return;
			}
			this.sectionSelector = null;
			if (this.sections != null)
			{
				foreach (ISection section in this.sections.Values)
				{
					section.CleanUp();
				}
				this.sections.Clear();
			}
			this.sections = null;
			this.CurrentSection = null;
			this.CurrentSectionType = SectionType.None;
			LayeredContent.ClearQueues();
		}

		// Token: 0x0600CD0D RID: 52493 RVA: 0x004ABB7C File Offset: 0x004A9D7C
		public void OnWorldExit()
		{
			ConnectionManager.OnClientDisconnected -= this.OnClientDisconnected;
			if (this.PlayerEligibleForBloodmoonCache != null)
			{
				Dictionary<int, bool> playerEligibleForBloodmoonCache = this.PlayerEligibleForBloodmoonCache;
				lock (playerEligibleForBloodmoonCache)
				{
					this.PlayerEligibleForBloodmoonCache.Clear();
					this.PlayerEligibleForBloodmoonCache = null;
				}
			}
		}

		// Token: 0x0600CD0E RID: 52494 RVA: 0x004ABBE4 File Offset: 0x004A9DE4
		public override string ToString()
		{
			return "Conductor:\n" + string.Format("Current Section Type: {0}\n", this.CurrentSectionType);
		}

		// Token: 0x0600CD0F RID: 52495 RVA: 0x004ABC08 File Offset: 0x004A9E08
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnClientDisconnected(ClientInfo _ci)
		{
			if (this.PlayerEligibleForBloodmoonCache != null)
			{
				Dictionary<int, bool> playerEligibleForBloodmoonCache = this.PlayerEligibleForBloodmoonCache;
				lock (playerEligibleForBloodmoonCache)
				{
					if (!this.PlayerEligibleForBloodmoonCache.Remove(_ci.entityId))
					{
						Log.Warning(string.Format("DynamicMusic: {0} was not in Bloodmoon state cache on disconnect", _ci.entityId));
					}
					else
					{
						Log.Out(string.Format("DynamicMusic: {0} successfully removed from Bloodmoon state cache", _ci.entityId));
					}
				}
			}
		}

		// Token: 0x04009BA4 RID: 39844
		[PublicizedFrom(EAccessModifier.Private)]
		public ISectionSelector sectionSelector;

		// Token: 0x04009BA5 RID: 39845
		[PublicizedFrom(EAccessModifier.Private)]
		public EnumDictionary<SectionType, ISection> sections;

		// Token: 0x04009BA7 RID: 39847
		[PublicizedFrom(EAccessModifier.Private)]
		public ISection CurrentSection;

		// Token: 0x04009BA8 RID: 39848
		[PublicizedFrom(EAccessModifier.Private)]
		public World world;

		// Token: 0x04009BA9 RID: 39849
		public Dictionary<int, bool> PlayerEligibleForBloodmoonCache;

		// Token: 0x04009BAA RID: 39850
		public bool IsBloodmoonMusicEligible;

		// Token: 0x04009BAB RID: 39851
		[PublicizedFrom(EAccessModifier.Private)]
		public bool wasMusicPlaying;
	}
}
