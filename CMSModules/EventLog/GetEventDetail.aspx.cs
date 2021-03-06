using System;
using System.Web;
using System.Text;

using CMS.GlobalHelper;
using CMS.EventLog;
using CMS.UIControls;

public partial class CMSModules_EventLog_GetEventDetail : CMSEventLogPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        int eventId = QueryHelper.GetInteger("eventid", 0);
        EventLogInfo ev = EventLogProvider.GetEventLogInfo(eventId);
        // Set edited object
        EditedObject = ev;

        if (ev != null)
        {
            UTF8Encoding enc = new UTF8Encoding();
            string text = HTMLHelper.StripTags(HttpUtility.HtmlDecode(EventLogHelper.GetEventText(ev)));
            byte[] file = enc.GetBytes(text);

            Response.AddHeader("Content-disposition", "attachment; filename=eventdetails.txt");
            Response.ContentType = "text/plain";
            Response.BinaryWrite(file);

            RequestHelper.EndResponse();
        }
    }
}
