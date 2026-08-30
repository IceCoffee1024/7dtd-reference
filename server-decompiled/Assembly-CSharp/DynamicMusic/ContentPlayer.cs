using System;

namespace DynamicMusic
{
	// Token: 0x02001A28 RID: 6696
	public abstract class ContentPlayer : IPlayable
	{
		// Token: 0x170018CC RID: 6348
		// (get) Token: 0x0600CB38 RID: 52024 RVA: 0x004A5DDF File Offset: 0x004A3FDF
		// (set) Token: 0x0600CB39 RID: 52025 RVA: 0x004A5DE7 File Offset: 0x004A3FE7
		public virtual float Volume { get; set; } = 1f;

		// Token: 0x170018CD RID: 6349
		// (get) Token: 0x0600CB3A RID: 52026 RVA: 0x004A5DF0 File Offset: 0x004A3FF0
		// (set) Token: 0x0600CB3B RID: 52027 RVA: 0x004A5DF8 File Offset: 0x004A3FF8
		public virtual bool IsDone
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x170018CE RID: 6350
		// (get) Token: 0x0600CB3C RID: 52028 RVA: 0x004A5E01 File Offset: 0x004A4001
		// (set) Token: 0x0600CB3D RID: 52029 RVA: 0x004A5E09 File Offset: 0x004A4009
		public virtual bool IsPaused
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x170018CF RID: 6351
		// (get) Token: 0x0600CB3E RID: 52030 RVA: 0x004A5E12 File Offset: 0x004A4012
		// (set) Token: 0x0600CB3F RID: 52031 RVA: 0x004A5E1A File Offset: 0x004A401A
		public virtual bool IsPlaying
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x170018D0 RID: 6352
		// (get) Token: 0x0600CB40 RID: 52032 RVA: 0x004A5E23 File Offset: 0x004A4023
		// (set) Token: 0x0600CB41 RID: 52033 RVA: 0x004A5E2B File Offset: 0x004A402B
		public virtual bool IsReady
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x0600CB42 RID: 52034
		public abstract void Init();

		// Token: 0x0600CB43 RID: 52035 RVA: 0x004A5E34 File Offset: 0x004A4034
		public virtual void Play()
		{
			this.IsDone = false;
			this.IsPlaying = true;
			this.IsPaused = false;
		}

		// Token: 0x0600CB44 RID: 52036 RVA: 0x004A5E4B File Offset: 0x004A404B
		public virtual void Pause()
		{
			this.IsPlaying = false;
			this.IsPaused = true;
		}

		// Token: 0x0600CB45 RID: 52037 RVA: 0x004A5E5B File Offset: 0x004A405B
		public virtual void UnPause()
		{
			this.IsPlaying = true;
			this.IsPaused = false;
		}

		// Token: 0x0600CB46 RID: 52038 RVA: 0x004A5E6B File Offset: 0x004A406B
		public virtual void Stop()
		{
			this.IsDone = true;
			this.IsPlaying = false;
			this.IsPaused = false;
		}

		// Token: 0x0600CB47 RID: 52039 RVA: 0x004A5E82 File Offset: 0x004A4082
		[PublicizedFrom(EAccessModifier.Protected)]
		public ContentPlayer()
		{
		}
	}
}
