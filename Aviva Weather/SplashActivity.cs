using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aviva_Weather
{
    [Activity(MainLauncher = true)]
    public class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.splash_screen);

        }

        protected override async void OnResume()
        {
            base.OnResume();
            await SimulateStartup();
        }

        async private Task SimulateStartup()
        {
            await Task.Delay(TimeSpan.FromSeconds(4));
            StartActivity(new Android.Content.Intent(Application.Context, typeof(MainActivity)));
        }
    }
}