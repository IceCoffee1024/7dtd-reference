using System;
using UnityEngine;

// Token: 0x0200129C RID: 4764
[AddComponentMenu("NGUI/Examples/Follow Target")]
public class NGuiUIFollowTarget : MonoBehaviour
{
	// Token: 0x06009736 RID: 38710 RVA: 0x0038D302 File Offset: 0x0038B502
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Awake()
	{
		this.mTrans = base.transform;
	}

	// Token: 0x06009737 RID: 38711 RVA: 0x0038D310 File Offset: 0x0038B510
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

	// Token: 0x06009738 RID: 38712 RVA: 0x0038D39C File Offset: 0x0038B59C
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

	// Token: 0x06009739 RID: 38713 RVA: 0x0038D3E0 File Offset: 0x0038B5E0
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

	// Token: 0x0600973A RID: 38714 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnUpdate(bool isVisible)
	{
	}

	// Token: 0x04007133 RID: 28979
	public Transform target;

	// Token: 0x04007134 RID: 28980
	public Camera gameCamera;

	// Token: 0x04007135 RID: 28981
	public Camera uiCamera;

	// Token: 0x04007136 RID: 28982
	public bool disableIfInvisible = true;

	// Token: 0x04007137 RID: 28983
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform mTrans;

	// Token: 0x04007138 RID: 28984
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool mIsVisible;

	// Token: 0x04007139 RID: 28985
	public Vector3 offset = Vector3.zero;
}
