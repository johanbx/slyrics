using System.Windows.Data;
 
namespace slyrics
{
    public class SettingBindingExtension : Binding
    {
        public SettingBindingExtension ()
        {
            Initialize();
        }

        public SettingBindingExtension (string path)
            : base(path)
        {
            Initialize();
        }

        private void Initialize ()
        {
            this.Source = slyrics.Properties.Settings.Default;
            this.Mode = BindingMode.TwoWay;
        }
    }
}