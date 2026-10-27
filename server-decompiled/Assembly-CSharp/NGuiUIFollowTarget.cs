using System;
using UnityEngine;

// Token: 0x020012C6 RID: 4806
[AddComponentMenu("NGUI/Examples/Follow Target")]
public class NGuiUIFollowTarget : MonoBehaviour
{
	// Token: 0x0600981B RID: 38939 RVA: 0x00393F52 File Offset: 0x00392152
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Awake()
	{
		this.mTrans = base.transform;
	}

	// Token: 0x0600981C RID: 38940 RVA: 0x00393F60 File Offset: 0x00392160
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Start()
	{
		if (this.target != null)
		{
			if (this.gameCamera == null)
			{
				this.gameCamera = NGUITools.FindCameraForLayer(this.target.gameObject.layer);
			}
			if (this.uiCamera == null)
			{
				this.uiCamera = NGUITools.FindCameraForLayer(base.gameObject.layer);
			}
			this.SetVisible(false);
			return;
		}
		Log.Error("Expected to have 'target' set to a valid transform", new object[] { this });
		base.enabled = false;
	}

	// Token: 0x0600981D RID: 38941 RVA: 0x00393FEC File Offset: 0x003921EC
	[PublicizedFrom(EAccessModifier.Protected)]
	public void SetVisible(bool val)
	{
		this.mIsVisible = val;
		int i = 0;
		int childCount = this.mTrans.childCount;
		while (i < childCount)
		{
			NGUITools.SetActive(this.mTrans.GetChild(i).gameObject, val);
			i++;
		}
	}

	// Token: 0x0600981E RID: 38942 RVA: 0x00394030 File Offset: 0x00392230
	[PublicizedFrom(EAccessModifier.Protected)]
	public void LateUpdate()
	{
		if (this.target == null || this.gameCamera == null)
		{
			return;
		}
		Vector3 vector = this.gameCamera.WorldToViewportPoint(this.target.position + this.offset);
		bool flag = (this.gameCamera.orthographic || vector.z > 0f) && (!this.disableIfInvisible || (vector.x > 0f && vector.x < 1f && vector.y > 0f && vector.y < 1f));
		if (this.mIsVisible != flag)
		{
			this.SetVisible(flag);
		}
		if (flag)
		{
			base.transform.position = this.uiCamera.ViewportToWorldPoint(vector);
			vector = this.mTrans.localPosition;
			vector.x = (float)Mathf.FloorToInt(vector.x);
			vector.y = (float)Mathf.FloorToInt(vector.y);
			vector.z = 0f;
			this.mTrans.localPosition = vector;
		}
		this.OnUpdate(flag);
	}

	// Token: 0x0600981F RID: 38943 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnUpdate(bool isVisible)
	{
	}

	// Token: 0x040071DE RID: 29150
	public Transform target;

	// Token: 0x040071DF RID: 29151
	public Camera gameCamera;

	// Token: 0x040071E0 RID: 29152
	public Camera uiCamera;

	// Token: 0x040071E1 RID: 29153
	public bool disableIfInvisible = true;

	// Token: 0x040071E2 RID: 29154
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform mTrans;

	// Token: 0x040071E3 RID: 29155
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool mIsVisible;

	// Token: 0x040071E4 RID: 29156
	public Vector3 offset = Vector3.zero;
}
