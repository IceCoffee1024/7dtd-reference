using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001567 RID: 5479
public class vp_Input : MonoBehaviour
{
	// Token: 0x170013B1 RID: 5041
	// (get) Token: 0x0600AB44 RID: 43844 RVA: 0x003FD120 File Offset: 0x003FB320
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

	// Token: 0x0600AB45 RID: 43845 RVA: 0x000027FC File Offset: 0x000009FC
	public static void CreateIfNoExist()
	{
	}

	// Token: 0x0600AB46 RID: 43846 RVA: 0x003FD1B4 File Offset: 0x003FB3B4
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Awake()
	{
		if (vp_Input.m_Instance == null)
		{
			vp_Input.m_Instance = vp_Input.Instance;
		}
	}

	// Token: 0x0600AB47 RID: 43847 RVA: 0x003FD1CD File Offset: 0x003FB3CD
	public virtual void SetDirty(bool dirty)
	{
		vp_Input.mIsDirty = dirty;
	}

	// Token: 0x0600AB48 RID: 43848 RVA: 0x003FD1D8 File Offset: 0x003FB3D8
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

	// Token: 0x0600AB49 RID: 43849 RVA: 0x003FD35C File Offset: 0x003FB55C
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

	// Token: 0x0600AB4A RID: 43850 RVA: 0x003FD3A8 File Offset: 0x003FB5A8
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

	// Token: 0x0600AB4B RID: 43851 RVA: 0x003FD418 File Offset: 0x003FB618
	public virtual void AddUnityAxis(string n)
	{
		if (this.UnityAxis.Contains(n))
		{
			this.UnityAxis[this.UnityAxis.IndexOf(n)] = n;
			return;
		}
		this.UnityAxis.Add(n);
	}

	// Token: 0x0600AB4C RID: 43852 RVA: 0x003FD450 File Offset: 0x003FB650
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

	// Token: 0x0600AB4D RID: 43853 RVA: 0x003FD514 File Offset: 0x003FB714
	public static bool GetButtonAny(string button)
	{
		return vp_Input.Instance.DoGetButtonAny(button);
	}

	// Token: 0x0600AB4E RID: 43854 RVA: 0x003FD524 File Offset: 0x003FB724
	public virtual bool DoGetButtonAny(string button)
	{
		if (this.Buttons.ContainsKey(button))
		{
			return Input.GetKey(this.Buttons[button]) || Input.GetKeyDown(this.Buttons[button]) || Input.GetKeyUp(this.Buttons[button]);
		}
		Debug.LogError("\"" + button + "\" is not in VP Input Manager's Buttons. You must add it for this Button to work.");
		return false;
	}

	// Token: 0x0600AB4F RID: 43855 RVA: 0x003FD58F File Offset: 0x003FB78F
	public static bool GetButton(string button)
	{
		return vp_Input.Instance.DoGetButton(button);
	}

	// Token: 0x0600AB50 RID: 43856 RVA: 0x003FD59C File Offset: 0x003FB79C
	public virtual bool DoGetButton(string button)
	{
		if (this.Buttons.ContainsKey(button))
		{
			return Input.GetKey(this.Buttons[button]);
		}
		Debug.LogError("\"" + button + "\" is not in VP Input Manager's Buttons. You must add it for this Button to work.");
		return false;
	}

	// Token: 0x0600AB51 RID: 43857 RVA: 0x003FD5D4 File Offset: 0x003FB7D4
	public static bool GetButtonDown(string button)
	{
		return vp_Input.Instance.DoGetButtonDown(button);
	}

	// Token: 0x0600AB52 RID: 43858 RVA: 0x003FD5E1 File Offset: 0x003FB7E1
	public virtual bool DoGetButtonDown(string button)
	{
		if (this.Buttons.ContainsKey(button))
		{
			return Input.GetKeyDown(this.Buttons[button]);
		}
		Debug.LogError("\"" + button + "\" is not in VP Input Manager's Buttons. You must add it for this Button to work.");
		return false;
	}

	// Token: 0x0600AB53 RID: 43859 RVA: 0x003FD619 File Offset: 0x003FB819
	public static bool GetButtonUp(string button)
	{
		return vp_Input.Instance.DoGetButtonUp(button);
	}

	// Token: 0x0600AB54 RID: 43860 RVA: 0x003FD626 File Offset: 0x003FB826
	public virtual bool DoGetButtonUp(string button)
	{
		if (this.Buttons.ContainsKey(button))
		{
			return Input.GetKeyUp(this.Buttons[button]);
		}
		Debug.LogError("\"" + button + "\" is not in VP Input Manager's Buttons. You must add it for this Button to work.");
		return false;
	}

	// Token: 0x0600AB55 RID: 43861 RVA: 0x003FD65E File Offset: 0x003FB85E
	public static float GetAxisRaw(string axis)
	{
		return vp_Input.Instance.DoGetAxisRaw(axis);
	}

	// Token: 0x0600AB56 RID: 43862 RVA: 0x003FD66C File Offset: 0x003FB86C
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

	// Token: 0x0600AB57 RID: 43863 RVA: 0x003FD704 File Offset: 0x003FB904
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

	// Token: 0x0600AB58 RID: 43864 RVA: 0x003FD770 File Offset: 0x003FB970
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

	// Token: 0x04007E98 RID: 32408
	public int ControlType;

	// Token: 0x04007E99 RID: 32409
	public Dictionary<string, KeyCode> Buttons = new Dictionary<string, KeyCode>();

	// Token: 0x04007E9A RID: 32410
	public List<string> ButtonKeys = new List<string>();

	// Token: 0x04007E9B RID: 32411
	public List<KeyCode> ButtonValues = new List<KeyCode>();

	// Token: 0x04007E9C RID: 32412
	public Dictionary<string, vp_Input.vp_InputAxis> Axis = new Dictionary<string, vp_Input.vp_InputAxis>();

	// Token: 0x04007E9D RID: 32413
	public List<string> AxisKeys = new List<string>();

	// Token: 0x04007E9E RID: 32414
	public List<vp_Input.vp_InputAxis> AxisValues = new List<vp_Input.vp_InputAxis>();

	// Token: 0x04007E9F RID: 32415
	public List<string> UnityAxis = new List<string>();

	// Token: 0x04007EA0 RID: 32416
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public static string m_FolderPath = "UltimateFPS/Content/Resources/Input";

	// Token: 0x04007EA1 RID: 32417
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public static string m_PrefabPath = "Assets/UltimateFPS/Content/Resources/Input/vp_Input.prefab";

	// Token: 0x04007EA2 RID: 32418
	public static bool mIsDirty = true;

	// Token: 0x04007EA3 RID: 32419
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public static vp_Input m_Instance;

	// Token: 0x02001568 RID: 5480
	[Serializable]
	public class vp_InputAxis
	{
		// Token: 0x04007EA4 RID: 32420
		public KeyCode Positive;

		// Token: 0x04007EA5 RID: 32421
		public KeyCode Negative;
	}
}
