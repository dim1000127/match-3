using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private const string scaleLess = "AnimationScaleLess";
    private const string scaleMore = "AnimationScaleMore";
    private const string deleteGems = "AnimationDisappearance";


    public void ScaleLess(GameObject gameObject) {
        var animLess = gameObject.GetComponent<Animator>();
        animLess.Play(scaleLess);
    }

    public void ScaleMore(GameObject gameObject)
    {
        var animMore = gameObject.GetComponent<Animator>();
        animMore.Play(scaleMore);
    }

    public void DeleteGems(GameObject gameObject)
    {
        var animDel = gameObject.GetComponent<Animator>();
        animDel.Play(deleteGems);
    }
}
