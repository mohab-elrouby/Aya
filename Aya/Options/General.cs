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
        [Description("Set duration for how often you want to see an Aya")]
        [DefaultValue(100000)]
        public int duration { get; set; }
        
    }
}
