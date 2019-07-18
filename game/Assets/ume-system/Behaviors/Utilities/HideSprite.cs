using UnityEngine;
using System.Collections;
namespace UME{

    [AddComponentMenu("UME/Utility/HideSprite")]
    public class HideSprite : BaseTrigger
	{

		// Use this for initialization
		public override void Initialize ()
		{
			GetComponent<SpriteRenderer>().enabled = false;
		}

	}
}

