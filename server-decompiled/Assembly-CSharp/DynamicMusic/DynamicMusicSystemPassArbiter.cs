using System;
using UnityEngine.Scripting;

namespace DynamicMusic
{
	// Token: 0x02001AE6 RID: 6886
	[Preserve]
	public struct DynamicMusicSystemPassArbiter : IPassArbiter, IGamePrefsChangedListener, IPauseable
	{
		// Token: 0x17001A16 RID: 6678
		// (get) Token: 0x0600CF74 RID: 53108 RVA: 0x004B2BC3 File Offset: 0x004B0DC3
		public bool WillAllowPass
		{
			get
			{
				return this.BoolContainer.Equals(224);
			}
		}

		// Token: 0x17001A17 RID: 6679
		// (set) Token: 0x0600CF75 RID: 53109 RVA: 0x004B2BD5 File Offset: 0x004B0DD5
		public bool DoesPlayerExist
		{
			set
			{
				this.SetBoolContainer(value, 64);
			}
		}

		// Token: 0x17001A18 RID: 6680
		// (set) Token: 0x0600CF76 RID: 53110 RVA: 0x004B2BE0 File Offset: 0x004B0DE0
		public bool IsGameUnPaused
		{
			[PublicizedFrom(EAccessModifier.Private)]
			set
			{
				this.SetBoolContainer(value, 32);
			}
		}

		// Token: 0x17001A19 RID: 6681
		// (set) Token: 0x0600CF77 RID: 53111 RVA: 0x004B2BEB File Offset: 0x004B0DEB
		public bool IsDynamicMusicEnabled
		{
			set
			{
				this.SetBoolContainer(value, 128);
			}
		}

		// Token: 0x0600CF78 RID: 53112 RVA: 0x004B2BF9 File Offset: 0x004B0DF9
		public DynamicMusicSystemPassArbiter(bool _enabled)
		{
			this.BoolContainer = 0;
			this.IsDynamicMusicEnabled = _enabled;
			GamePrefs.AddChangeListener(this);
		}

		// Token: 0x0600CF79 RID: 53113 RVA: 0x004B2C19 File Offset: 0x004B0E19
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

		// Token: 0x0600CF7A RID: 53114 RVA: 0x004B2C3F File Offset: 0x004B0E3F
		public void OnGamePrefChanged(EnumGamePrefs _enum)
		{
			if (_enum == EnumGamePrefs.OptionsDynamicMusicEnabled)
			{
				this.IsDynamicMusicEnabled = GamePrefs.GetBool(_enum);
			}
		}

		// Token: 0x0600CF7B RID: 53115 RVA: 0x004B2C55 File Offset: 0x004B0E55
		public void OnPause()
		{
			this.IsGameUnPaused = false;
		}

		// Token: 0x0600CF7C RID: 53116 RVA: 0x004B2C5E File Offset: 0x004B0E5E
		public void OnUnPause()
		{
			this.IsGameUnPaused = true;
		}

		// Token: 0x04009D25 RID: 40229
		[PublicizedFrom(EAccessModifier.Private)]
		public byte BoolContainer;
	}
}
