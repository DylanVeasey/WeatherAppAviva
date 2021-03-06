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

        string[] FavouriteLocations = new string[3];

        string WeatherJSON;

        string Location;

        string response;

        Rootobject Weather;

        bool IsFavOne = false;
        bool IsFavTwo = false;
        bool IsFavThree = false;
     
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

         
            SetContentView(Resource.Layout.home_screen);
            
            
            //When the user clicks on the settings button, the systems loads the settings screen
            Button LoadSettingsScreenButton = FindViewById<Button>(Resource.Id.LoadSettingsScreenButton);
            LoadSettingsScreenButton.Click += delegate
            {
                LoadSettingsScreen();
            };

            
            EditText LocationInputFromUser = FindViewById<EditText>(Resource.Id.edittext);
                LocationInputFromUser.KeyPress += (object sender, View.KeyEventArgs e) =>
                {
                    e.Handled = false;
                    //Checks to see if the user has pressed the enter button, if they have, then we can take the location from the text box
                    if (e.Event.Action == KeyEventActions.Down && e.KeyCode == Keycode.Enter)
                    {  
                        Location = LocationInputFromUser.Text;
                        WeatherJSON = GetWeather(Location);
                        if (WeatherJSON == "error")
                        {
                            //If the API returned an error, the user is returned to the home screen and alearted that the location they entered does not exist
                            LoadHomeScreen();
                            Toast.MakeText(Application.Context, "No Location with this name!", ToastLength.Short).Show();
                        }
                        else
                        {
                            LoadWeatherScreen(WeatherJSON);
                        }
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
        //Try's to pull data from the API, if it fails, then it will return an error, else it will return the API response
            using( var client = new WebClient())
            {
                try
                {
                    response = client.DownloadString("https://api.openweathermap.org/data/2.5/weather?q=" + Location + "&appid=51c429c3d3d1c550a530e74e49a76b0c");
                }
                catch
                {
                    return "error";
                }
            }
            return response; 
        }

        public void LoadSettingsScreen()
        {
            
            SetContentView(Resource.Layout.settings);

            //When the user clicks on the Back button, the system loads the home screen
            Button LoadHomeScreenButton = FindViewById<Button>(Resource.Id.LoadHomeScreenButton);
            LoadHomeScreenButton.Click += delegate
            {
                LoadHomeScreen();
            };
        }

        public void LoadHomeScreen()
        {
            SetContentView(Resource.Layout.home_screen);

            //When the user clicks the Settings button, the system loads the settings screen
            Button LoadSettingsScreenButton = FindViewById<Button>(Resource.Id.LoadSettingsScreenButton);
            LoadSettingsScreenButton.Click += delegate
            {
                LoadSettingsScreen();
            };
            
            
            //When the user clicks the Favourite location button, the system loads the weather screen for the corresponding favourite location
            Button FavouritesButtonOne = FindViewById<Button>(Resource.Id.FavouritesButtonOne);
            FavouritesButtonOne.Click += delegate
            {
                LoadWeatherScreen(GetWeather(FavouriteLocations[0]));
            };
            
            //When the user clicks the Favourite location button, the system loads the weather screen for the corresponding favourite location
            Button FavouritesButtonTwo = FindViewById<Button>(Resource.Id.FavouritesButtonTwo);
            FavouritesButtonTwo.Click += delegate
            {
                LoadWeatherScreen(GetWeather(FavouriteLocations[1]));
            };

            //When the user clicks the Favourite location button, the system loads the weather screen for the corresponding favourite location
            Button FavouritesButtonThree = FindViewById<Button>(Resource.Id.FavouritesButtonThree);
            FavouritesButtonThree.Click += delegate
            {
                LoadWeatherScreen(GetWeather(FavouriteLocations[2]));
            };
            
            //Sets the text values of the buttons to be equal to the favourite locations
            FavouritesButtonOne.Text = FavouriteLocations[0];
            FavouritesButtonTwo.Text = FavouriteLocations[1];
            FavouritesButtonThree.Text = FavouriteLocations[2];

            EditText LocationInputFromUser = FindViewById<EditText>(Resource.Id.edittext);
            LocationInputFromUser.KeyPress += (object sender, View.KeyEventArgs e) => {
                e.Handled = false;
                //Checks to see if the user has pressed the enter button, if they have, then the system can take the location from the text box
                if (e.Event.Action == KeyEventActions.Down && e.KeyCode == Keycode.Enter)
                {
                    Location = LocationInputFromUser.Text;
                    WeatherJSON = GetWeather(Location);
                    if (WeatherJSON == "error")
                    {
                        //If the API returned an error, the user is returned to the home screen and alearted that the location they entered does not exist
                        LoadHomeScreen();
                        Toast.MakeText(Application.Context, "No Location with this name!", ToastLength.Short).Show();
                    }
                    else
                    {
                        LoadWeatherScreen(WeatherJSON);
                    }
                    e.Handled = true;
                }
            };
        }
        
        public void LoadWeatherScreen(string WeatherJSON)
        {
            SetContentView(Resource.Layout.weather_screen);
    
            //Converts the JSON string into an object using the JsonSerializer library
            Weather = JsonSerializer.Deserialize<Rootobject>(WeatherJSON);
            
            //When the user clicks the back button, the system loads the home screen.
            Button LoadHomeScreenButton2 = FindViewById<Button>(Resource.Id.LoadHomeScreenButton);
            LoadHomeScreenButton2.Click += delegate
            {
                LoadHomeScreen();
            };
            
            //Displays the value of temperature, rounded to the nearest integer
            TextView TemperatureText = FindViewById<TextView>(Resource.Id.TemperatureText);
            TemperatureText.Text = Math.Round(ConvertKelvinToCelcius(Weather.main.temp)).ToString()  + " Degrees ";

            //Displays the description of the weather
            TextView TemperatureTextDescription = FindViewById<TextView>(Resource.Id.TemperatureTextMinMax);
            TemperatureTextDescription.Text = Weather.weather[0].description;

            //When the user clicks the Add to favourites button, the system loads the favourite screen
            Button LoadFavouritesScreenButton2 = FindViewById<Button>(Resource.Id.LoadFavouritesScreenButton);
            LoadFavouritesScreenButton2.Click += delegate
            {
                LoadFavouritesScreen(Weather);
            };
        }

        public void LoadFavouritesScreen(Rootobject Weather)
        {
            SetContentView(Resource.Layout.favourites_screen);

            //When the user clicks the back button, the system loads the home screen
            Button LoadHomeScreenButon = FindViewById<Button>(Resource.Id.LoadHomeScreenButton);
            LoadHomeScreenButon.Click += delegate
            {
                LoadHomeScreen();
            };

            //Displays the name of the location on the screen
            TextView FavouriteLocationNameOne = FindViewById<TextView>(Resource.Id.FavouriteLocationNameOne);
            FavouriteLocationNameOne.Text = Weather.name;

            //When the user clicks on the confirm button, the system adds the location to the list of favourties
            Button ConfirmFavouriteButton = FindViewById<Button>(Resource.Id.ConfirmFavouriteButton);
            ConfirmFavouriteButton.Click += delegate
            {
                AddFavourite(Weather.name);
            };
        }

        public void AddFavourite(string LocationName)
        {
            //Checks to see if there is already a value for the favourite and adds sets the favourite location if it is not already set.
          if(IsFavOne == false)
            {
                FavouriteLocations[0] = LocationName;
                IsFavOne = true;
            }
          else if (IsFavTwo == false)
            {
                FavouriteLocations[1] = LocationName;
                IsFavTwo = true;
            }
          else if (IsFavThree == false)
            {
                FavouriteLocations[2] = LocationName;
                IsFavThree = true;
            }
            LoadHomeScreen();
        }

        public float ConvertKelvinToCelcius(float Temperature)
        {
        //To convert from kelvin to celcius, minus 273.15
            return Temperature - 273.15F;
        }
    }
}
