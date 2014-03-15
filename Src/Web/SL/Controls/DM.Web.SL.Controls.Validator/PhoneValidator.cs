using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace DM.Web.SL.Controls.Validator
{
    /// <summary>
    /// 电话号码验证器，输入格式如111 111 1111，输出(111) 111-1111。
    /// </summary>
    public class PhoneValidator : RegexValidator
    {
        #region Fields

        /// <summary>
        /// ApplyFormatProperty
        /// </summary>
        public static readonly DependencyProperty ApplyFormatProperty = 
            DependencyProperty.Register("ApplyFormat", typeof (bool), typeof (PhoneValidator),
                                        new PropertyMetadata(null));

        #endregion Fields

        #region Constructors

        /// <summary>
        /// 构造。
        /// </summary>
        public PhoneValidator()
        {
            Expression = @"^(1?)(\D*)([0-9]{3})(\D*)([0-9]{3})(\D*)([0-9]{4})$";
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// ApplyFormat。
        /// </summary>
        public bool ApplyFormat
        {
            get { return (bool) GetValue(ApplyFormatProperty); }
            set { SetValue(ApplyFormatProperty, value); }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// 丢失焦点事件。
        /// </summary>
        protected override void ElementToValidate_LostFocus(object sender, RoutedEventArgs e)
        {
            bool valid = Validate();

            if (valid && ApplyFormat)
            {
                //format phone number
                string formatted = DoFormat(((TextBox) ElementToValidate).Text);

                if (((TextBox) ElementToValidate).Text != formatted)
                    (ElementToValidate as TextBox).Text = formatted;
            }
        }

        /// <summary>
        /// 1111111111 => (111) 111-1111
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private static string AddFormat(string s)
        {
            if (string.IsNullOrEmpty(s))
                return "";

            s = StripFormat(s);

            if (s.Length != 10)
                return s;

            try
            {
                return ("(" + s.Substring(0, 3) + ") " + s.Substring(3, 3) + "-" + s.Substring(6, 4));
            }
            catch
            {
                return s;
            }
        }

        /// <summary>
        /// 111 111 1111 =>(111) 111-1111
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private static string DoFormat(string s)
        {
            string news = StripFormat(s);

            if (news.Length == 10)
                return AddFormat(news);
            return s;
        }

        /// <summary>
        /// 111 111 1111 => 1111111111
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private static string StripFormat(string s)
        {
            if (s == null)
                return null;

            Regex r = new Regex(@"\D");
            return r.Replace(s, "");
        }

        #endregion Methods
    }
}