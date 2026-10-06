using System;

namespace PrefabVolumes
{
	// Token: 0x020018E8 RID: 6376
	public abstract class PrefabVolumeAbs<T> : PrefabVolumeAbs where T : PrefabVolumeAbs<T>, new()
	{
		// Token: 0x0600C4CA RID: 50378 RVA: 0x00487C0D File Offset: 0x00485E0D
		public override PrefabVolumeAbs Clone()
		{
			return this.CloneGeneric();
		}

		// Token: 0x0600C4CB RID: 50379 RVA: 0x00487C1C File Offset: 0x00485E1C
		public virtual T CloneGeneric()
		{
			T t = new T();
			this.CopyValues(t, false);
			return t;
		}

		// Token: 0x0600C4CC RID: 50380 RVA: 0x00487C40 File Offset: 0x00485E40
		public override void CopyValues(PrefabVolumeAbs _target, bool _nonBasicOnly = false)
		{
			T t = _target as T;
			if (t == null)
			{
				throw new ArgumentException("Can not copy values to volume of type " + _target.VolumeType.ToStringCached<PrefabVolumeAbs.EVolumeType>() + " from volume of type " + this.VolumeType.ToStringCached<PrefabVolumeAbs.EVolumeType>(), "_target");
			}
			this.CopyValues(t, _nonBasicOnly);
		}

		// Token: 0x0600C4CD RID: 50381 RVA: 0x00487C9E File Offset: 0x00485E9E
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual void CopyValues(PrefabVolumeAbs<T> _target, bool _nonBasicOnly = false)
		{
			_target.used = base.Used;
			_target.startPos = this.startPos;
			_target.size = this.size;
		}

		// Token: 0x0600C4CE RID: 50382 RVA: 0x00487CC4 File Offset: 0x00485EC4
		[PublicizedFrom(EAccessModifier.Protected)]
		public PrefabVolumeAbs()
		{
		}
	}
}
