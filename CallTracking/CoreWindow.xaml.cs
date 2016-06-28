using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using CallTracking.DB_Classes;
using GroupBox = System.Windows.Controls.GroupBox;
using HorizontalAlignment = System.Windows.HorizontalAlignment;
using MessageBox = System.Windows.MessageBox;
using VerticalAlignment = System.Windows.VerticalAlignment;

namespace CallTracking
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class CoreWindow : Window
    {
        readonly CallRecordContext _context = new CallRecordContext();
        #region fields
        private readonly string _email;
        private int _recordId;
        private int _bankId;
        private int _gridHeight = 24;
        #endregion

        #region Props
        public int CurrentGridHeight
        {
            get { return (int)MainGrid.ActualHeight; }
            set { _gridHeight = value; }
        }

        public int CurrentRecordId
        {
            get
            {

                return _context.Records.First(r => r.RecordId == _recordId).RecordId;

            }
            set
            {
                try
                {
                    _context.Records.Create().RecordId = value;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        public DateTime CurrentStarTime
        {
            get { return _context.Records.First(r => r.RecordId == _recordId).StartTime; }
            set { _context.Records.First(r => r.RecordId == _recordId).StartTime = value; }
        }

        public DateTime CurrentFinishTime
        {
            get { return _context.Records.First(r => r.RecordId == _recordId).FinishTime; }
            set
            {
                try
                {
                    _context.Records.First(r => r.RecordId == _recordId).FinishTime = value;
                    //if (context.Records.Any(r => r.RecordId != null));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        public string CurrentCommentText
        {
            get { return _context.Records.First(r => r.RecordId == _recordId).Comment; }
            set { _context.Records.First(r => r.RecordId == _recordId).Comment = value; }
        }
        public int CurrentBankId
        {
            //get { return context.Banks.First(b => b.BankId == _bankId).BankId; }
            get { return _context.Banks.First(b => true).BankId; }
            set { _context.Banks.FirstOrDefault(b => b.BankId != null).BankId = value; }

        }
        #endregion


        #region Ctor

        public CoreWindow(string fullName, string email)
        {
            InitializeComponent();
            LabelForName.Content = fullName;
            _email = email;
            using (CallRecordContext db = new CallRecordContext())
            {
                foreach (var curItem in db.Banks.Where(b => b.Configs.Any(c => c.EmailAdress == _email)))
                {
                    DrawSelectedBank(curItem.BankName, _gridHeight, true);
                    _gridHeight += 72;
                    _bankId = curItem.BankId;
                }
            }
        }
        #endregion
        // вчаспмриотjksdf
        private void CommentTextBoxOnTextChanged(object sender, TextChangedEventArgs textChangedEventArgs)
        {
            var commentTextBox = sender as TextBox;
            CurrentCommentText = commentTextBox.Text;
        }

        private void DrawSelectedBank(string bankName, int top, bool flag)
        {
            int topMarg;
            if (flag)
            {
                topMarg = top;
            }
            else
            {
                topMarg = CurrentGridHeight;
            }

            #region groupBox
            var gb = new GroupBox
            {
                Header = bankName,
                Height = 72,
                Margin = new Thickness(0, topMarg, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Top
            };
            MainGrid.Children.Add(gb);
            #endregion
            #region startButton
            var startButton = new Button
            {
                Content = "Начало звонка",
                Width = 108,
                Height = 24,
                Margin = new Thickness(12, 28 + topMarg, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top
            };
            MainGrid.Children.Add(startButton);
            #endregion
            #region finishButton
            var finishButton = new Button
            {
                Content = "Окончание звонка",
                Width = 108,
                Height = 24,
                Margin = new Thickness(132, 28 + topMarg, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top
            };
            MainGrid.Children.Add(finishButton);
            #endregion
            #region durationLabel
            var durationLabel = new Label
            {
                Content = "Продолжительность:",
                Height = 24,
                Margin = new Thickness(252, 28 + topMarg, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top
            };
            MainGrid.Children.Add(durationLabel);
            #endregion
            #region commentLabel
            var commentLabel = new Label
            {
                Content = "Комментарий",
                Height = 24,
                Margin = new Thickness(0, 28 + topMarg, 494, 0),
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Top
            };
            MainGrid.Children.Add(commentLabel);
            #endregion
            #region commentTextBox
            var commentTextBox = new TextBox
            {
                Text = String.Empty,
                Width = 360,
                Height = 24,
                Margin = new Thickness(0, 28 + topMarg, 132, 0),
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Top
            };
            MainGrid.Children.Add(commentTextBox);
            #endregion
            #region commentButton
            var commentButton = new Button
            {
                Content = "Отправить",
                Width = 108,
                Height = 24,
                Margin = new Thickness(0, 28 + topMarg, 12, 0),
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Top
            };
            MainGrid.Children.Add(commentButton);
            #endregion

            commentTextBox.TextChanged += CommentTextBoxOnTextChanged;
            startButton.Click += StartButtonOnClick;
            finishButton.Click += FinishButtonOnClick;
            commentButton.Click += CommentButtonOnClick;
        }

        private void StartButtonOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            using (CallRecordContext db = new CallRecordContext())
            {
                Record record = new Record();
                record.StartTime = DateTime.Now;
                record.Config = db.Configs.Single(c => c.EmailAdress == _email);
                record.Bank = db.Banks.Single(b => b.BankId == CurrentBankId);
                db.Records.Add(record);
                db.SaveChanges();
                _recordId = record.RecordId;
                CurrentRecordId = _recordId;
            }
        }

        private void FinishButtonOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            using (CallRecordContext db = new CallRecordContext())
            {
                CurrentFinishTime = DateTime.Now;
                db.Records.First(r => r.RecordId == CurrentRecordId).FinishTime = CurrentFinishTime;
                db.Records.First(r => r.RecordId == CurrentRecordId).Duration = CurrentFinishTime - CurrentStarTime;
                db.SaveChanges();
            }
        }

        private void CommentButtonOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            using (CallRecordContext db = new CallRecordContext())
            {
                db.Records.First(r => r.RecordId == CurrentRecordId).Comment = CurrentCommentText;
                db.SaveChanges();
            }
            MessageBox.Show("Запись занесена в базу данных");
            CurrentCommentText = String.Empty;
        }

        private void AddBankButton_OnClick(object sender, RoutedEventArgs e)
        {
            var bankWindow = new BankListWindow();
            bankWindow.ShowDialog();

            using (var db = new CallRecordContext())
            {
                var choise = (string)bankWindow.ComboBox.SelectionBoxItem;
                if (choise != String.Empty && bankWindow.DialogResult == true)
                {
                    try
                    {
                        var configSingle = db.Configs.Single(c => c.EmailAdress == _email);
                        var bankSingle = db.Banks.Single(b => b.BankName == choise);
                        configSingle.Banks = new[] { bankSingle };
                        db.SaveChanges();
                        DrawSelectedBank(choise, CurrentGridHeight, false);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message + "\r\n\r\n" + "Этот банк уже в вашей конфигурации!");
                    }
                }
            }
        }

        private void EraseSelectedBank(string bankName)
        {
            #region groupBox
            var topMarg = CurrentGridHeight;
            var gb = new GroupBox
            {
                Header = bankName,
                Height = 72,
                Margin = new Thickness(0, topMarg, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Top
            };
            MainGrid.Children.Remove(gb);
            #endregion
            #region startButton
            var startButton = new Button
            {
                Content = "Начало звонка",
                Width = 108,
                Height = 24,
                Margin = new Thickness(12, 28 + topMarg, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top
            };
            MainGrid.Children.Remove(startButton);
            #endregion
            #region finishButton
            var finishButton = new Button
            {
                Content = "Окончание звонка",
                Width = 108,
                Height = 24,
                Margin = new Thickness(132, 28 + topMarg, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top
            };
            MainGrid.Children.Remove(finishButton);
            #endregion
            #region durationLabel
            var durationLabel = new Label
            {
                Content = "Продолжительность:",
                Height = 24,
                Margin = new Thickness(252, 28 + topMarg, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top
            };
            MainGrid.Children.Remove(durationLabel);
            #endregion
            #region commentLabel
            var commentLabel = new Label
            {
                Content = "Комментарий",
                Height = 24,
                Margin = new Thickness(0, 28 + topMarg, 494, 0),
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Top
            };
            MainGrid.Children.Remove(commentLabel);
            #endregion
            #region commentTextBox
            var commentTextBox = new TextBox
            {
                Text = String.Empty,
                Width = 360,
                Height = 24,
                Margin = new Thickness(0, 28 + topMarg, 132, 0),
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Top
            };
            MainGrid.Children.Remove(commentTextBox);
            #endregion
            #region commentButton
            var commentButton = new Button
            {
                Content = "Отправить",
                Width = 108,
                Height = 24,
                Margin = new Thickness(0, 28 + topMarg, 12, 0),
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Top
            };
            MainGrid.Children.Remove(commentButton);
            #endregion
            MainGrid.UpdateLayout();
        }

        private void DeleteBankButton_Click(object sender, RoutedEventArgs e)
        {

            using (var db = new CallRecordContext())
            {
                var bankWindow = new BankListWindow();
                //bankWindow.ComboBox.ItemsSource = db.Banks.Include("Configs").Where(b => b.Configs.All(c => c.EmailAdress == _email));
                bankWindow.ComboBox.ItemsSource = db.Banks.Local.ToList();
                bankWindow.ShowDialog();
                var choise = (string)bankWindow.ComboBox.SelectionBoxItem;
                if (choise != String.Empty && bankWindow.DialogResult == true)
                {
                    try
                    {
                        var configSingle = db.Configs.Include("Banks").Single(c => c.EmailAdress == _email);
                        var bankSingle = db.Banks.First(b => b.BankName == choise);
                        configSingle.Banks.Remove(bankSingle);
                        db.SaveChanges();
                        EraseSelectedBank(choise);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        public event EventHandler Drawer;
        protected virtual void OnDrawer(object sender, RoutedEventArgs routedEventArgs)
        {
            var handler = sender as UIElement;
        }

        //private void DrawStartConfig()
        //{
        //    using (var db = new CallRecordContext())
        //    {
        //        var query = db.Banks.Where(b => b.Configs.Any(c => c.EmailAdress == Email));
        //        var i = 0;
        //        var list = query.ToList();
        //        foreach (var curItem in list)
        //        {
        //            #region groupBox
        //            var groupBox = new GroupBox();
        //            groupBox.Header = curItem.BankName;
        //            groupBox.Height = 72;
        //            groupBox.Margin = new Thickness(0, 24 + 72 * i, 0, 0);
        //            groupBox.HorizontalAlignment = HorizontalAlignment.Stretch;
        //            groupBox.VerticalAlignment = VerticalAlignment.Top;

        //            MainGrid.Children.Add(groupBox);
        //            #endregion
        //            #region startButton
        //            var startButton = new Button
        //            {
        //                Content = "Начало звонка",
        //                Width = 108,
        //                Height = 24,
        //                Margin = new Thickness(12, 52 + 72 * i, 0, 0),
        //                HorizontalAlignment = HorizontalAlignment.Left,
        //                VerticalAlignment = VerticalAlignment.Top

        //            };
        //            MainGrid.Children.Add(startButton);
        //            #endregion
        //            #region finishButton
        //            var finishButton = new Button
        //            {
        //                Content = "Окончание звонка",
        //                Width = 108,
        //                Height = 24,
        //                Margin = new Thickness(132, 52 + 72 * i, 0, 0),
        //                HorizontalAlignment = HorizontalAlignment.Left,
        //                VerticalAlignment = VerticalAlignment.Top
        //            };
        //            MainGrid.Children.Add(finishButton);
        //            #endregion
        //            #region durationLabel
        //            var durationLabel = new Label
        //            {
        //                Content = "Продолжительность:",
        //                Height = 24,
        //                Margin = new Thickness(252, 52 + 72 * i, 0, 0),
        //                HorizontalAlignment = HorizontalAlignment.Left,
        //                VerticalAlignment = VerticalAlignment.Top
        //            };
        //            MainGrid.Children.Add(durationLabel);

        //            #endregion
        //            #region commentLabel
        //            var commentLabel = new Label
        //            {
        //                Content = "Комментарий:",
        //                Height = 24,
        //                Margin = new Thickness(0, 52 + 72 * i, 494, 0),
        //                HorizontalAlignment = HorizontalAlignment.Right,
        //                VerticalAlignment = VerticalAlignment.Top
        //            };
        //            MainGrid.Children.Add(commentLabel);
        //            #endregion
        //            #region commentTextBox
        //            var commentTextBox = new TextBox
        //            {
        //                Text = String.Empty,
        //                Width = 360,
        //                Height = 24,
        //                Margin = new Thickness(0, 52 + 72 * i, 132, 0),
        //                HorizontalAlignment = HorizontalAlignment.Right,
        //                VerticalAlignment = VerticalAlignment.Top
        //            };
        //            MainGrid.Children.Add(commentTextBox);
        //            #endregion
        //            #region commentButton
        //            var commentButton = new Button
        //            {
        //                Content = "Отправить",
        //                Width = 108,
        //                Height = 24,
        //                Margin = new Thickness(0, 52 + 72 * i, 12, 0),
        //                HorizontalAlignment = HorizontalAlignment.Right,
        //                VerticalAlignment = VerticalAlignment.Top,
        //            };
        //            MainGrid.Children.Add(commentButton);
        //            #endregion
        //            _bankId = curItem.BankId;
        //            //bankIds[i] = curItem.BankId;
        //            startButton.Click += StartButtonOnClick;
        //            startButton.Click += dispatcherTimer_Tick;
        //            finishButton.Click += FinishButtonOnClick;
        //            commentTextBox.TextChanged += CommentTextBoxOnTextChanged;
        //            commentButton.Click += CommentButtonOnClick;
        //            i++;
        //        }
        //    }
        //}
    }
}
