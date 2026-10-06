using System;
using UnityEngine.Scripting;

namespace DynamicMusic
{
	// Token: 0x02001AB5 RID: 6837
	[Preserve]
	public struct DynamicMusicSystemPassArbiter : IPassArbiter, IGamePrefsChangedListener, IPauseable
	{
		// Token: 0x170019CE RID: 6606
		// (get) Token: 0x0600CE60 RID: 52832 RVA: 0x004AF0C7 File Offset: 0x004AD2C7
		public bool WillAllowPass
		{
			get
			{
				return this.BoolContainer.Equals(224);
			}
		}

		// Token: 0x170019CF RID: 6607
		// (set) Token: 0x0600CE61 RID: 52833 RVA: 0x004AF0D9 File Offset: 0x004AD2D9
		public bool DoesPlayerExist
		{
			set
			{
				this.SetBoolContainer(value, 64);
			}
		}

		// Token: 0x170019D0 RID: 6608
		// (set) Token: 0x0600CE62 RID: 52834 RVA: 0x004AF0E4 File Offset: 0x004AD2E4
		public bool IsGameUnPaused
		{
			[PublicizedFrom(EAccessModifier.Private)]
			set
			{
				this.SetBoolContainer(value, 32);
			}
		}

		// Token: 0x170019D1 RID: 6609
		// (set) Token: 0x0600CE63 RID: 52835 RVA: 0x004AF0EF File Offset: 0x004AD2EF
		public bool IsDynamicMusicEnabled
		{
			set
			{
				this.SetBoolContainer(value, 128);
			}
		}

		// Token: 0x0600CE64 RID: 52836 RVA: 0x004AF0FD File Offset: 0x004AD2FD
		public DynamicMusicSystemPassArbiter(bool _enabled)
		{
			this.BoolContainer = 0;
			this.IsDynamicMusicEnabled = _enabled;
			GamePrefs.AddChangeListener(this);
		}

		// Token: 0x0600CE65 RID: 52837 RVA: 0x004AF11D File Offset: 0x004AD31D
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

		// Token: 0x0600CE66 RID: 52838 RVA: 0x004AF143 File Offset: 0x004AD343
		public void OnGamePrefChanged(EnumGamePrefs _enum)
		{
			if (_enum == EnumGamePrefs.OptionsDynamicMusicEnabled)
			{
				this.IsDynamicMusicEnabled = GamePrefs.GetBool(_enum);
			}
		}

		// Token: 0x0600CE67 RID: 52839 RVA: 0x004AF159 File Offset: 0x004AD359
		public void OnPause()
		{
			this.IsGameUnPaused = false;
		}

		// Token: 0x0600CE68 RID: 52840 RVA: 0x004AF162 File Offset: 0x004AD362
		public void OnUnPause()
		{
			this.IsGameUnPaused = true;
		}

		// Token: 0x04009C3C RID: 39996
		[PublicizedFrom(EAccessModifier.Private)]
		public byte BoolContainer;
	}
}
