using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CodeFirst
{
    class Class1
    {
        private void AddBankButton_OnClick(object sender, RoutedEventArgs e)
        {
            Window2 w2 = new Window2(MainGrid.ActualHeight);
            w2.ShowDialog();

            using (CallRecordContext db = new CallRecordContext())
            {
                db.Configs.Load();
                var configs = db.Configs;
                var query =
                    from config in configs
                    select new { config.Email };
                var list = query.ToList();
                foreach (var curItem in list)
                {
                    if (eMail == curItem.Email)
                    {

                    }
                }

            }
        }
    }
}
