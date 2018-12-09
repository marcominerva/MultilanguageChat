using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Graphics;
using MultilanguageChat.Droid.Renderers;

[assembly: ExportRenderer(typeof(Label), typeof(AwesomeLabelRenderer))]
[assembly: ExportRenderer(typeof(Xamarin.Forms.Button), typeof(AwesomeButtonRenderer))]
namespace MultilanguageChat.Droid.Renderers
{
    public class AwesomeLabelRenderer : LabelRenderer
    {
        public AwesomeLabelRenderer(Context context) : base(context)
        {
            if (Control != null)
                AwesomeUtil.CheckAndSetTypeFace(Context, Control);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);
            AwesomeUtil.CheckAndSetTypeFace(Context, Control);
        }
    }

    public class AwesomeButtonRenderer : ButtonRenderer
    {
        public AwesomeButtonRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Button> e)
        {
            base.OnElementChanged(e);
            AwesomeUtil.CheckAndSetTypeFace(Context, Control);
        }
    }

    internal static class AwesomeUtil
    {
        public static void CheckAndSetTypeFace(Context context, TextView view)
        {
            var text = view.Text;
            if (text.Length == 0 || text.Length > 1 || (text.Length == 1 && text[0] < 0xf000))
            {
                return;
            }

            var font = Typeface.CreateFromAsset(context.ApplicationContext.Assets, "fontawesome-webfont.ttf");
            view.Typeface = font;
        }
    }
}