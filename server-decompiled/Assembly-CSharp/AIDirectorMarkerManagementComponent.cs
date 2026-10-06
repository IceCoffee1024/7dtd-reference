using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000418 RID: 1048
[Preserve]
public class AIDirectorMarkerManagementComponent : AIDirectorComponent
{
	// Token: 0x06002048 RID: 8264 RVA: 0x000C3D3C File Offset: 0x000C1F3C
	public override void Tick(double _dt)
	{
		base.Tick(_dt);
		this.TickMarkers(_dt);
	}

	// Token: 0x06002049 RID: 8265 RVA: 0x000C3D4C File Offset: 0x000C1F4C
	[PublicizedFrom(EAccessModifier.Private)]
	public void TickMarkers(double _dt)
	{
		for (int i = this.markers.Count - 1; i >= 0; i--)
		{
			IAIDirectorMarker iaidirectorMarker = this.markers[i];
			iaidirectorMarker.Tick(_dt);
			if (iaidirectorMarker.TimeToLive <= 0f || (iaidirectorMarker.Player != null && iaidirectorMarker.Player.IsDead()))
			{
				this.markers.RemoveAt(i);
				iaidirectorMarker.Release();
			}
		}
	}

	// Token: 0x0600204A RID: 8266 RVA: 0x000C3DC0 File Offset: 0x000C1FC0
	public IAIDirectorMarker FindBestMarker(Vector3 _pos, ref double _inOutIntensity)
	{
		IAIDirectorMarker iaidirectorMarker = null;
		int num = -1;
		for (int i = this.markers.Count - 1; i >= 0; i--)
		{
			IAIDirectorMarker iaidirectorMarker2 = this.markers[i];
			if (iaidirectorMarker2.TimeToLive > 0f)
			{
				double num2 = iaidirectorMarker2.IntensityForPosition(_pos);
				if (num2 > 0.0 && iaidirectorMarker2.Priority > num)
				{
					num = iaidirectorMarker2.Priority;
					iaidirectorMarker = iaidirectorMarker2;
					_inOutIntensity = num2;
				}
			}
		}
		return iaidirectorMarker;
	}

	// Token: 0x0400161A RID: 5658
	[PublicizedFrom(EAccessModifier.Private)]
	public List<IAIDirectorMarker> markers = new List<IAIDirectorMarker>(256);
}
