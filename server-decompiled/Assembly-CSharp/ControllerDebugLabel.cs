using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using InControl;
using UnityEngine;

// Token: 0x020011A8 RID: 4520
public class ControllerDebugLabel : MonoBehaviour
{
	// Token: 0x06009058 RID: 36952 RVA: 0x00365374 File Offset: 0x00363574
	public ControllerDebugLabel()
	{
		this.m_debugStringProviders = new SortedDictionary<string, Action<StringBuilder>>();
		this.m_debugStringProviderNames = new List<string>();
		this.m_debugStringBuilderMain = new StringBuilder(4096);
		this.m_debugStringBuilderForProvider = new StringBuilder(4096);
		this.m_debugStringBuilderForControlsString = new StringBuilder(512);
	}

	// Token: 0x06009059 RID: 36953 RVA: 0x00365401 File Offset: 0x00363601
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnEnable()
	{
		this.m_label = base.GetComponent<UILabel>();
		this.AddDebugProvider("Key Codes", new Action<StringBuilder>(this.BuildKeyCodeString));
		this.AddDebugProvider("Devices", new Action<StringBuilder>(this.BuildDevicesString));
	}

	// Token: 0x0600905A RID: 36954 RVA: 0x0036543D File Offset: 0x0036363D
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDisable()
	{
		this.RemoveDebugProvider("Key Codes");
		this.RemoveDebugProvider("Devices");
	}

	// Token: 0x0600905B RID: 36955 RVA: 0x00365458 File Offset: 0x00363658
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

	// Token: 0x0600905C RID: 36956 RVA: 0x003655E0 File Offset: 0x003637E0
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateProviderNames()
	{
		this.m_debugStringProviderNames.Clear();
		this.m_debugStringProviderNames.AddRange(this.m_debugStringProviders.Keys);
	}

	// Token: 0x0600905D RID: 36957 RVA: 0x00365603 File Offset: 0x00363803
	public void AddDebugProvider(string providerName, Action<StringBuilder> provider)
	{
		bool flag = !this.m_debugStringProviders.ContainsKey(providerName);
		this.m_debugStringProviders[providerName] = provider;
		if (flag)
		{
			this.UpdateProviderNames();
		}
	}

	// Token: 0x0600905E RID: 36958 RVA: 0x00365629 File Offset: 0x00363829
	public void RemoveDebugProvider(string providerName)
	{
		if (this.m_debugStringProviders.Remove(providerName))
		{
			this.UpdateProviderNames();
		}
	}

	// Token: 0x0600905F RID: 36959 RVA: 0x00365640 File Offset: 0x00363840
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

	// Token: 0x06009060 RID: 36960 RVA: 0x00365690 File Offset: 0x00363890
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

	// Token: 0x06009061 RID: 36961 RVA: 0x00365708 File Offset: 0x00363908
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

	// Token: 0x04006AB8 RID: 27320
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const string KEY_DEVICES = "Devices";

	// Token: 0x04006AB9 RID: 27321
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const string KEY_KEY_CODES = "Key Codes";

	// Token: 0x04006ABA RID: 27322
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly SortedDictionary<string, Action<StringBuilder>> m_debugStringProviders;

	// Token: 0x04006ABB RID: 27323
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly List<string> m_debugStringProviderNames;

	// Token: 0x04006ABC RID: 27324
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly StringBuilder m_debugStringBuilderMain;

	// Token: 0x04006ABD RID: 27325
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly StringBuilder m_debugStringBuilderForProvider;

	// Token: 0x04006ABE RID: 27326
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly StringBuilder m_debugStringBuilderForControlsString;

	// Token: 0x04006ABF RID: 27327
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public UILabel m_label;

	// Token: 0x04006AC0 RID: 27328
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly KeyCode[] m_allKeyCodes = (KeyCode[])Enum.GetValues(typeof(KeyCode));

	// Token: 0x04006AC1 RID: 27329
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly InputControlType[] m_allControls = (InputControlType[])Enum.GetValues(typeof(InputControlType));
}
