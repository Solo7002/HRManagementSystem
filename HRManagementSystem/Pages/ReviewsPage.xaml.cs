using HRManagementSystem.DbClasses;
using HRManagementSystem.TransferClasses;
using HRManagementSystem.Translation;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace HRManagementSystem.Pages
{
    /// <summary>
    /// Interaction logic for ReviewsPage.xaml
    /// </summary>
    public partial class ReviewsPage : Page
    {
        private HrManagementDb hrDb;
        public ReviewsPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            textBlockHeader.Text = OpenTranslation.GetTranslation(LanguageTransfer.CurrentLanguage, "RPMainHeader");
            hrDb = TransferClasses.HrDbTransfer.hrManagementDb;

            StackPanelReviews.Children.Clear(); 
            foreach (Review review in CurrentUserTransfer.employee.Reviews)
            {
                StackPanel stackPanel = new StackPanel { Background = new SolidColorBrush(Color.FromRgb(68, 68, 68)), Margin = new Thickness(10)};
                TextBlock ReviewType = new TextBlock { Text = review.IsGoodReview == true ? OpenTranslation.GetTranslation(LanguageTransfer.CurrentLanguage, "RPGoodR") : OpenTranslation.GetTranslation(LanguageTransfer.CurrentLanguage, "RPBadR"), Foreground = review.IsGoodReview == true ? new SolidColorBrush(Color.FromRgb(46,204,113)) : new SolidColorBrush(Color.FromRgb(250, 83, 83)), FontSize = 20, FontWeight = FontWeights.Medium, VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(7, 3, 3, 3) };
                TextBlock ReviewText = new TextBlock { Text = review.ReviewName, Foreground = new SolidColorBrush(Colors.White), FontSize = 20, FontWeight = FontWeights.Normal, VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(7, 3, 3, 3) };
                TextBlock ReviewFrom = new TextBlock { Text = $"{OpenTranslation.GetTranslation(LanguageTransfer.CurrentLanguage, "RPFrom")}: {review.Employee1.LastName} {review.Employee1.FirstName} ({review.ReviewDate.Value.ToShortDateString()})", Foreground = new SolidColorBrush(Colors.Orange), FontSize = 20, FontWeight = FontWeights.Medium, VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Right, Margin = new Thickness(3, 3, 7, 3) };
                stackPanel.Children.Add( ReviewType );
                stackPanel.Children.Add( ReviewText );
                stackPanel.Children.Add( ReviewFrom );

                StackPanelReviews.Children.Add( stackPanel );
            }
        }
    }
}
