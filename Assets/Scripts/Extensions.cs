using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    static class Extensions
    {

        public static bool Overlaps(this RectTransform thisRectTransform,RectTransform otherRectTransform)
        {
            return thisRectTransform.toRect().Overlaps(otherRectTransform.toRect());
        }

        public static Rect toRect(this RectTransform rectTransform)
        {
            return new Rect(rectTransform.position, rectTransform.rect.size);
        } 
    }
}
