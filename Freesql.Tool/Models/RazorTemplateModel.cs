namespace Freesql.Tool.Models
{
    public class RazorTemplateModel
    {
        public CopyRightUserInfo UserInfo { get; set; }
        public string TableName { get; set; }
        public string NameSpace { get; set; } = "Demo";
        public string Folder { get; set; }
        public string AssemblyOrConnect { get; set; }

        public string OutPath { get; set; }

        public string LowerTableName => TableName.Substring(0, 1).ToLower() + TableName.Substring(1);
    }
}
