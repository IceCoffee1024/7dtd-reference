using System;
using System.Collections;
using System.Collections.Generic;
using DynamicMusic.Factories;
using MusicUtils.Enums;
using UniLinq;
using UnityEngine.Scripting;

namespace DynamicMusic
{
	// Token: 0x02001A94 RID: 6804
	[Preserve]
	public class Conductor : IUpdatable, ICleanable
	{
		// Token: 0x170019CE RID: 6606
		// (get) Token: 0x0600CE17 RID: 52759 RVA: 0x004AEFFD File Offset: 0x004AD1FD
		// (set) Token: 0x0600CE18 RID: 52760 RVA: 0x004AF005 File Offset: 0x004AD205
		public SectionType CurrentSectionType
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x0600CE19 RID: 52761 RVA: 0x004AF00E File Offset: 0x004AD20E
		public Conductor()
		{
			if (GameManager.IsDedicatedServer)
			{
				return;
			}
			MixerController.Instance.Init();
		}

		// Token: 0x0600CE1A RID: 52762 RVA: 0x004AF028 File Offset: 0x004AD228
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

		// Token: 0x170019CF RID: 6607
		// (get) Token: 0x0600CE1B RID: 52763 RVA: 0x004AF154 File Offset: 0x004AD354
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

		// Token: 0x0600CE1C RID: 52764 RVA: 0x004AF1BC File Offset: 0x004AD3BC
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

		// Token: 0x0600CE1D RID: 52765 RVA: 0x004AF1CC File Offset: 0x004AD3CC
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

		// Token: 0x0600CE1E RID: 52766 RVA: 0x004AF580 File Offset: 0x004AD780
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

		// Token: 0x0600CE1F RID: 52767 RVA: 0x004AF5B1 File Offset: 0x004AD7B1
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

		// Token: 0x0600CE20 RID: 52768 RVA: 0x004AF5E4 File Offset: 0x004AD7E4
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

		// Token: 0x0600CE21 RID: 52769 RVA: 0x004AF678 File Offset: 0x004AD878
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

		// Token: 0x0600CE22 RID: 52770 RVA: 0x004AF6E0 File Offset: 0x004AD8E0
		public override string ToString()
		{
			return "Conductor:\n" + string.Format("Current Section Type: {0}\n", this.CurrentSectionType);
		}

		// Token: 0x0600CE23 RID: 52771 RVA: 0x004AF704 File Offset: 0x004AD904
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

		// Token: 0x04009C8D RID: 40077
		[PublicizedFrom(EAccessModifier.Private)]
		public ISectionSelector sectionSelector;

		// Token: 0x04009C8E RID: 40078
		[PublicizedFrom(EAccessModifier.Private)]
		public EnumDictionary<SectionType, ISection> sections;

		// Token: 0x04009C90 RID: 40080
		[PublicizedFrom(EAccessModifier.Private)]
		public ISection CurrentSection;

		// Token: 0x04009C91 RID: 40081
		[PublicizedFrom(EAccessModifier.Private)]
		public World world;

		// Token: 0x04009C92 RID: 40082
		public Dictionary<int, bool> PlayerEligibleForBloodmoonCache;

		// Token: 0x04009C93 RID: 40083
		public bool IsBloodmoonMusicEligible;

		// Token: 0x04009C94 RID: 40084
		[PublicizedFrom(EAccessModifier.Private)]
		public bool wasMusicPlaying;
	}
}
