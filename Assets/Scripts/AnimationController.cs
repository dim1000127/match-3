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

    /*public IEnumerator AnimDestroy(GameObject gameObjectDestroy) 
    {
        var anim = gameObjectDestroy.GetComponent<Animator>();
        anim.Play(deleteGems);

        yield return new WaitForSeconds(1f);
    }*/

     /*public IEnumerator DeleteGems(GameObject gameObjectDestroy)
     {
         var anim = gameObjectDestroy.GetComponent<Animation>();
         anim.Play(deleteGems);
         do
         {
             yield return null;
         } while (anim.isPlaying);

     }*/

    /*public void DeleteGems(GameObject gameObject)
    {
        var animMore = gameObject.GetComponent<Animator>();
        animMore.Play(deleteGems);
    }*/

    /*public IEnumerator AnimDestroy(GameObject gameObjectDestroy) 
    {
        var anim = gameObjectDestroy.GetComponent<Animation>();
        anim.Play(animDestroy);

        yield return new WaitForSeconds(1f);
    }*/

    /*public void AnimSpawn(GameObject gameObjectDestroy)
    {
        var anim = gameObjectDestroy.GetComponent<Animation>();
        anim.Play("AnimationAppearance");
    }*/

    /* public void AnimDestroy(GameObject gameObjectDestroy) 
     {
         var anim = gameObjectDestroy.GetComponent<Animation>();
         anim.Play(animDestroy);
     }*/
}
