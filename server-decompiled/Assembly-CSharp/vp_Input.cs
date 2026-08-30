using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001582 RID: 5506
public class vp_Input : MonoBehaviour
{
	// Token: 0x170013EA RID: 5098
	// (get) Token: 0x0600AC41 RID: 44097 RVA: 0x00402460 File Offset: 0x00400660
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

	// Token: 0x0600AC42 RID: 44098 RVA: 0x000027FC File Offset: 0x000009FC
	public static void CreateIfNoExist()
	{
	}

	// Token: 0x0600AC43 RID: 44099 RVA: 0x004024F4 File Offset: 0x004006F4
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Awake()
	{
		if (vp_Input.m_Instance == null)
		{
			vp_Input.m_Instance = vp_Input.Instance;
		}
	}

	// Token: 0x0600AC44 RID: 44100 RVA: 0x0040250D File Offset: 0x0040070D
	public virtual void SetDirty(bool dirty)
	{
		vp_Input.mIsDirty = dirty;
	}

	// Token: 0x0600AC45 RID: 44101 RVA: 0x00402518 File Offset: 0x00400718
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

	// Token: 0x0600AC46 RID: 44102 RVA: 0x0040269C File Offset: 0x0040089C
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

	// Token: 0x0600AC47 RID: 44103 RVA: 0x004026E8 File Offset: 0x004008E8
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

	// Token: 0x0600AC48 RID: 44104 RVA: 0x00402758 File Offset: 0x00400958
	public virtual void AddUnityAxis(string n)
	{
		if (this.UnityAxis.Contains(n))
		{
			this.UnityAxis[this.UnityAxis.IndexOf(n)] = n;
			return;
		}
		this.UnityAxis.Add(n);
	}

	// Token: 0x0600AC49 RID: 44105 RVA: 0x00402790 File Offset: 0x00400990
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

	// Token: 0x0600AC4A RID: 44106 RVA: 0x00402854 File Offset: 0x00400A54
	public static bool GetButtonAny(string button)
	{
		return vp_Input.Instance.DoGetButtonAny(button);
	}

	// Token: 0x0600AC4B RID: 44107 RVA: 0x00402864 File Offset: 0x00400A64
	public virtual bool DoGetButtonAny(string button)
	{
		if (this.Buttons.ContainsKey(button))
		{
			return Input.GetKey(this.Buttons[button]) || Input.GetKeyDown(this.Buttons[button]) || Input.GetKeyUp(this.Buttons[button]);
		}
		Debug.LogError("\"" + button + "\" is not in VP Input Manager's Buttons. You must add it for this Button to work.");
		return false;
	}

	// Token: 0x0600AC4C RID: 44108 RVA: 0x004028CF File Offset: 0x00400ACF
	public static bool GetButton(string button)
	{
		return vp_Input.Instance.DoGetButton(button);
	}

	// Token: 0x0600AC4D RID: 44109 RVA: 0x004028DC File Offset: 0x00400ADC
	public virtual bool DoGetButton(string button)
	{
		if (this.Buttons.ContainsKey(button))
		{
			return Input.GetKey(this.Buttons[button]);
		}
		Debug.LogError("\"" + button + "\" is not in VP Input Manager's Buttons. You must add it for this Button to work.");
		return false;
	}

	// Token: 0x0600AC4E RID: 44110 RVA: 0x00402914 File Offset: 0x00400B14
	public static bool GetButtonDown(string button)
	{
		return vp_Input.Instance.DoGetButtonDown(button);
	}

	// Token: 0x0600AC4F RID: 44111 RVA: 0x00402921 File Offset: 0x00400B21
	public virtual bool DoGetButtonDown(string button)
	{
		if (this.Buttons.ContainsKey(button))
		{
			return Input.GetKeyDown(this.Buttons[button]);
		}
		Debug.LogError("\"" + button + "\" is not in VP Input Manager's Buttons. You must add it for this Button to work.");
		return false;
	}

	// Token: 0x0600AC50 RID: 44112 RVA: 0x00402959 File Offset: 0x00400B59
	public static bool GetButtonUp(string button)
	{
		return vp_Input.Instance.DoGetButtonUp(button);
	}

	// Token: 0x0600AC51 RID: 44113 RVA: 0x00402966 File Offset: 0x00400B66
	public virtual bool DoGetButtonUp(string button)
	{
		if (this.Buttons.ContainsKey(button))
		{
			return Input.GetKeyUp(this.Buttons[button]);
		}
		Debug.LogError("\"" + button + "\" is not in VP Input Manager's Buttons. You must add it for this Button to work.");
		return false;
	}

	// Token: 0x0600AC52 RID: 44114 RVA: 0x0040299E File Offset: 0x00400B9E
	public static float GetAxisRaw(string axis)
	{
		return vp_Input.Instance.DoGetAxisRaw(axis);
	}

	// Token: 0x0600AC53 RID: 44115 RVA: 0x004029AC File Offset: 0x00400BAC
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

	// Token: 0x0600AC54 RID: 44116 RVA: 0x00402A44 File Offset: 0x00400C44
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

	// Token: 0x0600AC55 RID: 44117 RVA: 0x00402AB0 File Offset: 0x00400CB0
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

	// Token: 0x04007F71 RID: 32625
	public int ControlType;

	// Token: 0x04007F72 RID: 32626
	public Dictionary<string, KeyCode> Buttons = new Dictionary<string, KeyCode>();

	// Token: 0x04007F73 RID: 32627
	public List<string> ButtonKeys = new List<string>();

	// Token: 0x04007F74 RID: 32628
	public List<KeyCode> ButtonValues = new List<KeyCode>();

	// Token: 0x04007F75 RID: 32629
	public Dictionary<string, vp_Input.vp_InputAxis> Axis = new Dictionary<string, vp_Input.vp_InputAxis>();

	// Token: 0x04007F76 RID: 32630
	public List<string> AxisKeys = new List<string>();

	// Token: 0x04007F77 RID: 32631
	public List<vp_Input.vp_InputAxis> AxisValues = new List<vp_Input.vp_InputAxis>();

	// Token: 0x04007F78 RID: 32632
	public List<string> UnityAxis = new List<string>();

	// Token: 0x04007F79 RID: 32633
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public static string m_FolderPath = "UltimateFPS/Content/Resources/Input";

	// Token: 0x04007F7A RID: 32634
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public static string m_PrefabPath = "Assets/UltimateFPS/Content/Resources/Input/vp_Input.prefab";

	// Token: 0x04007F7B RID: 32635
	public static bool mIsDirty = true;

	// Token: 0x04007F7C RID: 32636
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public static vp_Input m_Instance;

	// Token: 0x02001583 RID: 5507
	[Serializable]
	public class vp_InputAxis
	{
		// Token: 0x04007F7D RID: 32637
		public KeyCode Positive;

		// Token: 0x04007F7E RID: 32638
		public KeyCode Negative;
	}
}
