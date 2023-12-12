
using Weather_APP.Services;

namespace Weather_APP;


public partial class WeatherPage : ContentPage
{
    private double latitude;
	private double longitude;
    public List<Models.List> WeatherList;

	public WeatherPage()
	{
		InitializeComponent();
		WeatherList = new List<Models.List>();
	}

	protected async override void OnAppearing()
	{
		base.OnAppearing();
		await GetLocation();
		await GetWeatherDataByLocation(latitude, longitude);
	}

	public async Task GetLocation()
	{
		var location = await Geolocation.GetLocationAsync();
		if (location != null)
		{
			latitude = location.Latitude;
			longitude = location.Longitude;
		}
	}

	public async Task GetWeatherDataByLocation(double latitude, double longitude)
	{
        var result = await ApiService.GetWeather(latitude, longitude);
        UpdateUI(result);
    }
	private async void TapLocation_Tapped(object sender, EventArgs e)
	{
		await GetLocation();
		await GetWeatherDataByLocation(latitude, longitude);
	}

	private async void ImageButton_Clicked(object sender, EventArgs e)
	{
		var response = await DisplayPromptAsync(title: "", message: "", placeholder: "Search weather by city", accept: "", cancel: "Cencel");
		if(response != null)
		{
			await GetWeatherDataByCity(response);
		}
	}

	private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
	{
		await GetLocation();
		var result = await ApiService.GetWeather(latitude, longitude);
		UpdateUI(result);
	}

	public async Task GetWeatherDataByCity(string city)
	{
		var result = await ApiService.GetWeatherByCity(city);
		UpdateUI(result);
	}

	public void UpdateUI(dynamic result)
	{
        foreach (var item in result.list)
        {
            WeatherList.Add(item);
        }
        CvWeather.ItemsSource = WeatherList;

        LblCity.Text = result.city.name;
        LblWeatherDescription.Text = result.list[0].weather[0].description;
        LblTemperature.Text = result.list[0].main.temperature + "¡CC";
        LblHumidity.Text = result.list[0].main.humidity + "%";
        LblWind.Text = result.list[0].wind.speed + "km/h";
        ImgWeatherIcon.Source = result.list[0].weather[0].customIcon;
    }
}