using System;

namespace PrefabVolumes
{
	// Token: 0x02001919 RID: 6425
	public abstract class PrefabVolumeAbs<T> : PrefabVolumeAbs where T : PrefabVolumeAbs<T>, new()
	{
		// Token: 0x0600C5DE RID: 50654 RVA: 0x0048B5B9 File Offset: 0x004897B9
		public override PrefabVolumeAbs Clone()
		{
			return this.CloneGeneric();
		}

		// Token: 0x0600C5DF RID: 50655 RVA: 0x0048B5C8 File Offset: 0x004897C8
		public virtual T CloneGeneric()
		{
			T t = new T();
			this.CopyValues(t, false);
			return t;
		}

		// Token: 0x0600C5E0 RID: 50656 RVA: 0x0048B5EC File Offset: 0x004897EC
		public override void CopyValues(PrefabVolumeAbs _target, bool _nonBasicOnly = false)
		{
			T t = _target as T;
			if (t == null)
			{
				throw new ArgumentException("Can not copy values to volume of type " + _target.VolumeType.ToStringCached<PrefabVolumeAbs.EVolumeType>() + " from volume of type " + this.VolumeType.ToStringCached<PrefabVolumeAbs.EVolumeType>(), "_target");
			}
			this.CopyValues(t, _nonBasicOnly);
		}

		// Token: 0x0600C5E1 RID: 50657 RVA: 0x0048B64A File Offset: 0x0048984A
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual void CopyValues(PrefabVolumeAbs<T> _target, bool _nonBasicOnly = false)
		{
			_target.used = base.Used;
			_target.startPos = this.startPos;
			_target.size = this.size;
		}

		// Token: 0x0600C5E2 RID: 50658 RVA: 0x0048B670 File Offset: 0x00489870
		[PublicizedFrom(EAccessModifier.Protected)]
		public PrefabVolumeAbs()
		{
		}
	}
}
