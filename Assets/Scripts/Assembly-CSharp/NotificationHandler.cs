using System;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Text))]
public class NotificationHandler : MonoBehaviour
{
	
	private void Awake()
	{
		NotificationHandler.Instance = this;
		this.m_text = base.GetComponent<Text>();
	}

	
	public void Notify(string text)
	{
		this.m_text.text = text;
		this.m_alpha = 1.5f;
	}

	
	private void FixedUpdate()
	{
		bool flag = this.m_alpha > 0f;
		if (flag)
		{
			this.m_text.color = new Color(1f, 1f, 1f, this.m_alpha);
			this.m_alpha -= this.alphaDecay;
		}
		bool flag2 = this.m_alpha <= 0f && this.m_text.color != Color.clear;
		if (flag2)
		{
			this.m_text.color = Color.clear;
			this.m_alpha = 0f;
		}
	}

	
	public static NotificationHandler Instance;

	
	public float alphaDecay = 0.01f;

	
	private Text m_text;

	
	private float m_alpha = 0f;
}
