// Decompiled with JetBrains decompiler
// Type: SatelliteMenu.PopoutAnimationFactory
// Assembly: SatelliteMenu, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 193A7BB1-5276-4CC6-8511-0A490974AD2E
// Assembly location: C:\Users\trungdc\Desktop\satellite-menu-1.2.1.0\lib\android\SatelliteMenu.dll

using System;
using Android.Content;
using Android.Views.Animations;

namespace bottomtabbedpage.Droid.Satellite
{
    /// <summary>
    /// Popout animation factory creates parts of the popout bounce animation pieces from the resource definitions.
    /// </summary>
    public class PopoutAnimationFactory
    {
        public static Animation CreateItemInAnimation(Context context, int index, long expandDuration, int x, int y)
        {
            var translateAnimation = new TranslateAnimation(x, 0.0f, y, 0.0f);
            long num1 = 250;
            if (expandDuration <= 250L)
                num1 = expandDuration / 3L;
            long num2 = 250;
            if (expandDuration - num1 > num2)
                num2 = expandDuration - num1;
            translateAnimation.Duration = num2;
            translateAnimation.StartOffset = num1;
            var alphaAnimation = new AlphaAnimation(1f, 0.0f);
            long num3 = 10;
            if (expandDuration < 10L)
                num3 = expandDuration / 10L;
            alphaAnimation.Duration = num3;
            alphaAnimation.StartOffset = num1 + num2 - num3;
            var animationSet = new AnimationSet(false)
            {
                FillAfter = false,
                FillBefore = true,
                FillEnabled = true
            };
            animationSet.AddAnimation(alphaAnimation);
            animationSet.AddAnimation(translateAnimation);
            animationSet.StartOffset = 30 * index;
            animationSet.Start();
            animationSet.StartNow();
            return animationSet;
        }

        public static Animation CreateItemOutAnimation(Context context, int index, long expandDuration, int x, int y)
        {
            var alphaAnimation = new AlphaAnimation(0.0f, 1f);
            long num1 = 60;
            if (expandDuration < 60L)
                num1 = expandDuration / 4L;
            alphaAnimation.Duration = num1;
            alphaAnimation.StartOffset = 0L;
            var translateAnimation = new TranslateAnimation(0.0f, x, 0.0f, y)
            {
                StartOffset = 0L,
                Duration = expandDuration
            };
            translateAnimation.SetInterpolator(context, Resource.Animation.popoutMenuItemOvershootInterpolator);
            var rotateAnimation = new RotateAnimation(0.0f, 360f, Dimension.RelativeToSelf, 0.5f, Dimension.RelativeToSelf, 0.5f);
            rotateAnimation.SetInterpolator(context, Resource.Animation.popoutMenuItemOutRotateInterpolator);
            long num2 = 100;
            if (expandDuration <= 150L)
                num2 = expandDuration / 3L;
            rotateAnimation.Duration = expandDuration - num2;
            rotateAnimation.StartOffset = num2;
            var animationSet = new AnimationSet(false)
            {
                FillAfter = false,
                FillBefore = true,
                FillEnabled = true
            };
            animationSet.AddAnimation(alphaAnimation);
            animationSet.AddAnimation(rotateAnimation);
            animationSet.AddAnimation(translateAnimation);
            animationSet.StartOffset = 30 * index;
            return animationSet;
        }

        public static Animation CreateMainButtonAnimation(Context context)
        {
            return AnimationUtils.LoadAnimation(context, Resource.Animation.popoutMenuRotateLeftAnim);
        }

        public static Animation CreateMainButtonInverseAnimation(Context context)
        {
            return AnimationUtils.LoadAnimation(context, Resource.Animation.popoutMenuRotateRight);
        }

        public static Animation CreateItemClickAnimation(Context context)
        {
            return AnimationUtils.LoadAnimation(context, Resource.Animation.popoutMenuItemClickAnim);
        }

        public static int GetTranslateX(float degree, int distance)
        {
            return (int)Math.Floor(distance * Math.Cos(DegreeToRadian(degree)));
        }

        public static int GetTranslateY(float degree, int distance)
        {
            return (int)Math.Floor(-1 * distance * Math.Sin(DegreeToRadian(degree)));
        }

        private static double DegreeToRadian(double angle)
        {
            return Math.PI * angle / 180.0;
        }
    }
}