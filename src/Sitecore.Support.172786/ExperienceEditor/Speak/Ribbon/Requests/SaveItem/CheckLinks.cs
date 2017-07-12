namespace Sitecore.Support.ExperienceEditor.Speak.Ribbon.Requests.SaveItem
{
    using System.Collections.Generic;
    using System.Text;
    using Sitecore.Data;
    using Sitecore.Data.Fields;
    using Sitecore.Data.Items;
    using Sitecore.ExperienceEditor.Speak.Server.Responses;
    using Sitecore.Globalization;
    using Sitecore.Links;

    public class CheckLinks : Sitecore.ExperienceEditor.Speak.Ribbon.Requests.SaveItem.CheckLinks
    {
        public override PipelineProcessorResponseValue ProcessRequest()
        {
            PipelineProcessorResponseValue value2 = new PipelineProcessorResponseValue();
            Item item = base.RequestContext.Item.Database.GetItem(base.RequestContext.ItemId, Language.Parse(base.RequestContext.Language), Version.Parse(base.RequestContext.Version));
            if (item != null)
            {
                ItemLink[] brokenLinks = item.Links.GetBrokenLinks(false);
                if (brokenLinks.Length <= 0)
                {
                    return value2;
                }
                StringBuilder builder = new StringBuilder(Translate.Text("The item \"{0}\" contains broken links in these fields:\n\n", new object[] { item.DisplayName }));
                bool flag = false;
                #region Added code
                Dictionary<string, int> fields = new Dictionary<string, int>(); // list of fields and count of broken links
                #endregion
                foreach (ItemLink link in brokenLinks)
                {
                    if (!link.SourceFieldID.IsNull)
                    {
                        Field field = item.Fields[link.SourceFieldID];
                        #region Removed code
                        //builder.Append(" - ");
                        //builder.Append((field != null) ? field.DisplayName : Translate.Text("[Unknown field: {0}]", new object[] { link.SourceFieldID.ToString() }));
                        //if (!string.IsNullOrEmpty(link.TargetPath) && !ID.IsID(link.TargetPath))
                        //{
                        //    builder.Append(": \"");
                        //    builder.Append(link.TargetPath);
                        //    builder.Append("\"");
                        //}
                        //builder.Append("\n"); 
                        #endregion
                        #region Added code
                        string name = (field != null) ? field.DisplayName : Translate.Text("[Unknown field: {0}]", new object[] { link.SourceFieldID.ToString() });
                        if (!fields.ContainsKey(name))
                        {
                            fields.Add(name, 1);
                        }
                        else
                        {
                            fields[name]++;
                        } 
                        #endregion
                    }
                    else
                    {
                        flag = true;
                    }
                }
                if (flag)
                {
                    builder.Append("\n");
                    builder.Append(Translate.Text("The template or branch for this item is missing."));
                }
                #region Added code
                foreach (KeyValuePair<string, int> f in fields) // display validation message with broken links count instead of links values
                {
                    builder.Append(" - ");
                    builder.Append(f.Key);
                    builder.Append(" (");
                    builder.Append(f.Value);
                    builder.Append(")");
                }
                #endregion
                builder.Append("\n ");
                builder.Append(Translate.Text("Do you want to save anyway?"));
                value2.ConfirmMessage = builder.ToString();
            }
            return value2;
        }
    }
}