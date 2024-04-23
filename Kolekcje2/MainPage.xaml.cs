using System;
using System.Diagnostics;
namespace Kolekcje2
{
    public partial class MainPage : ContentPage
    {
        Grid mainGrid;

         public MainPage()
         {
             InitializeComponent();
             LoadTextFiles();
             Debug.WriteLine("DANE APLIKACJI: " + Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)+"\\Kolekcje");
         }
        private void LoadTextFiles()
        {
            string driveLetter = "C:\\Users\\Andrii\\AppData\\Roaming\\Kolekcje";
            string[] txtFiles = Directory.GetFiles(driveLetter, "*.txt");

            mainGrid = new Grid();
            mainGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); 
            mainGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); 

            StackLayout dynamicButtonsLayout = new StackLayout();
            if (!Directory.Exists(driveLetter))
            {
                Directory.CreateDirectory(driveLetter);
            }


            foreach (string filePath in txtFiles)
            {
                string fileName = Path.GetFileName(filePath);
                Button fileButton = new Button
                {
                    Text = fileName,
                    Margin = new Thickness(5)
                };
                fileButton.Clicked += (sender, e) =>
                {
                    OpenCollection(filePath);
                };

                dynamicButtonsLayout.Children.Add(fileButton);
            }

            Grid.SetRow(dynamicButtonsLayout, 0);

            mainGrid.Children.Add(dynamicButtonsLayout);

            Button fixedButton = new Button
            {
                FontFamily = "Bahnschrift",
                FontSize = 20,
                Text = "Nowa Kolekcja",
                Margin = new Thickness(5),
                BackgroundColor = Color.FromHex("#FF0000"), 
                TextColor = Color.FromHex("#000000") 
            };
            fixedButton.Clicked += OnNewCollection;

            Button restartButton = new Button
            {
                FontFamily = "Bahnschrift",
                FontSize = 20,
                Text = "Odśwież",
                Margin = new Thickness(5),
                BackgroundColor = Color.FromHex("#FF0000"),
                TextColor = Color.FromHex("#000000")
            };
            restartButton.Clicked += OnRestart;

            StackLayout buttonLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.Center,
                Children =
    {
        fixedButton,
        restartButton
    }
            };
            Grid.SetRow(buttonLayout, 1);
            mainGrid.Children.Add(buttonLayout);

            Content = mainGrid;
        }

        private async void OpenCollection(string filePath)
        {
            string text = File.ReadAllText(filePath);

            string selectedOption = await DisplayActionSheet("Wybierz Działanie", "Anuluj", null, "Wyświetl kolekcję", "Edytuj kolekcję", "Usuń");

            switch (selectedOption)
            {
                case "Wyświetl kolekcję":
                    DisplayAlert("Lista elementów kolekcji", $"Elementy kolekcji: '{Path.GetFileName(filePath)}':\n{text}", "OK");
                    break;
                case "Edytuj kolekcję":
                    await Navigation.PushAsync(new NewPage1(filePath));
                    break;
                case "Usuń":
                    Usun(filePath);
                    break;
                default:
                    //await DisplayAlert("ERROR!", "Zaznaczyłeś złą opcję", "OK");
                    break;
            }
        }
        public void OnRestart(object sender, EventArgs e)
        {
            Content = null;
            LoadTextFiles(); 
        }
        private void Usun(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                DisplayAlert("Komunikat", "Kolekcja została usunięta pomyślnie!", "OK");
            }
            else
            {
                DisplayAlert("ERROR!", "Żądana kolekcja nie została znaleziona!", "OK");
            }
        }

        private async void OnNewCollection(object sender, EventArgs e)
        {
            string collName = await DisplayPromptAsync("Nowa kolekcja", "Wpisz nazwę kolekcji: ", "Dodaj", "Anuluj");
            if (collName != null)
            {
                string driveletter = "C:\\Users\\Andrii\\AppData\\Roaming\\Kolekcje";
                string filename = collName + ".txt";
                string filePath = Path.Combine(driveletter, filename);
                DisplayAlert("Utworzono kolekcje ", $"{filename}", "OK");
                File.WriteAllText(filePath, "");
            }
        }

    }
}




