using System;

namespace DynamicMusic
{
	// Token: 0x02001A01 RID: 6657
	public abstract class ContentPlayer : IPlayable
	{
		// Token: 0x1700185A RID: 6234
		// (get) Token: 0x0600C9AF RID: 51631 RVA: 0x0049F9EB File Offset: 0x0049DBEB
		// (set) Token: 0x0600C9B0 RID: 51632 RVA: 0x0049F9F3 File Offset: 0x0049DBF3
		public virtual float Volume { get; set; } = 1f;

		// Token: 0x1700185B RID: 6235
		// (get) Token: 0x0600C9B1 RID: 51633 RVA: 0x0049F9FC File Offset: 0x0049DBFC
		// (set) Token: 0x0600C9B2 RID: 51634 RVA: 0x0049FA04 File Offset: 0x0049DC04
		public virtual bool IsDone
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x1700185C RID: 6236
		// (get) Token: 0x0600C9B3 RID: 51635 RVA: 0x0049FA0D File Offset: 0x0049DC0D
		// (set) Token: 0x0600C9B4 RID: 51636 RVA: 0x0049FA15 File Offset: 0x0049DC15
		public virtual bool IsPaused
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x1700185D RID: 6237
		// (get) Token: 0x0600C9B5 RID: 51637 RVA: 0x0049FA1E File Offset: 0x0049DC1E
		// (set) Token: 0x0600C9B6 RID: 51638 RVA: 0x0049FA26 File Offset: 0x0049DC26
		public virtual bool IsPlaying
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x1700185E RID: 6238
		// (get) Token: 0x0600C9B7 RID: 51639 RVA: 0x0049FA2F File Offset: 0x0049DC2F
		// (set) Token: 0x0600C9B8 RID: 51640 RVA: 0x0049FA37 File Offset: 0x0049DC37
		public virtual bool IsReady
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x0600C9B9 RID: 51641
		public abstract void Init();

		// Token: 0x0600C9BA RID: 51642 RVA: 0x0049FA40 File Offset: 0x0049DC40
		public virtual void Play()
		{
			this.IsDone = false;
			this.IsPlaying = true;
			this.IsPaused = false;
		}

		// Token: 0x0600C9BB RID: 51643 RVA: 0x0049FA57 File Offset: 0x0049DC57
		public virtual void Pause()
		{
			this.IsPlaying = false;
			this.IsPaused = true;
		}

		// Token: 0x0600C9BC RID: 51644 RVA: 0x0049FA67 File Offset: 0x0049DC67
		public virtual void UnPause()
		{
			this.IsPlaying = true;
			this.IsPaused = false;
		}

		// Token: 0x0600C9BD RID: 51645 RVA: 0x0049FA77 File Offset: 0x0049DC77
		public virtual void Stop()
		{
			this.IsDone = true;
			this.IsPlaying = false;
			this.IsPaused = false;
		}

		// Token: 0x0600C9BE RID: 51646 RVA: 0x0049FA8E File Offset: 0x0049DC8E
		[PublicizedFrom(EAccessModifier.Protected)]
		public ContentPlayer()
		{
		}
	}
}
