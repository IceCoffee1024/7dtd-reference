using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000412 RID: 1042
[Preserve]
public class AIDirectorMarkerManagementComponent : AIDirectorComponent
{
	// Token: 0x0600203C RID: 8252 RVA: 0x000C34EC File Offset: 0x000C16EC
	public override void Tick(double _dt)
	{
		base.Tick(_dt);
		this.TickMarkers(_dt);
	}

	// Token: 0x0600203D RID: 8253 RVA: 0x000C34FC File Offset: 0x000C16FC
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

	// Token: 0x0600203E RID: 8254 RVA: 0x000C3570 File Offset: 0x000C1770
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

	// Token: 0x0400160E RID: 5646
	[PublicizedFrom(EAccessModifier.Private)]
	public List<IAIDirectorMarker> markers = new List<IAIDirectorMarker>(256);
}
