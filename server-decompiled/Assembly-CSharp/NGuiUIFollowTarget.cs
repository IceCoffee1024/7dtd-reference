using System;
using UnityEngine;

// Token: 0x020012B4 RID: 4788
[AddComponentMenu("NGUI/Examples/Follow Target")]
public class NGuiUIFollowTarget : MonoBehaviour
{
	// Token: 0x060097C3 RID: 38851 RVA: 0x00391B4A File Offset: 0x0038FD4A
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Awake()
	{
		this.mTrans = base.transform;
	}

	// Token: 0x060097C4 RID: 38852 RVA: 0x00391B58 File Offset: 0x0038FD58
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

	// Token: 0x060097C5 RID: 38853 RVA: 0x00391BE4 File Offset: 0x0038FDE4
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

	// Token: 0x060097C6 RID: 38854 RVA: 0x00391C28 File Offset: 0x0038FE28
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

	// Token: 0x060097C7 RID: 38855 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnUpdate(bool isVisible)
	{
	}

	// Token: 0x0400719D RID: 29085
	public Transform target;

	// Token: 0x0400719E RID: 29086
	public Camera gameCamera;

	// Token: 0x0400719F RID: 29087
	public Camera uiCamera;

	// Token: 0x040071A0 RID: 29088
	public bool disableIfInvisible = true;

	// Token: 0x040071A1 RID: 29089
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform mTrans;

	// Token: 0x040071A2 RID: 29090
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool mIsVisible;

	// Token: 0x040071A3 RID: 29091
	public Vector3 offset = Vector3.zero;
}
