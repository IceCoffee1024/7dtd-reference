using System;

namespace DynamicMusic
{
	// Token: 0x02001A54 RID: 6740
	public abstract class ContentPlayer : IPlayable
	{
		// Token: 0x1700196E RID: 6510
		// (get) Token: 0x0600CC90 RID: 52368 RVA: 0x004AA2AB File Offset: 0x004A84AB
		// (set) Token: 0x0600CC91 RID: 52369 RVA: 0x004AA2B3 File Offset: 0x004A84B3
		public virtual float Volume { get; set; } = 1f;

		// Token: 0x1700196F RID: 6511
		// (get) Token: 0x0600CC92 RID: 52370 RVA: 0x004AA2BC File Offset: 0x004A84BC
		// (set) Token: 0x0600CC93 RID: 52371 RVA: 0x004AA2C4 File Offset: 0x004A84C4
		public virtual bool IsDone
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x17001970 RID: 6512
		// (get) Token: 0x0600CC94 RID: 52372 RVA: 0x004AA2CD File Offset: 0x004A84CD
		// (set) Token: 0x0600CC95 RID: 52373 RVA: 0x004AA2D5 File Offset: 0x004A84D5
		public virtual bool IsPaused
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x17001971 RID: 6513
		// (get) Token: 0x0600CC96 RID: 52374 RVA: 0x004AA2DE File Offset: 0x004A84DE
		// (set) Token: 0x0600CC97 RID: 52375 RVA: 0x004AA2E6 File Offset: 0x004A84E6
		public virtual bool IsPlaying
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x17001972 RID: 6514
		// (get) Token: 0x0600CC98 RID: 52376 RVA: 0x004AA2EF File Offset: 0x004A84EF
		// (set) Token: 0x0600CC99 RID: 52377 RVA: 0x004AA2F7 File Offset: 0x004A84F7
		public virtual bool IsReady
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x0600CC9A RID: 52378
		public abstract void Init();

		// Token: 0x0600CC9B RID: 52379 RVA: 0x004AA300 File Offset: 0x004A8500
		public virtual void Play()
		{
			this.IsDone = false;
			this.IsPlaying = true;
			this.IsPaused = false;
		}

		// Token: 0x0600CC9C RID: 52380 RVA: 0x004AA317 File Offset: 0x004A8517
		public virtual void Pause()
		{
			this.IsPlaying = false;
			this.IsPaused = true;
		}

		// Token: 0x0600CC9D RID: 52381 RVA: 0x004AA327 File Offset: 0x004A8527
		public virtual void UnPause()
		{
			this.IsPlaying = true;
			this.IsPaused = false;
		}

		// Token: 0x0600CC9E RID: 52382 RVA: 0x004AA337 File Offset: 0x004A8537
		public virtual void Stop()
		{
			this.IsDone = true;
			this.IsPlaying = false;
			this.IsPaused = false;
		}

		// Token: 0x0600CC9F RID: 52383 RVA: 0x004AA34E File Offset: 0x004A854E
		[PublicizedFrom(EAccessModifier.Protected)]
		public ContentPlayer()
		{
		}
	}
}
