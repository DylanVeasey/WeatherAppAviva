using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using AndroidX.AppCompat.App;
using Android.Views;
using System;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Aviva_Weather
{
    [Activity]
    public class MainActivity : AppCompatActivity
    {

        string FavouriteLocationOne;
        string FavouriteLocationTwo;
        string FavouriteLocationThree;

        string WeatherJSON;

        string Location;

        string response;

        bool IsFavOne = false;
        bool IsFavTwo = false;
        bool IsFavThree = false;
     
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

         
            SetContentView(Resource.Layout.home_screen);



            Button LoadSettingsScreenButton = FindViewById<Button>(Resource.Id.LoadSettingsScreenButton);
            LoadSettingsScreenButton.Click += delegate
            {
                LoadSettingsScreen();
            };

            EditText edittext = FindViewById<EditText>(Resource.Id.edittext);

                edittext.KeyPress += (object sender, View.KeyEventArgs e) =>
                {
                    e.Handled = false;
                    if (e.Event.Action == KeyEventActions.Down && e.KeyCode == Keycode.Enter)
                    {  
                        Location = edittext.Text;
                        LoadWeatherScreen(Location);
                        e.Handled = true;
                    }
                };  
        }

 

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }


        public string GetWeather(string Location)
        {
            using( var client = new WebClient())
            {
                response = client.DownloadString("https://api.openweathermap.org/data/2.5/weather?q=" + Location + "&appid=51c429c3d3d1c550a530e74e49a76b0c");
            }
            return response; 
        }

        public void LoadSettingsScreen()
        {
            SetContentView(Resource.Layout.settings);

            Button LoadHomeScreenButton = FindViewById<Button>(Resource.Id.LoadHomeScreenButton);
            LoadHomeScreenButton.Click += delegate
            {
                LoadHomeScreen();
            };
        }

        public void LoadHomeScreen()
        {
            SetContentView(Resource.Layout.home_screen);

            Button LoadSettingsScreenButton = FindViewById<Button>(Resource.Id.LoadSettingsScreenButton);
            LoadSettingsScreenButton.Click += delegate
            {
                LoadSettingsScreen();
            };

            Button FavouritesButtonOne = FindViewById<Button>(Resource.Id.FavouritesButtonOne);
            FavouritesButtonOne.Click += delegate
            {
                LoadWeatherScreen(FavouriteLocationOne);
            };

            Button FavouritesButtonTwo = FindViewById<Button>(Resource.Id.FavouritesButtonTwo);
            FavouritesButtonTwo.Click += delegate
            {
                LoadWeatherScreen(FavouriteLocationTwo);
            };

            Button FavouritesButtonThree = FindViewById<Button>(Resource.Id.FavouritesButtonThree);
            FavouritesButtonThree.Click += delegate
            {
                LoadWeatherScreen(FavouriteLocationThree);
            };

            FavouritesButtonOne.Text = FavouriteLocationOne;
            FavouritesButtonTwo.Text = FavouriteLocationTwo;
            FavouritesButtonThree.Text = FavouriteLocationThree;




            EditText edittext = FindViewById<EditText>(Resource.Id.edittext);
            edittext.KeyPress += (object sender, View.KeyEventArgs e) => {
                e.Handled = false;
                if (e.Event.Action == KeyEventActions.Down && e.KeyCode == Keycode.Enter)
                {
                    Location = edittext.Text;
                    LoadWeatherScreen(Location);
                    e.Handled = true;
                }
            };

        }
        public void LoadWeatherScreen(string Location)
        {
            SetContentView(Resource.Layout.weather_screen);
            WeatherJSON = GetWeather(Location);
            Rootobject Weather = JsonSerializer.Deserialize<Rootobject>(WeatherJSON);


            Button LoadHomeScreenButton2 = FindViewById<Button>(Resource.Id.LoadHomeScreenButton2);
            LoadHomeScreenButton2.Click += delegate
            {
                LoadHomeScreen();
            };

            TextView TemperatureText = FindViewById<TextView>(Resource.Id.TemperatureText);
            TemperatureText.Text = ConvertKelvinToCelcius(Weather.main.temp).ToString();

            TextView TemperatureTextMinMax = FindViewById<TextView>(Resource.Id.TemperatureTextMinMax);
            TemperatureTextMinMax.Text = Weather.weather[0].description;

            Button LoadFavouritesScreenButton2 = FindViewById<Button>(Resource.Id.LoadFavouritesScreenButton2);
            LoadFavouritesScreenButton2.Click += delegate
            {
                LoadFavouritesScreen(Weather);
            };
        }

        public void LoadFavouritesScreen(Rootobject Weather)
        {
            SetContentView(Resource.Layout.favourites_screen);

            Button LoadHomeScreenButon = FindViewById<Button>(Resource.Id.LoadHomeScreenButton3);
            LoadHomeScreenButon.Click += delegate
            {
                LoadHomeScreen();
            };

            TextView FavouriteLocationNameOne = FindViewById<TextView>(Resource.Id.FavouriteLocationNameOne);
            FavouriteLocationNameOne.Text = Weather.name;

            Button ConfirmFavouriteButton = FindViewById<Button>(Resource.Id.ConfirmFavouriteButton);
            ConfirmFavouriteButton.Click += delegate
            {
                AddFavourite(Weather.name);
            };
        }

        public void AddFavourite(string LocationName)
        {
          if(IsFavOne == false)
            {
                FavouriteLocationOne = LocationName;
                IsFavOne = true;
            }
          else if (IsFavTwo == false)
            {
                FavouriteLocationTwo = LocationName;
                IsFavTwo = true;
            }
          else if (IsFavThree == false)
            {
                FavouriteLocationThree = LocationName;
                IsFavThree = true;
            }
            LoadHomeScreen();
        }

        public float ConvertKelvinToCelcius(float Temperature)
        {
            return Temperature - 273.15F;
        }







    }
}