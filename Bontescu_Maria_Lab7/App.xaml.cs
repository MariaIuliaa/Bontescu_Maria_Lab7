using System;
using System.IO;
using Bontescu_Maria_Lab7.Data;

namespace Bontescu_Maria_Lab7
{
    public partial class App : Application
    {
        // Singleton pentru baza de date
        private static ShoppingListDatabase database;

        // Proprietatea statică pentru accesarea bazei de date
        public static ShoppingListDatabase Database
        {
            get
            {
                if (database == null)
                {
                    // Creăm baza de date cu calea către directorul local al aplicației
                    database = new ShoppingListDatabase(Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                        "ShoppingList.db3"));
                }
                return database;
            }
        }

        // Constructorul aplicației
        public App()
        {
            InitializeComponent();
            // Setăm pagina principală a aplicației
            MainPage = new AppShell();
        }
    }
}
