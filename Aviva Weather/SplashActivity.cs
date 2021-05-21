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
        
        //Keeps the splash screen open for 4 seconds and then starts the main activity file
        async private Task SimulateStartup()
        {
            //Keeps the home screen open for four seconds to simulate the startup
            await Task.Delay(TimeSpan.FromSeconds(4));
            //Loads the main activity file
            StartActivity(new Android.Content.Intent(Application.Context, typeof(MainActivity)));
        }
    }
}
