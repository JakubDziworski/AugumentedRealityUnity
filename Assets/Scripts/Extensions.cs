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
            Rect thisRectangle = new Rect(thisRectTransform.position,thisRectTransform.rect.size);
            Rect otherRectangle = new Rect(otherRectTransform.position, otherRectTransform.rect.size);
            return thisRectangle.Overlaps(otherRectangle);
        }
    }
}
