namespace Bontescu_Maria_Lab7;
using Bontescu_Maria_Lab7.Models;

public partial class ListPage : ContentPage
{
    public ListPage()
    {
        InitializeComponent();
    }

    async void OnSaveButtonClicked(object sender, EventArgs e)
    {
        try
        {
            var slist = (ShopList)BindingContext;
            slist.Date = DateTime.UtcNow;

            if (ShopPicker.SelectedItem is not Shop selectedShop)
            {
                await DisplayAlert("Eroare", "Selectati un magazin inainte de a salva lista!", "OK");
                return;
            }

            slist.ShopID = selectedShop.ID;

            await App.Database.SaveShopListAsync(slist);
            await DisplayAlert("Succes", "Lista a fost salvata cu succes!", "OK");
            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Eroare", $"A aparut o eroare la salvare: {ex.Message}", "OK");
        }
    }

    async void OnDeleteButtonClicked(object sender, EventArgs e)
    {
        try
        {
            var slist = (ShopList)BindingContext;

            if (slist == null)
            {
                await DisplayAlert("Eroare", "Nu exista o lista selectata pentru stergere.", "OK");
                return;
            }

            bool confirmDelete = await DisplayAlert(
                "Confirmare",
                "Sigur doriti sa stergeti aceasta lista?",
                "Da",
                "Nu"
            );

            if (confirmDelete)
            {
                await App.Database.DeleteShopListAsync(slist);
                await DisplayAlert("Succes", "Lista a fost stearsa cu succes.", "OK");
                await Navigation.PopAsync();
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Eroare", $"A aparut o eroare la stergere: {ex.Message}", "OK");
        }
    }

    async void OnChooseButtonClicked(object sender, EventArgs e)
    {
        try
        {
            await Navigation.PushAsync(new ProductPage((ShopList)this.BindingContext)
            {
                BindingContext = new Product()
            });
        }
        catch (Exception ex)
        {
            await DisplayAlert("Eroare", $"A aparut o eroare la navigare: {ex.Message}", "OK");
        }
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        try
        {
            var items = await App.Database.GetShopsAsync();
            ShopPicker.ItemsSource = (System.Collections.IList)items;
            ShopPicker.ItemDisplayBinding = new Binding("ShopDetails");

            var shopl = (ShopList)BindingContext;

            if (shopl != null)
            {
                listView.ItemsSource = await App.Database.GetListProductsAsync(shopl.ID);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Eroare", $"A aparut o eroare la incarcare: {ex.Message}", "OK");
        }
    }

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
