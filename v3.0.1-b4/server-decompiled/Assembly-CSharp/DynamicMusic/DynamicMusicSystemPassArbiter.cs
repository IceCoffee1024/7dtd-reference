using System;
using UnityEngine.Scripting;

namespace DynamicMusic
{
	// Token: 0x02001A62 RID: 6754
	[Preserve]
	public struct DynamicMusicSystemPassArbiter : IPassArbiter, IGamePrefsChangedListener, IPauseable
	{
		// Token: 0x170018BA RID: 6330
		// (get) Token: 0x0600CB7F RID: 52095 RVA: 0x004A4807 File Offset: 0x004A2A07
		public bool WillAllowPass
		{
			get
			{
				return this.BoolContainer.Equals(224);
			}
		}

		// Token: 0x170018BB RID: 6331
		// (set) Token: 0x0600CB80 RID: 52096 RVA: 0x004A4819 File Offset: 0x004A2A19
		public bool DoesPlayerExist
		{
			set
			{
				this.SetBoolContainer(value, 64);
			}
		}

		// Token: 0x170018BC RID: 6332
		// (set) Token: 0x0600CB81 RID: 52097 RVA: 0x004A4824 File Offset: 0x004A2A24
		public bool IsGameUnPaused
		{
			[PublicizedFrom(EAccessModifier.Private)]
			set
			{
				this.SetBoolContainer(value, 32);
			}
		}

		// Token: 0x170018BD RID: 6333
		// (set) Token: 0x0600CB82 RID: 52098 RVA: 0x004A482F File Offset: 0x004A2A2F
		public bool IsDynamicMusicEnabled
		{
			set
			{
				this.SetBoolContainer(value, 128);
			}
		}

		// Token: 0x0600CB83 RID: 52099 RVA: 0x004A483D File Offset: 0x004A2A3D
		public DynamicMusicSystemPassArbiter(bool _enabled)
		{
			this.BoolContainer = 0;
			this.IsDynamicMusicEnabled = _enabled;
			GamePrefs.AddChangeListener(this);
		}

		// Token: 0x0600CB84 RID: 52100 RVA: 0x004A485D File Offset: 0x004A2A5D
		[PublicizedFrom(EAccessModifier.Private)]
		public void SetBoolContainer(bool _value, byte _place)
		{
			if (_value)
			{
				this.BoolContainer |= _place;
				return;
			}
			this.BoolContainer &= ~_place;
		}

		// Token: 0x0600CB85 RID: 52101 RVA: 0x004A4883 File Offset: 0x004A2A83
		public void OnGamePrefChanged(EnumGamePrefs _enum)
		{
			if (_enum == EnumGamePrefs.OptionsDynamicMusicEnabled)
			{
				this.IsDynamicMusicEnabled = GamePrefs.GetBool(_enum);
			}
		}

		// Token: 0x0600CB86 RID: 52102 RVA: 0x004A4899 File Offset: 0x004A2A99
		public void OnPause()
		{
			this.IsGameUnPaused = false;
		}

		// Token: 0x0600CB87 RID: 52103 RVA: 0x004A48A2 File Offset: 0x004A2AA2
		public void OnUnPause()
		{
			this.IsGameUnPaused = true;
		}

		// Token: 0x04009A45 RID: 39493
		[PublicizedFrom(EAccessModifier.Private)]
		public byte BoolContainer;
	}
}
