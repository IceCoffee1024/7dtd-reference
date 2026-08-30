using System;
using UnityEngine.Scripting;

namespace DynamicMusic
{
	// Token: 0x02001A89 RID: 6793
	[Preserve]
	public struct DynamicMusicSystemPassArbiter : IPassArbiter, IGamePrefsChangedListener, IPauseable
	{
		// Token: 0x1700192C RID: 6444
		// (get) Token: 0x0600CD08 RID: 52488 RVA: 0x004AABFB File Offset: 0x004A8DFB
		public bool WillAllowPass
		{
			get
			{
				return this.BoolContainer.Equals(224);
			}
		}

		// Token: 0x1700192D RID: 6445
		// (set) Token: 0x0600CD09 RID: 52489 RVA: 0x004AAC0D File Offset: 0x004A8E0D
		public bool DoesPlayerExist
		{
			set
			{
				this.SetBoolContainer(value, 64);
			}
		}

		// Token: 0x1700192E RID: 6446
		// (set) Token: 0x0600CD0A RID: 52490 RVA: 0x004AAC18 File Offset: 0x004A8E18
		public bool IsGameUnPaused
		{
			[PublicizedFrom(EAccessModifier.Private)]
			set
			{
				this.SetBoolContainer(value, 32);
			}
		}

		// Token: 0x1700192F RID: 6447
		// (set) Token: 0x0600CD0B RID: 52491 RVA: 0x004AAC23 File Offset: 0x004A8E23
		public bool IsDynamicMusicEnabled
		{
			set
			{
				this.SetBoolContainer(value, 128);
			}
		}

		// Token: 0x0600CD0C RID: 52492 RVA: 0x004AAC31 File Offset: 0x004A8E31
		public DynamicMusicSystemPassArbiter(bool _enabled)
		{
			this.BoolContainer = 0;
			this.IsDynamicMusicEnabled = _enabled;
			GamePrefs.AddChangeListener(this);
		}

		// Token: 0x0600CD0D RID: 52493 RVA: 0x004AAC51 File Offset: 0x004A8E51
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

		// Token: 0x0600CD0E RID: 52494 RVA: 0x004AAC77 File Offset: 0x004A8E77
		public void OnGamePrefChanged(EnumGamePrefs _enum)
		{
			if (_enum == EnumGamePrefs.OptionsDynamicMusicEnabled)
			{
				this.IsDynamicMusicEnabled = GamePrefs.GetBool(_enum);
			}
		}

		// Token: 0x0600CD0F RID: 52495 RVA: 0x004AAC8D File Offset: 0x004A8E8D
		public void OnPause()
		{
			this.IsGameUnPaused = false;
		}

		// Token: 0x0600CD10 RID: 52496 RVA: 0x004AAC96 File Offset: 0x004A8E96
		public void OnUnPause()
		{
			this.IsGameUnPaused = true;
		}

		// Token: 0x04009B76 RID: 39798
		[PublicizedFrom(EAccessModifier.Private)]
		public byte BoolContainer;
	}
}
