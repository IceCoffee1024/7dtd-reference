using System;
using UnityEngine;

// Token: 0x02001282 RID: 4738
[AddComponentMenu("NGUI/Examples/Follow Target")]
public class NGuiUIFollowTarget : MonoBehaviour
{
	// Token: 0x0600963B RID: 38459 RVA: 0x003881A6 File Offset: 0x003863A6
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Awake()
	{
		this.mTrans = base.transform;
	}

	// Token: 0x0600963C RID: 38460 RVA: 0x003881B4 File Offset: 0x003863B4
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

	// Token: 0x0600963D RID: 38461 RVA: 0x00388240 File Offset: 0x00386440
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

	// Token: 0x0600963E RID: 38462 RVA: 0x00388284 File Offset: 0x00386484
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

	// Token: 0x0600963F RID: 38463 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnUpdate(bool isVisible)
	{
	}

	// Token: 0x0400705E RID: 28766
	public Transform target;

	// Token: 0x0400705F RID: 28767
	public Camera gameCamera;

	// Token: 0x04007060 RID: 28768
	public Camera uiCamera;

	// Token: 0x04007061 RID: 28769
	public bool disableIfInvisible = true;

	// Token: 0x04007062 RID: 28770
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform mTrans;

	// Token: 0x04007063 RID: 28771
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool mIsVisible;

	// Token: 0x04007064 RID: 28772
	public Vector3 offset = Vector3.zero;
}
