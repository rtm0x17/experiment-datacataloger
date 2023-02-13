using System.Windows;

namespace rtm0x17.DefectCataloger.Windows
{
    public partial class InputTextDialog : Window
    {
        private bool _canBeEmpty;
        public string UserInput { get; set; }
        public bool PressedOk { get; set; }

        public InputTextDialog(string? message, bool canBeEmpty = true)
        {
            InitializeComponent();
            LabelMessage.Content = message ?? "Input";
            _canBeEmpty = canBeEmpty;
            TextBoxInputFromUser.Focus();
        }

        private void ButtonOk_Click(object sender, RoutedEventArgs e)
        {
            if (!_canBeEmpty && string.IsNullOrWhiteSpace(TextBoxInputFromUser.Text)) {
                MessageBox.Show("Il valore non può essere vuoto");
                return;
            }

            UserInput = TextBoxInputFromUser.Text.Trim();

            PressedOk = true;
            Close();
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            PressedOk = false;
            Close();
        }
    }
}
