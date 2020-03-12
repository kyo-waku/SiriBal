using UnityEngine;
using UnityEngine.UI;

public class DamagedEffect : MonoBehaviour 
{
	Image img;
    public bool IsDamaged{get; set;} = false;
	void Start ()
    {
		img = GetComponent<Image>();
		img.color = Color.clear;
	}
	void Update () 
	{
		if (IsDamaged)
		{
			this.img.color = new Color (0.5f, 0f, 0f, 0.5f);
            IsDamaged = false;
		}
		else
		{
			this.img.color = Color.Lerp (this.img.color, Color.clear, Time.deltaTime);
		}
	}
}