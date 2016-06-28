using System;
using System.Data.Entity;
using System.Diagnostics;
using System.DirectoryServices;
using System.Linq;
using System.Reflection;
using System.Windows;
using CallTracking.DB_Classes;
using Microsoft.Win32;

namespace CallTracking
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private readonly string _ldappath = "LDAP://local.st.by/DC=local,DC=st,DC=by";
        public MainWindow()
        {
            InitializeComponent();
            AutoLoad();
        }

        #region AD
        private DirectoryServicesUserInfo GetDirectoryServicesUser(string username, string password)
        {
            var domainAndUsername = "local.st.by" + @"\" + username;
            var entry = new DirectoryEntry(_ldappath, domainAndUsername, password);

            var search = new DirectorySearcher(entry)
            {
                Filter = "(SAMAccountName=" + username + ")"
            };

            var searchResult = search.FindOne();
            if (null == searchResult)
            {
                return null;
            }

            return new DirectoryServicesUserInfo
            {
                GivenName = SafeExtractProperty(searchResult, "givenname"),
                FullName = SafeExtractProperty(searchResult, "cn"),
                EmailAddress = SafeExtractProperty(searchResult, "mail"),
                Guid = new Guid((byte[])searchResult.Properties["objectguid"][0]),
                EmployeeType = SafeExtractProperty(searchResult, "employeeType")
            };
        }

        private string SafeExtractProperty(SearchResult item, string propertyName)
        {
            if (item.Properties.Contains(propertyName) && item.Properties[propertyName].Count > 0)
            {
                return item.Properties[propertyName][0].ToString();
            }
            return string.Empty;
        }

        class DirectoryServicesUserInfo
        {
            public string FullName { get; set; }
            public string GivenName { get; set; }
            public string Surname { get; set; }
            public Guid? Guid { get; set; }
            public string EmailAddress { get; set; }
            public string UserPrincipalName { get; set; }
            public string EmployeeType { get; set; }
        }
        #endregion

        private void ButtonLogin_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var userInfo = GetDirectoryServicesUser(TextBox1.Text, PasswordBox1.Password);

                if (null != userInfo && userInfo.Guid.HasValue)
                {
                    using (CallRecordContext db = new CallRecordContext())
                    {
                        db.Configs.Load();
                        if (db.Configs.Local.Count == 0)
                        {
                            CreateNewUser();
                            OpenWindows();
                        }
                        else
                        {
                            foreach (var curItem in db.Configs.Local)
                            {
                                if (db.Configs.All(c => c.EmailAdress != curItem.EmailAdress))
                                {
                                    CreateNewUser();
                                    OpenWindows();
                                }
                                else
                                {
                                    OpenWindows();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                TextBox1.Text = String.Empty;
                PasswordBox1.Password = String.Empty;
            }

        }
        private void CreateNewUser()
        {
            var userInfo = GetDirectoryServicesUser(TextBox1.Text, PasswordBox1.Password);
            if (null != userInfo && userInfo.Guid.HasValue)
            {
                using (CallRecordContext db = new CallRecordContext())
                {
                    var config = new Config
                    {
                        EmailAdress = userInfo.EmailAddress,
                        FullName = userInfo.FullName
                    };
                    db.Configs.Add(config);
                    db.SaveChanges();
                }
            }
        }

        private void OpenWindows()
        {
            var userInfo = GetDirectoryServicesUser(TextBox1.Text, PasswordBox1.Password);
            CoreWindow nWindow1 = new CoreWindow(userInfo.FullName, userInfo.EmailAddress);
            nWindow1.InitializeComponent();
            Close();
            nWindow1.ShowDialog();
        }

        private static void AutoLoad()
        {
            RegistryKey regKey = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run\\");
            regKey.SetValue("MyApp", Assembly.GetExecutingAssembly().Location);
            regKey.Close();
        }
    }
}
