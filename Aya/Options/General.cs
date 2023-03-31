using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Aya
{
    internal partial class OptionsProvider
    {
        // Register the options with this attribute on your package class:
        // [ProvideOptionPage(typeof(OptionsProvider.GeneralOptions), "Aya", "General", 0, 0, true, SupportsProfiles = true)]
        [ComVisible(true)]
        public class GeneralOptions : BaseOptionPage<General> { }
    }

    public class General : BaseOptionModel<General>
    {
        [Category("Aya")]
        [DisplayName("Duration")]
        [Description("Set how often you want to see an Aya (minutes), Maximum value is 120 and minimum is 20 minutes. the default value is 30 minutes")]
        [DefaultValue(30)]
        public int duration { get; set; } = 30;
        
    }
}
