using System.Windows.Forms;

namespace AIOFlipper
{
    public partial class AccountNameTagUC : UserControl
    {
        private string accountName;

        public AccountNameTagUC(string accountName)
        {
            InitializeComponent();

            this.accountName = accountName;
            labelAccountName.Text = accountName;
        }

        public AccountNameTagUC()
        {
            InitializeComponent();
            this.accountName = labelAccountName.Text;
        }

        public string AccountName
        {
            get
            {
                return accountName;
            }
            set
            {
                accountName = value;
                labelAccountName.Text = accountName;
            }
        }
    }
}
