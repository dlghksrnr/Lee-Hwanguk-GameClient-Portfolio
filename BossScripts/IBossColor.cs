using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBossColor
{

    //public IEnumerator Phase2Color(string phase2Color,Renderer rend)
    //{
    //    Color colorToBlink = Color.white;

    //    if (ColorUtility.TryParseHtmlString(phase2Color, out colorToBlink))
    //    {
    //        while (true)
    //        {
    //            rend.material.color = colorToBlink;
    //            yield return new WaitForSeconds(1f);
    //            rend.material.color = Color.white;
    //            yield return new WaitForSeconds(1f);
    //        }
    //    }
    //}

    //public IEnumerator Phase3Color(string phase3Color, Renderer rend) //->override
    //{
    //    Color colorToBlink = Color.white;

    //    if (ColorUtility.TryParseHtmlString(phase3Color, out colorToBlink))
    //    {
    //        while (true)
    //        {
    //            rend.material.color = colorToBlink;
    //            yield return new WaitForSeconds(0.5f);
    //            rend.material.color = Color.white;
    //            yield return new WaitForSeconds(0.5f);
    //        }
    //    }
    //}
}
