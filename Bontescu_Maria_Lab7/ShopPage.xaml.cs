namespace Bontescu_Maria_Lab7;

using Bontescu_Maria_Lab7.Models;
using Plugin.LocalNotification;

public partial class ShopPage : ContentPage
{
    public ShopPage()
    {
        InitializeComponent();
    }

    // Salvare magazin
    async void OnSaveButtonClicked(object sender, EventArgs e)
    {
        var shop = (Shop)BindingContext;
        if (shop == null || string.IsNullOrWhiteSpace(shop.ShopName))
        {
            await DisplayAlert("Eroare", "Numele magazinului este obligatoriu!", "OK");
            return;
        }

        await App.Database.SaveShopAsync(shop);
        await Navigation.PopAsync();
    }

    // Afisare locatie pe harta
    async void OnShowMapButtonClicked(object sender, EventArgs e)
    {
        try
        {
            var shop = (Shop)BindingContext;
            if (shop == null || string.IsNullOrWhiteSpace(shop.Adress))
            {
                await DisplayAlert("Eroare", "Adresa magazinului nu este specificata!", "OK");
                return;
            }

            var address = shop.Adress;
            var locations = await Geocoding.GetLocationsAsync(address);
            var shopLocation = locations?.FirstOrDefault();

            if (shopLocation != null)
            {
                var options = new MapLaunchOptions
                {
                    Name = shop.ShopName ?? "Magazinul meu preferat"
                };

                await Map.OpenAsync(shopLocation, options);
            }
            else
            {
                await DisplayAlert("Eroare", "Nu am putut gasi locatia pentru aceasta adresa.", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Eroare", $"A aparut o eroare: {ex.Message}", "OK");
        }
    }

    // Stergere magazin
    async void OnDeleteShopButtonClicked(object sender, EventArgs e)
    {
        try
        {
            var shop = (Shop)BindingContext;

            if (shop == null)
            {
                await DisplayAlert("Eroare", "Nu exista niciun magazin selectat pentru stergere.", "OK");
                return;
            }

            bool confirmDelete = await DisplayAlert(
                "Confirmare",
                $"Sigur doriti sa stergeti magazinul \"{shop.ShopName}\"?",
                "Da",
                "Nu"
            );

            if (confirmDelete)
            {
                await App.Database.DeleteShopAsync(shop);
                await DisplayAlert("Succes", "Magazinul a fost sters cu succes.", "OK");
                await Navigation.PopAsync();
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Eroare", $"A aparut o eroare: {ex.Message}", "OK");
        }
    }
}
