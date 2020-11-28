namespace Freesql.Tool.Models
{
    public class RazorTemplateModel
    {
        public CopyRightUserInfo UserInfo { get; set; }
        public string TableName { get; set; }
        public string NameSpace { get; set; } = "CloudB.Car.";
        public string Folder { get; set; }
        public string AssemblyOrConnect { get; set; }

        public string OutPath { get; set; }
    }
}
