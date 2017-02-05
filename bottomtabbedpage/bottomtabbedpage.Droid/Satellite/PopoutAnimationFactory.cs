// Decompiled with JetBrains decompiler
// Type: SatelliteMenu.PopoutAnimationFactory
// Assembly: SatelliteMenu, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 193A7BB1-5276-4CC6-8511-0A490974AD2E
// Assembly location: C:\Users\trungdc\Desktop\satellite-menu-1.2.1.0\lib\android\SatelliteMenu.dll

using System;
using System.CodeDom.Compiler;
using Android.Content;
using Android.Runtime;
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
    
    public class Resource
    {
        static Resource()
        {
            ResourceIdManager.UpdateIdValues();
        }

        public class Animation
        {
            public static int popoutMenuItemAnticipateInterpolator = 2130903040;
            public static int popoutMenuItemClickAnim = 2130903041;
            public static int popoutMenuItemClickInterpolator = 2130903042;
            public static int popoutMenuItemInRotateInterpolator = 2130903043;
            public static int popoutMenuItemOutRotateInterpolator = 2130903044;
            public static int popoutMenuItemOvershootInterpolator = 2130903045;
            public static int popoutMenuRotateLeftAnim = 2130903046;
            public static int popoutMenuRotateRight = 2130903047;

            static Animation()
            {
                ResourceIdManager.UpdateIdValues();
            }

            private Animation()
            {
            }
        }

        public class Attribute
        {
            public static int closeOnClick = 2130771971;
            public static int itemsAngle = 2130771969;
            public static int mainImage = 2130771972;
            public static int radius = 2130771970;
            public static int speed = 2130771968;

            static Attribute()
            {
                ResourceIdManager.UpdateIdValues();
            }

            private Attribute()
            {
            }
        }

        public class Id
        {
            public static int popoutMenuItem = 2131034112;

            static Id()
            {
                ResourceIdManager.UpdateIdValues();
            }

            private Id()
            {
            }
        }

        public class Layout
        {
            public static int popoutMenuItem = 2130837504;

            static Layout()
            {
                ResourceIdManager.UpdateIdValues();
            }

            private Layout()
            {
            }
        }

        public class String
        {
            public static int empty = 2130968576;

            static String()
            {
                ResourceIdManager.UpdateIdValues();
            }

            private String()
            {
            }
        }

        public class Styleable
        {
            public static int[] SatelliteMenu = {
                2130771968,
                2130771969,
                2130771970,
                2130771971,
                2130771972
            };

            public static int SatelliteMenu_closeOnClick = 3;
            public static int SatelliteMenu_itemsAngle = 1;
            public static int SatelliteMenu_mainImage = 4;
            public static int SatelliteMenu_radius = 2;
            public static int SatelliteMenu_speed = 0;

            static Styleable()
            {
                ResourceIdManager.UpdateIdValues();
            }

            private Styleable()
            {
            }
        }
    }
}
