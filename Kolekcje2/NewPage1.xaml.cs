namespace Kolekcje2;

public partial class NewPage1 : ContentPage
{
    public string _filePath;
    public int itemIdCounter = 0;
    public NewPage1(string filePath)
	{
		InitializeComponent();
        _filePath = filePath;

        List<string> lines = ReadTextFile(filePath);

        foreach (string line in lines)
        {
            Button button = new Button
            {
                Text = line,
                Margin = new Thickness(10),
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            button.Clicked += (sender, e) =>
            {
                OpenItem(filePath);
                OpenItem(line);
            };
            stackLayout.Children.Add(button);
        }

    }
    public async void OpenItem(string itemName)
    {
        try
        {
            string selectedOption = await DisplayActionSheet("Wybierz Dzia³anie", "Anuluj", null, "Usuñ przedmiot");

            switch (selectedOption)
            {
                case "Usuñ przedmiot":
                    DeleteItem(itemName);
                    break;
                default:
                    //await DisplayAlert("ERROR!", "Zaznaczy³eœ z³¹ opcjê", "OK");
                    break;
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("ERROR!", $"An error occurred: {ex.Message}", "OK");
        }
    }
    private List<string> ReadTextFile(string filePath)
    {
        List<string> lines = new List<string>();
        if (File.Exists(filePath))
        {
            lines.AddRange(File.ReadAllLines(filePath));
        }
        return lines;
    }

    private List<Item> _items = new List<Item>();

    public class Item
    {
        public int LineNumber { get; set; }
        public string Name { get; set; }
    }
    public void DeleteItem(string itemName)
    {
        List<string> lines = File.ReadAllLines(_filePath).ToList();

        int index = lines.FindIndex(line => line.Contains(itemName));

        if (index != -1)
        {
            lines.RemoveAt(index);

            File.WriteAllLines(_filePath, lines);

            DisplayAlert("Komunikat", "Przedmiot zosta³ pomyœlnie usuniêty", "OK");
        }
        else
        {
            DisplayAlert("Komunikat", "Nie mo¿na odnaleŸæ przedmiotu", "OK");
        }

    }
    public void OnRestart(object sender, EventArgs e)
    {
        Navigation.PopAsync();
        Navigation.PushAsync(new NewPage1(_filePath)); 
    }
    public async void OnNewItem(object sender, EventArgs e)
    {
        try
        {
            string entry = await DisplayPromptAsync("Nowy Przedmiot", "Wpisz nazwê:", "OK", "Cancel");

            if (!string.IsNullOrEmpty(entry))
            {
                string[] lines = File.ReadAllLines(_filePath);

                int lineNumber = 1;
                while (lines.Any(line => line.StartsWith($"{lineNumber}.")))
                {
                    lineNumber++;
                }
                string newItemLine = $"{lineNumber}. {entry}";

                File.AppendAllText(_filePath, newItemLine + Environment.NewLine);

                await DisplayAlert("Sukces!", "Nowy przedmiot dodany pomyœlnie", "OK");
            }
            else
            {
                await DisplayAlert("ERROR!", "WprowadŸ poprawn¹ nazwê przedmiotu.", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("ERROR!", $"An error occurred: {ex.Message}", "OK");
        }
    }
    public void Wroc(object sender, EventArgs e)
    {
        Navigation.PushAsync(new MainPage());
    }

}
