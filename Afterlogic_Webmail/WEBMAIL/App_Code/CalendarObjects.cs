using System;
using System.Collections.Generic;

[Serializable]
public class CalendarEvents
{
    private string _event_id;
    private string _calendar_id;
    private string _event_timefrom;
    private string _event_timetill;
    private string _event_allday;
    private string _event_name;
    private string _event_text;
    private string _event_priority;

    public string event_id
    {
        get { return _event_id; }
        set { _event_id = value; }
    }

    public string calendar_id
    {
        get { return _calendar_id; }
        set { _calendar_id = value; }
    }

    public string event_timefrom
    {
        get { return _event_timefrom; }
        set { _event_timefrom = value; }
    }

    public string event_timetill
    {
        get { return _event_timetill; }
        set { _event_timetill = value; }
    }

    public string event_allday
    {
        get { return _event_allday; }
        set { _event_allday = value; }
    }

    public string event_name
    {
        get { return _event_name; }
        set { _event_name = value; }
    }

    public string event_text
    {
        get { return _event_text; }
        set { _event_text = value; }
    }

    public string event_priority
    {
        get { return _event_priority; }
        set { _event_priority = value; }
    }
}
