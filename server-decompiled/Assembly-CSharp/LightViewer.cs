using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001270 RID: 4720
public class LightViewer : MonoBehaviour
{
	// Token: 0x060095F6 RID: 38390 RVA: 0x0038A692 File Offset: 0x00388892
	public static void SetEnabled(bool _on)
	{
		LightViewer.IsEnabled = _on;
		LightLOD.DebugViewDistance = (_on ? float.MaxValue : 0f);
	}

	// Token: 0x060095F7 RID: 38391 RVA: 0x0038A6AE File Offset: 0x003888AE
	public void OnDestroy()
	{
		this.Disable();
	}

	// Token: 0x060095F8 RID: 38392 RVA: 0x0038A6B6 File Offset: 0x003888B6
	public void SetUpdateFrequency(float _updateFrequency)
	{
		this.updateFrequency = _updateFrequency;
	}

	// Token: 0x060095F9 RID: 38393 RVA: 0x0038A6C0 File Offset: 0x003888C0
	public void Disable()
	{
		for (int i = 0; i < this.spheres.Count; i++)
		{
			global::UnityEngine.Object.DestroyImmediate(this.spheres[i]);
		}
		this.spheres.Clear();
		this.TurnOnAllLights();
		this.allLights = null;
	}

	// Token: 0x060095FA RID: 38394 RVA: 0x0038A70C File Offset: 0x0038890C
	public void TurnOnAllLights()
	{
		foreach (KeyValuePair<Light, bool> keyValuePair in this.lightsOn)
		{
			if (keyValuePair.Key != null)
			{
				keyValuePair.Key.enabled = keyValuePair.Value;
			}
		}
		this.lightsOn.Clear();
	}

	// Token: 0x060095FB RID: 38395 RVA: 0x0038A788 File Offset: 0x00388988
	public void TurnOffAllLights()
	{
		if (this.allLights == null)
		{
			this.allLights = global::UnityEngine.Object.FindObjectsByType<Light>(FindObjectsSortMode.None);
		}
		this.lightsOn.Clear();
		for (int i = 0; i < this.allLights.Length; i++)
		{
			if (this.allLights[i].type != LightType.Directional)
			{
				this.lightsOn.Add(this.allLights[i], this.allLights[i].enabled);
				this.allLights[i].enabled = false;
			}
		}
	}

	// Token: 0x060095FC RID: 38396 RVA: 0x0038A808 File Offset: 0x00388A08
	public void Update()
	{
		if (this.lightsOn.Count > 0)
		{
			return;
		}
		if (!LightViewer.IsEnabled)
		{
			this.Disable();
			return;
		}
		if (this.sourceSphereClear == null)
		{
			this.sourceSphereClear = Resources.Load<GameObject>("Prefabs/LightViewerClear");
		}
		if (this.sourceSphereInc == null)
		{
			this.sourceSphereInc = Resources.Load<GameObject>("Prefabs/LightViewerInc");
		}
		if (this.sourceSphereInvInc == null)
		{
			this.sourceSphereInvInc = Resources.Load<GameObject>("Prefabs/LightViewerInvInc");
		}
		if (this.sourceSphereColor == null)
		{
			this.sourceSphereColor = Resources.Load<GameObject>("Prefabs/LightViewerColor");
		}
		if (this.sourceSphereInvColor == null)
		{
			this.sourceSphereInvColor = Resources.Load<GameObject>("Prefabs/LightViewerInvColor");
		}
		if (this.sourceSphereNoShadowColor == null)
		{
			this.sourceSphereNoShadowColor = Resources.Load<GameObject>("Prefabs/LightViewerNoShadowColor");
		}
		if (this.sourceSphereNoShadowInvColor == null)
		{
			this.sourceSphereNoShadowInvColor = Resources.Load<GameObject>("Prefabs/LightViewerNoShadowInvColor");
		}
		if ((this.prevCameraPos - Camera.main.transform.position).magnitude > 1f || Time.realtimeSinceStartup >= this.timeLastGathered + this.updateFrequency)
		{
			this.allLights = global::UnityEngine.Object.FindObjectsByType<Light>(FindObjectsSortMode.None);
			for (int i = 0; i < this.spheres.Count; i++)
			{
				global::UnityEngine.Object.DestroyImmediate(this.spheres[i]);
			}
			this.spheres.Clear();
			if (this.noShadowList == null)
			{
				this.noShadowList = new List<Light>();
			}
			if (this.shadowList == null)
			{
				this.shadowList = new List<Light>();
			}
			for (int j = 0; j < this.allLights.Length; j++)
			{
				if (this.allLights[j].type != LightType.Directional)
				{
					if (this.allLights[j].shadows == LightShadows.None)
					{
						this.noShadowList.Add(this.allLights[j]);
					}
					else
					{
						this.shadowList.Add(this.allLights[j]);
					}
				}
			}
			for (int k = 0; k < this.noShadowList.Count; k++)
			{
				if (this.noShadowList[k].type != LightType.Directional)
				{
					GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(this.sourceSphereClear);
					gameObject.transform.position = this.noShadowList[k].transform.position;
					gameObject.transform.localScale = Vector3.one * this.noShadowList[k].range * 2f;
					gameObject.transform.parent = GameManager.Instance.gameObject.transform;
					gameObject.GetComponent<Renderer>().sortingOrder = k;
					this.spheres.Add(gameObject);
					if (this.noShadowList[k].enabled)
					{
						bool flag = (Camera.main.transform.position - this.noShadowList[k].transform.position).magnitude < this.noShadowList[k].range;
						gameObject = global::UnityEngine.Object.Instantiate<GameObject>(flag ? this.sourceSphereInvInc : this.sourceSphereInc);
						gameObject.transform.position = this.noShadowList[k].transform.position;
						gameObject.transform.localScale = Vector3.one * this.noShadowList[k].range * 2f;
						gameObject.transform.parent = GameManager.Instance.gameObject.transform;
						gameObject.GetComponent<Renderer>().sortingOrder = k + this.noShadowList.Count;
						this.spheres.Add(gameObject);
						gameObject = global::UnityEngine.Object.Instantiate<GameObject>(flag ? this.sourceSphereNoShadowInvColor : this.sourceSphereNoShadowColor);
						gameObject.transform.position = this.noShadowList[k].transform.position;
						gameObject.transform.localScale = Vector3.one * this.noShadowList[k].range * 2f;
						gameObject.transform.parent = GameManager.Instance.gameObject.transform;
						gameObject.GetComponent<Renderer>().sortingOrder = k + this.noShadowList.Count * 2;
						this.spheres.Add(gameObject);
					}
				}
			}
			for (int l = 0; l < this.shadowList.Count; l++)
			{
				if (this.shadowList[l].type != LightType.Directional)
				{
					GameObject gameObject2 = global::UnityEngine.Object.Instantiate<GameObject>(this.sourceSphereClear);
					gameObject2.transform.position = this.shadowList[l].transform.position;
					gameObject2.transform.localScale = Vector3.one * this.shadowList[l].range * 2f;
					gameObject2.transform.parent = GameManager.Instance.gameObject.transform;
					gameObject2.GetComponent<Renderer>().sortingOrder = l + this.noShadowList.Count * 3;
					this.spheres.Add(gameObject2);
					if (this.shadowList[l].enabled)
					{
						bool flag2 = (Camera.main.transform.position - this.shadowList[l].transform.position).magnitude < this.shadowList[l].range;
						gameObject2 = global::UnityEngine.Object.Instantiate<GameObject>(flag2 ? this.sourceSphereInvInc : this.sourceSphereInc);
						gameObject2.transform.position = this.shadowList[l].transform.position;
						gameObject2.transform.localScale = Vector3.one * this.shadowList[l].range * 2f;
						gameObject2.transform.parent = GameManager.Instance.gameObject.transform;
						gameObject2.GetComponent<Renderer>().sortingOrder = l + this.shadowList.Count + this.noShadowList.Count * 3;
						this.spheres.Add(gameObject2);
						gameObject2 = global::UnityEngine.Object.Instantiate<GameObject>(flag2 ? this.sourceSphereInvColor : this.sourceSphereColor);
						gameObject2.transform.position = this.shadowList[l].transform.position;
						gameObject2.transform.localScale = Vector3.one * this.shadowList[l].range * 2f;
						gameObject2.transform.parent = GameManager.Instance.gameObject.transform;
						gameObject2.GetComponent<Renderer>().sortingOrder = l + this.shadowList.Count * 2 + this.noShadowList.Count * 3;
						this.spheres.Add(gameObject2);
					}
				}
			}
			this.noShadowList.Clear();
			this.shadowList.Clear();
			this.prevCameraPos = Camera.main.transform.position;
			this.timeLastGathered = Time.realtimeSinceStartup;
		}
	}

	// Token: 0x0400708B RID: 28811
	public static bool IsEnabled;

	// Token: 0x0400708C RID: 28812
	public static bool IsAllOff;

	// Token: 0x0400708D RID: 28813
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Light[] allLights;

	// Token: 0x0400708E RID: 28814
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Dictionary<Light, bool> lightsOn = new Dictionary<Light, bool>();

	// Token: 0x0400708F RID: 28815
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<GameObject> spheres = new List<GameObject>();

	// Token: 0x04007090 RID: 28816
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public GameObject sourceSphereClear;

	// Token: 0x04007091 RID: 28817
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public GameObject sourceSphereInc;

	// Token: 0x04007092 RID: 28818
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public GameObject sourceSphereInvInc;

	// Token: 0x04007093 RID: 28819
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public GameObject sourceSphereColor;

	// Token: 0x04007094 RID: 28820
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public GameObject sourceSphereInvColor;

	// Token: 0x04007095 RID: 28821
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public GameObject sourceSphereNoShadowColor;

	// Token: 0x04007096 RID: 28822
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public GameObject sourceSphereNoShadowInvColor;

	// Token: 0x04007097 RID: 28823
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float timeLastGathered;

	// Token: 0x04007098 RID: 28824
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float updateFrequency = 1f;

	// Token: 0x04007099 RID: 28825
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 prevCameraPos;

	// Token: 0x0400709A RID: 28826
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<Light> noShadowList;

	// Token: 0x0400709B RID: 28827
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<Light> shadowList;
}
