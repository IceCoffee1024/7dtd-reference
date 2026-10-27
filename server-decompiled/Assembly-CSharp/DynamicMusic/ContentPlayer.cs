using System;

namespace DynamicMusic
{
	// Token: 0x02001A85 RID: 6789
	public abstract class ContentPlayer : IPlayable
	{
		// Token: 0x170019B6 RID: 6582
		// (get) Token: 0x0600CDA4 RID: 52644 RVA: 0x004ADDA7 File Offset: 0x004ABFA7
		// (set) Token: 0x0600CDA5 RID: 52645 RVA: 0x004ADDAF File Offset: 0x004ABFAF
		public virtual float Volume { get; set; } = 1f;

		// Token: 0x170019B7 RID: 6583
		// (get) Token: 0x0600CDA6 RID: 52646 RVA: 0x004ADDB8 File Offset: 0x004ABFB8
		// (set) Token: 0x0600CDA7 RID: 52647 RVA: 0x004ADDC0 File Offset: 0x004ABFC0
		public virtual bool IsDone
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x170019B8 RID: 6584
		// (get) Token: 0x0600CDA8 RID: 52648 RVA: 0x004ADDC9 File Offset: 0x004ABFC9
		// (set) Token: 0x0600CDA9 RID: 52649 RVA: 0x004ADDD1 File Offset: 0x004ABFD1
		public virtual bool IsPaused
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x170019B9 RID: 6585
		// (get) Token: 0x0600CDAA RID: 52650 RVA: 0x004ADDDA File Offset: 0x004ABFDA
		// (set) Token: 0x0600CDAB RID: 52651 RVA: 0x004ADDE2 File Offset: 0x004ABFE2
		public virtual bool IsPlaying
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x170019BA RID: 6586
		// (get) Token: 0x0600CDAC RID: 52652 RVA: 0x004ADDEB File Offset: 0x004ABFEB
		// (set) Token: 0x0600CDAD RID: 52653 RVA: 0x004ADDF3 File Offset: 0x004ABFF3
		public virtual bool IsReady
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		}

		// Token: 0x0600CDAE RID: 52654
		public abstract void Init();

		// Token: 0x0600CDAF RID: 52655 RVA: 0x004ADDFC File Offset: 0x004ABFFC
		public virtual void Play()
		{
			this.IsDone = false;
			this.IsPlaying = true;
			this.IsPaused = false;
		}

		// Token: 0x0600CDB0 RID: 52656 RVA: 0x004ADE13 File Offset: 0x004AC013
		public virtual void Pause()
		{
			this.IsPlaying = false;
			this.IsPaused = true;
		}

		// Token: 0x0600CDB1 RID: 52657 RVA: 0x004ADE23 File Offset: 0x004AC023
		public virtual void UnPause()
		{
			this.IsPlaying = true;
			this.IsPaused = false;
		}

		// Token: 0x0600CDB2 RID: 52658 RVA: 0x004ADE33 File Offset: 0x004AC033
		public virtual void Stop()
		{
			this.IsDone = true;
			this.IsPlaying = false;
			this.IsPaused = false;
		}

		// Token: 0x0600CDB3 RID: 52659 RVA: 0x004ADE4A File Offset: 0x004AC04A
		[PublicizedFrom(EAccessModifier.Protected)]
		public ContentPlayer()
		{
		}
	}
}
