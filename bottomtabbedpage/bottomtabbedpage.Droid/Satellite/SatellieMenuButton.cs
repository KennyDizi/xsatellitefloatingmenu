using System;
using System.Collections.Generic;
using Android.Content;
using Android.OS;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace bottomtabbedpage.Droid.Satellite
{
    /// <summary>
    /// Popout menu button control, mimics the Path 2.0 menu system with the collection of buttons.
    /// </summary>
    public class SatelliteMenuButton : FrameLayout
    {
        private const int DEFAULT_RADIUS = 300;
        private const float DEFAULT_ITEMS_ANGLE = 90f;
        private const bool DEFAULT_CLOSE_ON_CLICK = true;
        private const int DEFAULT_SPEED = 400;
        private readonly List<SatelliteMenuButtonItem> _menuItems = new List<SatelliteMenuButtonItem>();

        private readonly Dictionary<View, SatelliteMenuButtonItem> _viewToItemMap =
            new Dictionary<View, SatelliteMenuButtonItem>();

        private Android.Views.Animations.Animation _mainRotateRight;
        private Android.Views.Animations.Animation _mainRotateLeft;
        private View _imgMain;
        private bool _plusAnimationActive;
        private bool _rotated;
        private int _measureDiff;

        /// <summary>
        /// Gets or sets the degree between the first and the last item.
        /// </summary>
        /// <value>The degree.</value>
        public float ItemsAngle { get; set; }

        /// <summary>
        /// Gets or sets the distance of items from the center button.
        /// </summary>
        /// <value>The radius.</value>
        public int Radius { get; set; }

        /// <summary>
        /// Gets or sets the duration of expand and collapse operations in milliseconds.
        /// </summary>
        /// <value>The speed.</value>
        public int Speed { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:bottomtabbedpage.Droid.Satellite.SatelliteMenuButton" /> close items on click.
        /// </summary>
        /// <value>
        ///     <c>true</c> if close items on click; otherwise, <c>false</c>.
        /// </value>
        public bool CloseItemsOnClick { get; set; }

        /// <summary>Occurs when menu item is being touched by the user.</summary>
        public event EventHandler<SatelliteMenuItemEventArgs> MenuItemClick;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:bottomtabbedpage.Droid.Satellite.SatelliteMenuButton" /> class.
        /// </summary>
        /// <param name="context">Context.</param>
        public SatelliteMenuButton(Context context)
            : base(context)
        {
            CloseItemsOnClick = DEFAULT_CLOSE_ON_CLICK;
            Speed = DEFAULT_SPEED;
            Radius = DEFAULT_RADIUS;
            ItemsAngle = DEFAULT_ITEMS_ANGLE;
            Initialize(context, null, 0);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:bottomtabbedpage.Droid.Satellite.SatelliteMenuButton" /> class.
        /// </summary>
        /// <param name="context">Context.</param>
        /// <param name="attrs">Attrs.</param>
        public SatelliteMenuButton(Context context, IAttributeSet attrs)
            : base(context, attrs)
        {
            CloseItemsOnClick = DEFAULT_CLOSE_ON_CLICK;
            Speed = DEFAULT_SPEED;
            Radius = DEFAULT_RADIUS;
            ItemsAngle = DEFAULT_ITEMS_ANGLE;
            Initialize(context, attrs, 0);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:bottomtabbedpage.Droid.Satellite.SatelliteMenuButton" /> class.
        /// </summary>
        /// <param name="context">Context.</param>
        /// <param name="attrs">Attrs.</param>
        /// <param name="style">Style.</param>
        public SatelliteMenuButton(Context context, IAttributeSet attrs, int style)
            : base(context, attrs, style)
        {
            CloseItemsOnClick = DEFAULT_CLOSE_ON_CLICK;
            Speed = DEFAULT_SPEED;
            Radius = DEFAULT_RADIUS;
            ItemsAngle = DEFAULT_ITEMS_ANGLE;
            Initialize(context, attrs, style);
        }

        /// <summary>Initialize the specified context, attrs and style.</summary>
        /// <param name="context">Context.</param>
        /// <param name="attrs">Attrs.</param>
        /// <param name="style">Style.</param>
        private void Initialize(Context context, IAttributeSet attrs, int style)
        {
            if (attrs != null)
            {
                var styledAttributes = context.ObtainStyledAttributes(attrs, Resource.Styleable.SatelliteMenu, style, 0);
                Radius = styledAttributes.GetDimensionPixelSize(Resource.Styleable.SatelliteMenu_radius,
                    DEFAULT_RADIUS);
                ItemsAngle = styledAttributes.GetFloat(Resource.Styleable.SatelliteMenu_itemsAngle,
                    DEFAULT_ITEMS_ANGLE);
                CloseItemsOnClick = styledAttributes.GetBoolean(Resource.Styleable.SatelliteMenu_closeOnClick,
                    DEFAULT_CLOSE_ON_CLICK);
                Speed = styledAttributes.GetInt(Resource.Styleable.SatelliteMenu_speed,
                    DEFAULT_SPEED);
                styledAttributes.Recycle();
            }
            _mainRotateLeft = PopoutAnimationFactory.CreateMainButtonAnimation(context);
            _mainRotateLeft.AnimationEnd +=

                (param0, param1) => _plusAnimationActive = false;
            _mainRotateRight = PopoutAnimationFactory.CreateMainButtonInverseAnimation(context);
            _mainRotateRight.AnimationStart +=

                (param0, param1) => _plusAnimationActive = false;
        }

        protected override void OnFinishInflate()
        {
            base.OnFinishInflate();
            _imgMain = GetChildAt(0);
            _imgMain.Click += HandleClick;
        }

        private void HandleClick(object sender, EventArgs e)
        {
            if (_plusAnimationActive)
                return;
            _plusAnimationActive = true;
            if (!_rotated)
            {
                _imgMain.StartAnimation(_mainRotateLeft);
                foreach (var menuItem in _menuItems)
                    menuItem.View.StartAnimation(menuItem.OutAnimation);
            }
            else
            {
                _imgMain.StartAnimation(_mainRotateRight);
                foreach (var menuItem in _menuItems)
                    menuItem.View.StartAnimation(menuItem.InAnimation);
            }
            _rotated = !_rotated;
        }

        private void OpenItems()
        {
            if (_plusAnimationActive)
                return;
            _plusAnimationActive = true;
            if (!_rotated)
            {
                _imgMain.StartAnimation(_mainRotateLeft);
                foreach (var menuItem in _menuItems)
                    menuItem.View.StartAnimation(menuItem.OutAnimation);
            }
            _rotated = !_rotated;
        }

        private void CloseItems()
        {
            if (_plusAnimationActive)
                return;
            _plusAnimationActive = true;
            if (_rotated)
            {
                _imgMain.StartAnimation(_mainRotateRight);
                foreach (var menuItem in _menuItems)
                    menuItem.View.StartAnimation(menuItem.InAnimation);
            }
            _rotated = !_rotated;
        }

        /// <summary>
        /// Default provider for degrees between items. For number of items up to 3 tries to keep items centered in the given total degrees. For number equal and bigger than four, distirbutes evenly using min and max degrees.
        /// </summary>
        private static float[] GetDegrees(int count, float totalDegrees)
        {
            if (count < 1)
                return new float[0];
            var num1 = count >= 4 ? count - 1 : count + 1;
            var numArray = new float[count];
            var num2 = totalDegrees / num1;
            for (var index = 0; index < count; ++index)
            {
                var num3 = index;
                if (count < 4)
                    ++num3;
                numArray[index] = num3 * num2;
            }
            return numArray;
        }

        public void AddItem(SatelliteMenuButtonItem item)
        {
            AddItems(new[] {item});
        }

        public void AddItems(SatelliteMenuButtonItem[] items)
        {
            _menuItems.AddRange(items);
            RemoveView(_imgMain);
            new TextView(Context).LayoutParameters = new ViewGroup.LayoutParams(0, 0);
            var degrees = GetDegrees(_menuItems.Count, ItemsAngle);
            var index = 0;
            foreach (var menuItem in _menuItems)
            {
                var translateX = PopoutAnimationFactory.GetTranslateX(degrees[index], Radius);
                var translateY = PopoutAnimationFactory.GetTranslateY(degrees[index], Radius);
                var imageView1 =
                    (ImageView)
                    LayoutInflater.From(Context).Inflate(Resource.Layout.popoutMenuItem, this, false);
                var imageView2 =
                    (ImageView)
                    LayoutInflater.From(Context).Inflate(Resource.Layout.popoutMenuItem, this, false);
                imageView1.Tag = menuItem.Tag;
                imageView2.Visibility = ViewStates.Gone;
                imageView1.Visibility = ViewStates.Gone;
                imageView2.Click += (sender, e) =>
               {
                   if (!(sender is View))
                       return;
                   var viewToItem = _viewToItemMap[(View)sender];
                   ((View)sender).StartAnimation(viewToItem.ClickAnimation);
                   if (MenuItemClick == null)
                       return;
                   MenuItemClick(this, new SatelliteMenuItemEventArgs()
                   {
                       MenuItem = viewToItem
                   });
               };
                imageView2.Tag = menuItem.Tag;
                var layoutParameters = imageView2.LayoutParameters as MarginLayoutParams;
                if (layoutParameters != null)
                {
                    layoutParameters.BottomMargin = Math.Abs(translateY);
                    layoutParameters.LeftMargin = Math.Abs(translateX);
                    imageView2.LayoutParameters = layoutParameters;
                }
                if (menuItem.ImgResourceId > 0)
                {
                    imageView1.SetImageResource(menuItem.ImgResourceId);
                    imageView2.SetImageResource(menuItem.ImgResourceId);
                }
                else if (menuItem.ImgDrawable != null)
                {
                    imageView1.SetImageDrawable(menuItem.ImgDrawable);
                    imageView2.SetImageDrawable(menuItem.ImgDrawable);
                }
                var itemOutAnimation = PopoutAnimationFactory.CreateItemOutAnimation(Context, index,
                    Speed, translateX, translateY);
                var itemInAnimation = PopoutAnimationFactory.CreateItemInAnimation(Context, index,
                    Speed, translateX, translateY);
                var itemClickAnimation = PopoutAnimationFactory.CreateItemClickAnimation(Context);
                menuItem.View = imageView1;
                menuItem.CloneView = imageView2;
                menuItem.InAnimation = itemInAnimation;
                menuItem.OutAnimation = itemOutAnimation;
                menuItem.ClickAnimation = itemClickAnimation;
                menuItem.FinalX = translateX;
                menuItem.FinalY = translateY;
                itemInAnimation.SetAnimationListener(
                    new AnimationListener(imageView1, true, _viewToItemMap));
                itemOutAnimation.SetAnimationListener(
                    new AnimationListener(imageView1, false, _viewToItemMap));
                itemClickAnimation.SetAnimationListener(
                    new ClickAnimationListener(this, menuItem.Tag));
                AddView(imageView1);
                AddView(imageView2);
                _viewToItemMap.Add(imageView1, menuItem);
                _viewToItemMap.Add(imageView2, menuItem);
                ++index;
            }
            AddView(_imgMain);
        }

        public void ResetItems()
        {
            if (_menuItems.Count <= 0)
                return;
            var satelliteMenuButtonItemList =
                new List<SatelliteMenuButtonItem>(_menuItems);
            _menuItems.Clear();
            RemoveAllViews();
            AddItems(satelliteMenuButtonItemList.ToArray());
        }

        private void RecalculateMeasureDiff()
        {
            var num = 0;
            if (_menuItems.Count > 0)
                num = _menuItems[0].View.Width;
            _measureDiff = (int) (Radius * 0.200000002980232 + num);
        }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            base.OnMeasure(widthMeasureSpec, heightMeasureSpec);
            RecalculateMeasureDiff();
            SetMeasuredDimension(_imgMain.Width + Radius + _measureDiff,
                _imgMain.Height + Radius + _measureDiff);
        }

        public void Expand()
        {
            OpenItems();
        }

        public void Close()
        {
            CloseItems();
        }

        protected override IParcelable OnSaveInstanceState()
        {
            return new SavedState(base.OnSaveInstanceState())
            {
                Rotated = _rotated,
                TotalSpacingDegree = ItemsAngle,
                radius = Radius,
                MeasureDiff = _measureDiff,
                speed = Speed,
                closeItemsOnClick = CloseItemsOnClick
            };
        }

        protected override void OnRestoreInstanceState(IParcelable state)
        {
            var savedState = (SavedState) state;
            _rotated = savedState.Rotated;
            ItemsAngle = savedState.TotalSpacingDegree;
            Radius = savedState.radius;
            _measureDiff = savedState.MeasureDiff;
            Speed = savedState.speed;
            CloseItemsOnClick = savedState.closeItemsOnClick;
            base.OnRestoreInstanceState(savedState.SuperState);
        }

        private class AnimationListener : Java.Lang.Object, Android.Views.Animations.Animation.IAnimationListener
        {
            private readonly View _view;
            private readonly bool _isIn;
            private readonly Dictionary<View, SatelliteMenuButtonItem> _viewToItemMap;

            public AnimationListener(View view, bool isIn, Dictionary<View, SatelliteMenuButtonItem> viewToItemMap)
            {
                _view = view;
                _isIn = isIn;
                _viewToItemMap = viewToItemMap;
            }

            public void OnAnimationEnd(Android.Views.Animations.Animation animation)
            {
                if (_view == null)
                    return;
                var viewToItem = _viewToItemMap[_view];
                if (_isIn)
                {
                    viewToItem.View.Visibility = ViewStates.Gone;
                    viewToItem.CloneView.Visibility = ViewStates.Gone;
                }
                else
                {
                    viewToItem.CloneView.Visibility = ViewStates.Visible;
                    viewToItem.View.Visibility = ViewStates.Gone;
                }
            }

            public void OnAnimationRepeat(Android.Views.Animations.Animation animation)
            {
            }

            public void OnAnimationStart(Android.Views.Animations.Animation animation)
            {
                if (_view == null)
                    return;
                var viewToItem = _viewToItemMap[_view];
                if (_isIn)
                {
                    viewToItem.View.Visibility = ViewStates.Visible;
                    viewToItem.CloneView.Visibility = ViewStates.Gone;
                }
                else
                {
                    viewToItem.CloneView.Visibility = ViewStates.Gone;
                    viewToItem.View.Visibility = ViewStates.Visible;
                }
            }

            public new void Dispose()
            {
            }
        }

        private class ClickAnimationListener : Java.Lang.Object, Android.Views.Animations.Animation.IAnimationListener
        {
            private readonly SatelliteMenuButton _menu;
            private int _tag;

            public ClickAnimationListener(SatelliteMenuButton menu, int tag)
            {
                _menu = menu;
                _tag = tag;
            }

            public void OnAnimationEnd(Android.Views.Animations.Animation animation)
            {
            }

            public void OnAnimationRepeat(Android.Views.Animations.Animation animation)
            {
            }

            public void OnAnimationStart(Android.Views.Animations.Animation animation)
            {
                if (_menu == null || !_menu.CloseItemsOnClick)
                    return;
                _menu.Close();
            }
        }

        private class SavedState : BaseSavedState
        {
            public bool Rotated;
            public float TotalSpacingDegree;
            public int radius;
            public int MeasureDiff;
            public int speed;
            public bool closeItemsOnClick;

            public SavedState(IParcelable superState)
                : base(superState)
            {
            }

            public SavedState(Parcel parcel)
                : base(parcel)
            {
                Rotated = bool.Parse(parcel.ReadString());
                TotalSpacingDegree = parcel.ReadFloat();
                radius = parcel.ReadInt();
                MeasureDiff = parcel.ReadInt();
                speed = parcel.ReadInt();
                closeItemsOnClick = bool.Parse(parcel.ReadString());
            }

            public override int DescribeContents()
            {
                return 0;
            }

            public override void WriteToParcel(Parcel dest, ParcelableWriteFlags flags)
            {
                dest.WriteString(Rotated.ToString());
                dest.WriteFloat(TotalSpacingDegree);
                dest.WriteInt(radius);
                dest.WriteInt(MeasureDiff);
                dest.WriteInt(speed);
                dest.WriteString(closeItemsOnClick.ToString());
            }
        }
    }
}