
using TheTechIdea;

namespace Beep.Winform.Vis
{
    public class AddinsShownData
    {
        public AddinsShownData() { }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public IDM_Addin Addin { get; set; }
        public string GuidID { get; set; } = Guid.NewGuid().ToString();
        public bool IsSingleton { get; set; } = false;
        public bool IsShown { get; set; }=false;
        public bool IsHidden { get; set; } = false;
        public bool IsAdded { get; set; } = false;
        public bool IsRemoved { get; set; } = false;
    }
}
