using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTab : MonoBehaviour
{
    public virtual IEnumerator BeforeShow()
    {

        yield break;
    }
	
    public virtual IEnumerator Show()
    {

        yield break;
    }

    public virtual IEnumerator AfterShow()
    {

        yield break;
    }
}
