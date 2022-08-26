
using TheTechIdea.Beep.Vis;
using TheTechIdea.Util;

namespace BeepEnterprize.Winform.Vis.Controls.TreeControls
{
    public class MenuList
    {
        public MenuList()
        {

        }
        public MenuList(string objectType, string branchClass, EnumPointType pointType)
        {
            ObjectType = objectType;
            BranchClass = branchClass;
            PointType = pointType;
        }
        public ContextMenuStrip Menu { get; set; }
        public List<ToolStripMenuItem> Items { get; set; }
        public EnumPointType  PointType { get; set; }
        public string ObjectType { get; set; }
        public string BranchClass { get; set; } 
        public string branchname { get; set; }
        public List<AssemblyClassDefinition> classDefinitions { get; set; } = new List<AssemblyClassDefinition>();
    }
}
