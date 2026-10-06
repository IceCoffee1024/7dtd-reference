using System;
using DynamicMusic;
using UnityEngine;
using UnityEngine.Audio;

// Token: 0x020000B3 RID: 179
public class AudioMixerManager : MonoBehaviour
{
	// Token: 0x06000372 RID: 882 RVA: 0x00018E60 File Offset: 0x00017060
	public void Update()
	{
		if (GameManager.Instance == null)
		{
			return;
		}
		if (GameManager.Instance.World != null)
		{
			EntityPlayerLocal primaryPlayer = GameManager.Instance.World.GetPrimaryPlayer();
			if (primaryPlayer != null)
			{
				bool isUnderwaterCamera = primaryPlayer.IsUnderwaterCamera;
				if (primaryPlayer.isDeafened)
				{
					if (!this.wasDeafened)
					{
						this.transitionTo(this.deafenedSnapshot);
					}
				}
				else if (primaryPlayer.isStunned)
				{
					if (!this.wasStunned || this.wasDeafened)
					{
						this.transitionTo(this.stunnedSnapshot);
					}
				}
				else if (isUnderwaterCamera)
				{
					if (!this.bCameraWasUnderWater || this.wasStunned || this.wasDeafened)
					{
						this.transitionTo(this.underwaterSnapshot);
					}
				}
				else if (this.wasStunned || this.wasDeafened || this.bCameraWasUnderWater)
				{
					this.transitionTo(this.defaultSnapshot);
				}
				this.bCameraWasUnderWater = isUnderwaterCamera;
				this.wasStunned = primaryPlayer.isStunned;
				this.wasDeafened = primaryPlayer.isDeafened;
			}
		}
	}

	// Token: 0x06000373 RID: 883 RVA: 0x00018F60 File Offset: 0x00017160
	[PublicizedFrom(EAccessModifier.Private)]
	public void transitionTo(AudioMixerManager.SnapshotController _snapshot)
	{
		_snapshot.snapshot.TransitionTo(_snapshot.transitionToTime);
		if (GamePrefs.GetBool(EnumGamePrefs.OptionsDynamicMusicEnabled) && !GameManager.Instance.IsEditMode())
		{
			MixerController.Instance.OnSnapshotTransition();
		}
	}

	// Token: 0x040003D1 RID: 977
	public AudioMixerManager.SnapshotController underwaterSnapshot;

	// Token: 0x040003D2 RID: 978
	public AudioMixerManager.SnapshotController stunnedSnapshot;

	// Token: 0x040003D3 RID: 979
	public AudioMixerManager.SnapshotController deafenedSnapshot;

	// Token: 0x040003D4 RID: 980
	public AudioMixerManager.SnapshotController defaultSnapshot;

	// Token: 0x040003D5 RID: 981
	public bool bCameraWasUnderWater;

	// Token: 0x040003D6 RID: 982
	public bool wasStunned;

	// Token: 0x040003D7 RID: 983
	public bool wasDeafened;

	// Token: 0x020000B4 RID: 180
	[Serializable]
	public class SnapshotController
	{
		// Token: 0x040003D8 RID: 984
		public AudioMixerSnapshot snapshot;

		// Token: 0x040003D9 RID: 985
		public float transitionToTime = 1f;
	}
}
