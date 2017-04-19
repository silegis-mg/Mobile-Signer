using System;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;
using Android.Graphics.Drawables.Shapes;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Util;

namespace Almg.MobileSigner.Droid.Renderers
{
    public class Badge : TextView
    {
        public enum BadgePosition
        {
            PositionTopLeft = 1,
            PositionTopRight = 2,
            PositionBottomLeft = 3,
            PositionBottomRight = 4,
            PositionCenter = 5
        }

        private const int DefaultHmarginDip = -12;
        private const int DefaultVmarginDip = -8;
        private const int DefaultLrPaddingDip = 4;
        private const int DefaultCornerRadiusDip = 7;

        private static Animation fadeInAnimation;
        private static Animation fadeOutAnimation;

        private Context context;
        private readonly Color defaultBadgeColor = Color.ParseColor("#CCFF0000");
        private ShapeDrawable backgroundShape;

        public View Target { get; private set; }
        public BadgePosition Postion { get; set; } = BadgePosition.PositionTopRight;
        public int BadgeMarginH { get; set; }
        public int BadgeMarginV { get; set; }

        public static int TextSizeDip { get; set; } = 11;

        public Color BadgeColor
        {
            get { return backgroundShape.Paint.Color; }
            set
            {
                backgroundShape.Paint.Color = value;
                Background.InvalidateSelf();
            }
        }

        public Color TextColor
        {
            get { return new Color(CurrentTextColor); }
            set { SetTextColor(value); }
        }

        public Badge(Context context, View target) : this(context, null, Android.Resource.Attribute.TextViewStyle, target)
        {
        }

        public Badge(Context context, IAttributeSet attrs, int defStyle, View target) : base(context, attrs, defStyle)
        {
            Init(context, target);
        }

        private void Init(Context context, View target)
        {
            this.context = context;
            Target = target;

            BadgeMarginH = DipToPixels(DefaultHmarginDip);
            BadgeMarginV = DipToPixels(DefaultVmarginDip);

            Typeface = Typeface.DefaultBold;
            var paddingPixels = DipToPixels(DefaultLrPaddingDip);
            SetPadding(paddingPixels, 0, paddingPixels, 0);
            SetTextColor(Color.White);
            SetTextSize(ComplexUnitType.Dip, TextSizeDip);

            fadeInAnimation = new AlphaAnimation(0, 1)
            {
                Interpolator = new DecelerateInterpolator(),
                Duration = 200
            };

            fadeOutAnimation = new AlphaAnimation(1, 0)
            {
                Interpolator = new AccelerateInterpolator(),
                Duration = 200
            };

            backgroundShape = CreateBackgroundShape();
            Background = backgroundShape;
            BadgeColor = defaultBadgeColor;

            if (Target != null)
            {
                ApplyTo(Target);
            }
            else
            {
                Show();
            }
        }

        private ShapeDrawable CreateBackgroundShape()
        {
            var radius = DipToPixels(DefaultCornerRadiusDip);
            var outerR = new float[] { radius, radius, radius, radius, radius, radius, radius, radius };

            return new ShapeDrawable(new RoundRectShape(outerR, null, null));
        }

        private void ApplyTo(View target)
        {
            var lp = target.LayoutParameters;
            var parent = target.Parent;

            var group = parent as ViewGroup;
            if (group == null)
            {
                return;
            }

            group.SetClipChildren(false);
            group.SetClipToPadding(false);


            var container = new FrameLayout(this.context);
            var index = group.IndexOfChild(target);

            group.RemoveView(target);
            group.AddView(container, index, lp);

            container.AddView(target);
            group.Invalidate();

            Visibility = ViewStates.Gone;
            container.AddView(this);
        }

        public void Show()
        {
            Show(false, null);
        }

        public void Show(bool animate)
        {
            Show(animate, fadeInAnimation);
        }


        public void Hide(bool animate)
        {
            Hide(animate, fadeOutAnimation);
        }

        private void Show(bool animate, Animation anim)
        {
            ApplyLayoutParams();

            if (animate)
            {
                StartAnimation(anim);
            }

            Visibility = ViewStates.Visible;

        }

        private void Hide(bool animate, Animation anim)
        {
            Visibility = ViewStates.Gone;
            if (animate)
            {
                StartAnimation(anim);
            }
        }

        private void ApplyLayoutParams()
        {
            var layoutParameters = new FrameLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);

            switch (Postion)
            {
                case BadgePosition.PositionTopLeft:
                    layoutParameters.Gravity = GravityFlags.Left | GravityFlags.Top;
                    layoutParameters.SetMargins(BadgeMarginH, BadgeMarginV, 0, 0);
                    break;
                case BadgePosition.PositionTopRight:
                    layoutParameters.Gravity = GravityFlags.Right | GravityFlags.Top;
                    layoutParameters.SetMargins(0, BadgeMarginV, BadgeMarginH, 0);
                    break;
                case BadgePosition.PositionBottomLeft:
                    layoutParameters.Gravity = GravityFlags.Left | GravityFlags.Bottom;
                    layoutParameters.SetMargins(BadgeMarginH, 0, 0, BadgeMarginV);
                    break;
                case BadgePosition.PositionBottomRight:
                    layoutParameters.Gravity = GravityFlags.Right | GravityFlags.Bottom;
                    layoutParameters.SetMargins(0, 0, BadgeMarginH, BadgeMarginV);
                    break;
                case BadgePosition.PositionCenter:
                    layoutParameters.Gravity = GravityFlags.Center;
                    layoutParameters.SetMargins(0, 0, 0, 0);
                    break;
            }

            LayoutParameters = layoutParameters;
        }

        private int DipToPixels(int dip)
        {
            return (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, dip, Resources.DisplayMetrics);
        }

        public new string Text
        {
            get { return base.Text; }
            set
            {
                Hide(false);
                base.Text = value;

                if (Visibility == ViewStates.Gone && !string.IsNullOrEmpty(value))
                {
                    Show(true);
                }
            }
        }
    }
}