using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020015B6 RID: 5558
public class vp_Input : MonoBehaviour
{
	// Token: 0x1700143B RID: 5179
	// (get) Token: 0x0600AD4C RID: 44364 RVA: 0x004096B8 File Offset: 0x004078B8
	public static vp_Input Instance
	{
		get
		{
			if (vp_Input.mIsDirty)
			{
				vp_Input.mIsDirty = false;
				if (vp_Input.m_Instance == null)
				{
					if (Application.isPlaying)
					{
						GameObject gameObject = Resources.Load("Input/vp_Input") as GameObject;
						if (gameObject == null)
						{
							vp_Input.m_Instance = new GameObject("vp_Input").AddComponent<vp_Input>();
						}
						else
						{
							vp_Input.m_Instance = gameObject.GetComponent<vp_Input>();
							if (vp_Input.m_Instance == null)
							{
								vp_Input.m_Instance = gameObject.AddComponent<vp_Input>();
							}
						}
					}
					vp_Input.m_Instance.SetupDefaults("");
				}
			}
			return vp_Input.m_Instance;
		}
	}

	// Token: 0x0600AD4D RID: 44365 RVA: 0x000027FC File Offset: 0x000009FC
	public static void CreateIfNoExist()
	{
	}

	// Token: 0x0600AD4E RID: 44366 RVA: 0x0040974C File Offset: 0x0040794C
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Awake()
	{
		if (vp_Input.m_Instance == null)
		{
			vp_Input.m_Instance = vp_Input.Instance;
		}
	}

	// Token: 0x0600AD4F RID: 44367 RVA: 0x00409765 File Offset: 0x00407965
	public virtual void SetDirty(bool dirty)
	{
		vp_Input.mIsDirty = dirty;
	}

	// Token: 0x0600AD50 RID: 44368 RVA: 0x00409770 File Offset: 0x00407970
	public virtual void SetupDefaults(string type = "")
	{
		if ((type == "" || type == "Buttons") && this.ButtonKeys.Count == 0)
		{
			this.AddButton("Attack", KeyCode.Mouse0);
			this.AddButton("SetNextWeapon", KeyCode.E);
			this.AddButton("SetPrevWeapon", KeyCode.Q);
			this.AddButton("ClearWeapon", KeyCode.Backspace);
			this.AddButton("Zoom", KeyCode.Mouse1);
			this.AddButton("Reload", KeyCode.R);
			this.AddButton("Jump", KeyCode.Space);
			this.AddButton("Crouch", KeyCode.C);
			this.AddButton("Run", KeyCode.LeftShift);
			this.AddButton("Interact", KeyCode.F);
			this.AddButton("Accept1", KeyCode.Return);
			this.AddButton("Accept2", KeyCode.KeypadEnter);
			this.AddButton("Pause", KeyCode.P);
			this.AddButton("Menu", KeyCode.Escape);
		}
		if ((type == "" || type == "Axis") && this.AxisKeys.Count == 0)
		{
			this.AddAxis("Vertical", KeyCode.W, KeyCode.S);
			this.AddAxis("Horizontal", KeyCode.D, KeyCode.A);
		}
		if ((type == "" || type == "UnityAxis") && this.UnityAxis.Count == 0)
		{
			this.AddUnityAxis("Mouse X");
			this.AddUnityAxis("Mouse Y");
		}
		this.UpdateDictionaries();
	}

	// Token: 0x0600AD51 RID: 44369 RVA: 0x004098F4 File Offset: 0x00407AF4
	public virtual void AddButton(string n, KeyCode k = KeyCode.None)
	{
		if (this.ButtonKeys.Contains(n))
		{
			this.ButtonValues[this.ButtonKeys.IndexOf(n)] = k;
			return;
		}
		this.ButtonKeys.Add(n);
		this.ButtonValues.Add(k);
	}

	// Token: 0x0600AD52 RID: 44370 RVA: 0x00409940 File Offset: 0x00407B40
	public virtual void AddAxis(string n, KeyCode pk = KeyCode.None, KeyCode nk = KeyCode.None)
	{
		if (this.AxisKeys.Contains(n))
		{
			this.AxisValues[this.AxisKeys.IndexOf(n)] = new vp_Input.vp_InputAxis
			{
				Positive = pk,
				Negative = nk
			};
			return;
		}
		this.AxisKeys.Add(n);
		this.AxisValues.Add(new vp_Input.vp_InputAxis
		{
			Positive = pk,
			Negative = nk
		});
	}

	// Token: 0x0600AD53 RID: 44371 RVA: 0x004099B0 File Offset: 0x00407BB0
	public virtual void AddUnityAxis(string n)
	{
		if (this.UnityAxis.Contains(n))
		{
			this.UnityAxis[this.UnityAxis.IndexOf(n)] = n;
			return;
		}
		this.UnityAxis.Add(n);
	}

	// Token: 0x0600AD54 RID: 44372 RVA: 0x004099E8 File Offset: 0x00407BE8
	public virtual void UpdateDictionaries()
	{
		if (!Application.isPlaying)
		{
			return;
		}
		this.Buttons.Clear();
		for (int i = 0; i < this.ButtonKeys.Count; i++)
		{
			this.Buttons.Add(this.ButtonKeys[i], this.ButtonValues[i]);
		}
		this.Axis.Clear();
		for (int j = 0; j < this.AxisKeys.Count; j++)
		{
			this.Axis.Add(this.AxisKeys[j], new vp_Input.vp_InputAxis
			{
				Positive = this.AxisValues[j].Positive,
				Negative = this.AxisValues[j].Negative
			});
		}
	}

	// Token: 0x0600AD55 RID: 44373 RVA: 0x00409AAC File Offset: 0x00407CAC
	public static bool GetButtonAny(string button)
	{
		return vp_Input.Instance.DoGetButtonAny(button);
	}

	// Token: 0x0600AD56 RID: 44374 RVA: 0x00409ABC File Offset: 0x00407CBC
	public virtual bool DoGetButtonAny(string button)
	{
		if (this.Buttons.ContainsKey(button))
		{
			return Input.GetKey(this.Buttons[button]) || Input.GetKeyDown(this.Buttons[button]) || Input.GetKeyUp(this.Buttons[button]);
		}
		Debug.LogError("\"" + button + "\" is not in VP Input Manager's Buttons. You must add it for this Button to work.");
		return false;
	}

	// Token: 0x0600AD57 RID: 44375 RVA: 0x00409B27 File Offset: 0x00407D27
	public static bool GetButton(string button)
	{
		return vp_Input.Instance.DoGetButton(button);
	}

	// Token: 0x0600AD58 RID: 44376 RVA: 0x00409B34 File Offset: 0x00407D34
	public virtual bool DoGetButton(string button)
	{
		if (this.Buttons.ContainsKey(button))
		{
			return Input.GetKey(this.Buttons[button]);
		}
		Debug.LogError("\"" + button + "\" is not in VP Input Manager's Buttons. You must add it for this Button to work.");
		return false;
	}

	// Token: 0x0600AD59 RID: 44377 RVA: 0x00409B6C File Offset: 0x00407D6C
	public static bool GetButtonDown(string button)
	{
		return vp_Input.Instance.DoGetButtonDown(button);
	}

	// Token: 0x0600AD5A RID: 44378 RVA: 0x00409B79 File Offset: 0x00407D79
	public virtual bool DoGetButtonDown(string button)
	{
		if (this.Buttons.ContainsKey(button))
		{
			return Input.GetKeyDown(this.Buttons[button]);
		}
		Debug.LogError("\"" + button + "\" is not in VP Input Manager's Buttons. You must add it for this Button to work.");
		return false;
	}

	// Token: 0x0600AD5B RID: 44379 RVA: 0x00409BB1 File Offset: 0x00407DB1
	public static bool GetButtonUp(string button)
	{
		return vp_Input.Instance.DoGetButtonUp(button);
	}

	// Token: 0x0600AD5C RID: 44380 RVA: 0x00409BBE File Offset: 0x00407DBE
	public virtual bool DoGetButtonUp(string button)
	{
		if (this.Buttons.ContainsKey(button))
		{
			return Input.GetKeyUp(this.Buttons[button]);
		}
		Debug.LogError("\"" + button + "\" is not in VP Input Manager's Buttons. You must add it for this Button to work.");
		return false;
	}

	// Token: 0x0600AD5D RID: 44381 RVA: 0x00409BF6 File Offset: 0x00407DF6
	public static float GetAxisRaw(string axis)
	{
		return vp_Input.Instance.DoGetAxisRaw(axis);
	}

	// Token: 0x0600AD5E RID: 44382 RVA: 0x00409C04 File Offset: 0x00407E04
	public virtual float DoGetAxisRaw(string axis)
	{
		if (this.Axis.ContainsKey(axis) && this.ControlType == 0)
		{
			float num = 0f;
			if (Input.GetKey(this.Axis[axis].Positive))
			{
				num = 1f;
			}
			if (Input.GetKey(this.Axis[axis].Negative))
			{
				num = -1f;
			}
			return num;
		}
		if (this.UnityAxis.Contains(axis))
		{
			return Input.GetAxisRaw(axis);
		}
		Debug.LogError("\"" + axis + "\" is not in VP Input Manager's Unity Axis. You must add it for this Axis to work.");
		return 0f;
	}

	// Token: 0x0600AD5F RID: 44383 RVA: 0x00409C9C File Offset: 0x00407E9C
	public static void ChangeButtonKey(string button, KeyCode keyCode, bool save = false)
	{
		if (!vp_Input.Instance.Buttons.ContainsKey(button))
		{
			Debug.LogWarning("The Button \"" + button + "\" Doesn't Exist");
			return;
		}
		if (save)
		{
			vp_Input.Instance.ButtonValues[vp_Input.Instance.ButtonKeys.IndexOf(button)] = keyCode;
		}
		vp_Input.Instance.Buttons[button] = keyCode;
	}

	// Token: 0x0600AD60 RID: 44384 RVA: 0x00409D08 File Offset: 0x00407F08
	public static void ChangeAxis(string n, KeyCode pk = KeyCode.None, KeyCode nk = KeyCode.None, bool save = false)
	{
		if (!vp_Input.Instance.AxisKeys.Contains(n))
		{
			Debug.LogWarning("The Axis \"" + n + "\" Doesn't Exist");
			return;
		}
		if (save)
		{
			vp_Input.Instance.AxisValues[vp_Input.Instance.AxisKeys.IndexOf(n)] = new vp_Input.vp_InputAxis
			{
				Positive = pk,
				Negative = nk
			};
		}
		vp_Input.Instance.Axis[n] = new vp_Input.vp_InputAxis
		{
			Positive = pk,
			Negative = nk
		};
	}

	// Token: 0x04008035 RID: 32821
	public int ControlType;

	// Token: 0x04008036 RID: 32822
	public Dictionary<string, KeyCode> Buttons = new Dictionary<string, KeyCode>();

	// Token: 0x04008037 RID: 32823
	public List<string> ButtonKeys = new List<string>();

	// Token: 0x04008038 RID: 32824
	public List<KeyCode> ButtonValues = new List<KeyCode>();

	// Token: 0x04008039 RID: 32825
	public Dictionary<string, vp_Input.vp_InputAxis> Axis = new Dictionary<string, vp_Input.vp_InputAxis>();

	// Token: 0x0400803A RID: 32826
	public List<string> AxisKeys = new List<string>();

	// Token: 0x0400803B RID: 32827
	public List<vp_Input.vp_InputAxis> AxisValues = new List<vp_Input.vp_InputAxis>();

	// Token: 0x0400803C RID: 32828
	public List<string> UnityAxis = new List<string>();

	// Token: 0x0400803D RID: 32829
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public static string m_FolderPath = "UltimateFPS/Content/Resources/Input";

	// Token: 0x0400803E RID: 32830
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public static string m_PrefabPath = "Assets/UltimateFPS/Content/Resources/Input/vp_Input.prefab";

	// Token: 0x0400803F RID: 32831
	public static bool mIsDirty = true;

	// Token: 0x04008040 RID: 32832
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public static vp_Input m_Instance;

	// Token: 0x020015B7 RID: 5559
	[Serializable]
	public class vp_InputAxis
	{
		// Token: 0x04008041 RID: 32833
		public KeyCode Positive;

		// Token: 0x04008042 RID: 32834
		public KeyCode Negative;
	}
}
