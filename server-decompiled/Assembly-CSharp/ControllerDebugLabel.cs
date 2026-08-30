using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using InControl;
using UnityEngine;

// Token: 0x02001190 RID: 4496
public class ControllerDebugLabel : MonoBehaviour
{
	// Token: 0x06008FCC RID: 36812 RVA: 0x00360BA8 File Offset: 0x0035EDA8
	public ControllerDebugLabel()
	{
		this.m_debugStringProviders = new SortedDictionary<string, Action<StringBuilder>>();
		this.m_debugStringProviderNames = new List<string>();
		this.m_debugStringBuilderMain = new StringBuilder(4096);
		this.m_debugStringBuilderForProvider = new StringBuilder(4096);
		this.m_debugStringBuilderForControlsString = new StringBuilder(512);
	}

	// Token: 0x06008FCD RID: 36813 RVA: 0x00360C35 File Offset: 0x0035EE35
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnEnable()
	{
		this.m_label = base.GetComponent<UILabel>();
		this.AddDebugProvider("Key Codes", new Action<StringBuilder>(this.BuildKeyCodeString));
		this.AddDebugProvider("Devices", new Action<StringBuilder>(this.BuildDevicesString));
	}

	// Token: 0x06008FCE RID: 36814 RVA: 0x00360C71 File Offset: 0x0035EE71
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDisable()
	{
		this.RemoveDebugProvider("Key Codes");
		this.RemoveDebugProvider("Devices");
	}

	// Token: 0x06008FCF RID: 36815 RVA: 0x00360C8C File Offset: 0x0035EE8C
	[PublicizedFrom(EAccessModifier.Private)]
	public void Update()
	{
		if (this.m_label == null)
		{
			return;
		}
		this.m_debugStringBuilderMain.Clear();
		for (int i = 0; i < this.m_debugStringProviderNames.Count; i++)
		{
			string text = this.m_debugStringProviderNames[i];
			Action<StringBuilder> action = this.m_debugStringProviders[text];
			this.m_debugStringBuilderForProvider.Clear();
			try
			{
				action(this.m_debugStringBuilderForProvider);
			}
			catch (Exception ex)
			{
				this.m_debugStringBuilderForProvider.Clear();
				this.m_debugStringBuilderForProvider.Append(ex.Message);
			}
			if (this.m_debugStringBuilderForProvider.Length != 0)
			{
				if (this.m_debugStringBuilderMain.Length != 0)
				{
					this.m_debugStringBuilderMain.Append('\n');
				}
				this.m_debugStringBuilderMain.Append(text);
				this.m_debugStringBuilderMain.Append(": ");
				for (int j = 0; j < this.m_debugStringBuilderForProvider.Length; j++)
				{
					char c = this.m_debugStringBuilderForProvider[j];
					this.m_debugStringBuilderMain.Append(c);
					if (c == '\n')
					{
						for (int k = 0; k < text.Length + 2; k++)
						{
							this.m_debugStringBuilderMain.Append(' ');
						}
					}
				}
			}
		}
		if (!this.m_debugStringBuilderMain.Equals(this.m_label.text))
		{
			this.m_label.text = this.m_debugStringBuilderMain.ToString();
		}
	}

	// Token: 0x06008FD0 RID: 36816 RVA: 0x00360E14 File Offset: 0x0035F014
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateProviderNames()
	{
		this.m_debugStringProviderNames.Clear();
		this.m_debugStringProviderNames.AddRange(this.m_debugStringProviders.Keys);
	}

	// Token: 0x06008FD1 RID: 36817 RVA: 0x00360E37 File Offset: 0x0035F037
	public void AddDebugProvider(string providerName, Action<StringBuilder> provider)
	{
		bool flag = !this.m_debugStringProviders.ContainsKey(providerName);
		this.m_debugStringProviders[providerName] = provider;
		if (flag)
		{
			this.UpdateProviderNames();
		}
	}

	// Token: 0x06008FD2 RID: 36818 RVA: 0x00360E5D File Offset: 0x0035F05D
	public void RemoveDebugProvider(string providerName)
	{
		if (this.m_debugStringProviders.Remove(providerName))
		{
			this.UpdateProviderNames();
		}
	}

	// Token: 0x06008FD3 RID: 36819 RVA: 0x00360E74 File Offset: 0x0035F074
	[PublicizedFrom(EAccessModifier.Private)]
	public void BuildKeyCodeString(StringBuilder builder)
	{
		foreach (KeyCode keyCode in this.m_allKeyCodes)
		{
			if (Input.GetKey(keyCode))
			{
				if (builder.Length != 0)
				{
					builder.Append(" + ");
				}
				builder.Append(keyCode.ToStringCached<KeyCode>());
			}
		}
	}

	// Token: 0x06008FD4 RID: 36820 RVA: 0x00360EC4 File Offset: 0x0035F0C4
	[PublicizedFrom(EAccessModifier.Private)]
	public void BuildDevicesString(StringBuilder builder)
	{
		ReadOnlyCollection<InputDevice> devices = InputManager.Devices;
		for (int i = 0; i < devices.Count; i++)
		{
			InputDevice inputDevice = devices[i];
			if (inputDevice.IsActive)
			{
				this.m_debugStringBuilderForControlsString.Clear();
				this.BuildControlsString(this.m_debugStringBuilderForControlsString, inputDevice);
				if (this.m_debugStringBuilderForControlsString.Length != 0)
				{
					if (builder.Length != 0)
					{
						builder.Append('\n');
					}
					builder.Append(this.m_debugStringBuilderForControlsString);
				}
			}
		}
	}

	// Token: 0x06008FD5 RID: 36821 RVA: 0x00360F3C File Offset: 0x0035F13C
	[PublicizedFrom(EAccessModifier.Private)]
	public void BuildControlsString(StringBuilder builder, InputDevice device)
	{
		bool flag = false;
		builder.Append(device.Name);
		builder.Append(" (");
		builder.Append(device.Meta);
		builder.Append("): ");
		foreach (InputControlType inputControlType in this.m_allControls)
		{
			if (inputControlType != InputControlType.None && inputControlType != InputControlType.Count)
			{
				InputControl control = device.GetControl(inputControlType);
				if (control.IsPressed || control.RawValue != 0f)
				{
					flag = true;
					if (builder.Length != 0)
					{
						builder.Append(" + ");
					}
					builder.Append(inputControlType.ToStringCached<InputControlType>());
					if (control.IsAnalog)
					{
						builder.AppendFormat("={0:F4}", control.RawValue);
					}
				}
			}
		}
		if (!flag)
		{
			builder.Clear();
		}
	}

	// Token: 0x04006A52 RID: 27218
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const string KEY_DEVICES = "Devices";

	// Token: 0x04006A53 RID: 27219
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const string KEY_KEY_CODES = "Key Codes";

	// Token: 0x04006A54 RID: 27220
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly SortedDictionary<string, Action<StringBuilder>> m_debugStringProviders;

	// Token: 0x04006A55 RID: 27221
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly List<string> m_debugStringProviderNames;

	// Token: 0x04006A56 RID: 27222
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly StringBuilder m_debugStringBuilderMain;

	// Token: 0x04006A57 RID: 27223
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly StringBuilder m_debugStringBuilderForProvider;

	// Token: 0x04006A58 RID: 27224
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly StringBuilder m_debugStringBuilderForControlsString;

	// Token: 0x04006A59 RID: 27225
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public UILabel m_label;

	// Token: 0x04006A5A RID: 27226
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly KeyCode[] m_allKeyCodes = (KeyCode[])Enum.GetValues(typeof(KeyCode));

	// Token: 0x04006A5B RID: 27227
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly InputControlType[] m_allControls = (InputControlType[])Enum.GetValues(typeof(InputControlType));
}
